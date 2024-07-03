using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using Unity.VisualScripting;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using UnityEngine.UIElements;

/// <summary>
/// 敌人类
/// 实现状态切换，加载敌人巡逻路线
/// </summary>
public class Enemy : MonoBehaviour
{
    private NavMeshAgent agent;
    public Animator animator;
    private AudioSource audioSource;

    [Tooltip("敌人血量")]public float enemyHealth;
    [Tooltip("敌人血条")]public UnityEngine.UI.Slider slider;
    [Tooltip("敌人受到伤害文字UI")]public Text getDamageText;
    [Tooltip("敌人死亡特效")]public GameObject deadEffect;

    public GameObject[] wayPointObj;//存放敌人不同路线
    public List<Vector3> wayPoints = new List<Vector3>();//存放巡逻路线的每个巡逻点
    public int index;//下标值
    [Tooltip("敌人下标(用来分配随机路线)")]public int nameIndex;
    public int animState;//动画状态标识，0：idle， 1：run， 2：attack
    public Transform targetPoint;//目标位置

    public EnemyBaseState currentState;//存储敌人当前的状态
    public PatrolState patrolState;//定义敌人巡逻状态，声明对象
    public AttackState attackState;//定义敌人攻击状态，生命对象

    Vector3 targetPosition;
    //敌人的攻击目标，场景中有敌人（玩家）用列表存储
    public List<Transform> attackList = new List<Transform>();
    [Tooltip("攻击间隔，时间越长攻击频率越慢")]public float attackRate;
    private float nextAttack = 0;//下次攻击时间
    [Tooltip("普通攻击距离")]public float attackRange;
    private bool isDead;//判断是否死亡

    public GameObject attackParticle01;
    public Transform attackParticle01Position;
    public AudioClip attackSound;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        patrolState = transform.gameObject.AddComponent<PatrolState>();
        attackState = transform.gameObject.AddComponent<AttackState>();
    }

    // Start is called before the first frame update
    void Start()
    {
        isDead = false;
        slider.minValue = 0;
        slider.maxValue = enemyHealth;
        slider.value = enemyHealth;
        index = 0;
        //游戏一开始的时候敌人进入巡逻状态
        TransitionToState(patrolState);

    }

    // Update is called once per frame
    void Update()
    {
        if (isDead) return;
        //这里是表示当前状态持续执行
        //敌人移动的方法要一直执行
        currentState.OnUpdate(this);
        animator.SetInteger("state", animState);
    }

    /// <summary>
    /// 敌人想着导航点移动
    /// </summary>
    public void MoveToTaget()
    {
        if (attackList.Count == 0)
        {
            //敌人没有攻击目标，走巡逻点
            targetPosition = Vector3.MoveTowards(transform.position, wayPoints[index], agent.speed * Time.deltaTime);
        }
        else
        {
            //敌人扫描到玩家，向玩家方向走去
            targetPosition = Vector3.MoveTowards(transform.position, attackList[0].transform.position, agent.speed * Time.deltaTime);
        }

        agent.destination = targetPosition;
    }

    /// <summary>
    /// 加载路线
    /// </summary>
    public void LoadPath(GameObject go)
    {
        //加载路线之前清空 list
        wayPoints.Clear();
        //遍历路线预制体里所有导航点位置信息，并加到 list 里
        foreach (Transform T in go.transform)
        {
            wayPoints.Add(T.position);
        }
    }

    /// <summary>
    /// 切换敌人状态
    /// </summary>
    public void TransitionToState(EnemyBaseState state)
    {
        currentState = state;
        currentState.EnemyState(this);
    }

    /// <summary>
    /// 敌人受到伤害，扣除血量
    /// </summary>
    /// <param name="damage"></param>
    public void Health(float damage)
    {
        if (isDead) return;
        getDamageText.text = Mathf.Round(damage).ToString();
        enemyHealth -= damage;
        slider.value = enemyHealth;
        if (slider.value <= 0)
        {
            isDead = true;
            animator.SetTrigger("dying");
            slider.gameObject.SetActive(false);
            Destroy(Instantiate(deadEffect, transform.position, Quaternion.identity), 3f);

        }
    }

    /// <summary>
    /// 敌人攻击玩家
    /// 普通攻击
    /// </summary>
    public void AttackAction()
    {
        //当敌人和玩家距离很近的时候，触发攻击动画
        if (Vector3.Distance(transform.position, targetPoint.position) < attackRange)
        {
            if (Time.time > nextAttack)
            {
                //触发攻击
                animator.SetTrigger("attack");
                //更新下次攻击事件
                nextAttack = Time.time + attackRate;
            }
        }
    }

    /// <summary>
    /// attack 动画
    /// Animation Event
    /// </summary>
    public void PlayAttackSound()
    {
        audioSource.clip = attackSound;
        audioSource.Play();
    }

    /// <summary>
    /// Animation Event
    /// </summary>
    public void PlayMustantAttackEff()
    {
        if (gameObject.name == "Mutant")
        {
            GameObject attackPar01 = Instantiate(attackParticle01, attackParticle01Position.position, attackParticle01Position.rotation);
            PlayAttackSound();
            Destroy(attackPar01, 3f);   
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //攻击列表里不要剔除子弹，子弹不能添加
        if (!attackList.Contains(other.transform) && !isDead && !other.CompareTag("Bullet"))
        {
            attackList.Add(other.transform);
        }    
    }

    private void OnTriggerExit(Collider other)
    {
        attackList.Remove(other.transform);
    }
}
