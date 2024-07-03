using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 敌人进入攻击状态
/// </summary>
public class AttackState : EnemyBaseState
{
    public override void EnemyState(Enemy enemy)
    {
        enemy.animState = 2;
        enemy.targetPoint = enemy.attackList[0];
    }
    public override void OnUpdate(Enemy enemy)
    {
        //当前敌人没有目标，此时敌人切换回巡逻状态
        if (enemy.attackList.Count == 0)
        {
            enemy.TransitionToState(enemy.patrolState);
        }

        //当前敌人有目标，可能存在多个目标，要找距离最近的攻击目标
        if (enemy.attackList.Count > 1)
        {
            for (int i = 0; i < enemy.attackList.Count; i++)
            {
                //判断，敌人和攻击列表里的多个目标距离差 比上 敌人和第1个目标的距离差 要小
                //说明第i个目标的距离，离敌人更远，在此更新敌人目标
                if (Mathf.Abs(enemy.transform.position.x - enemy.attackList[i].position.x) < Mathf.Abs(enemy.transform.position.x - enemy.targetPoint.position.x))
                {
                    enemy.targetPoint = enemy.attackList[i];
                }
            }
        }
        //当敌人只有1个攻击目标时，就只找 List 里第1个
        if (enemy.attackList.Count == 1)
        {
            enemy.targetPoint = enemy.attackList[0];
        }

        //敌人攻击玩家
        if (enemy.targetPoint.tag == "Player")
        {
            //敌人要对玩家进行攻击
            enemy.AttackAction();
        }

        enemy.MoveToTaget();
    }
}
