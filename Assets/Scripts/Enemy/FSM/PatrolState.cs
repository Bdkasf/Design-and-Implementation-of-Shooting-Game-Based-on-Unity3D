using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 敌人进入巡逻状态
/// </summary>
public class PatrolState : EnemyBaseState
{
    public override void EnemyState(Enemy enemy)
    {
        enemy.animState = 0;
        //随机加载路线
        enemy.LoadPath (enemy.wayPointObj[WayPointManager.Instance.usingIndex[enemy.nameIndex]]);
    }
    public override void OnUpdate(Enemy enemy)
    {
        //判断 如果当前 idle 动画已经播放完以后，才能执行移动
        if (!enemy.animator.GetCurrentAnimatorStateInfo(0).IsName("idle"))
        {
            enemy.animState = 1;
            enemy.MoveToTaget();
        }


        //计算敌人和导航点的距离
        float distance = Vector3.Distance(enemy.transform.position, enemy.wayPoints[enemy.index]);
        //距离很小的时候表示已经走到了导航点
        if (distance <= 3f)
        {
            enemy.animState = 0;
            enemy.animator.Play("idle");
            enemy. index ++;//设置下一个导航点
            enemy.index = Mathf.Clamp(enemy.index, 0, enemy.wayPoints.Count - 1);
            //这里判断在此判断敌人和巡逻路线上最后1个导航点的距离，如果距离很小，那么当前路线已经走完，就重置导航点下标，使其重新又走一遍
            if (Vector3.Distance(enemy.transform.position, enemy.wayPoints[enemy.wayPoints.Count-1]) <= 3f)
            {
                enemy.index = 0;
            }
        }

        //判断，敌人巡逻扫描范围内出现敌人，此时进入攻击状态
        if (enemy.attackList.Count > 0)
        {
            enemy.TransitionToState(enemy.attackState);
        }
    }
}
