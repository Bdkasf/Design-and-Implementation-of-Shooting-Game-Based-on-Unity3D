using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class ReloadAmmoState : StateMachineBehaviour
{
    /* 霰弹枪用 */
    public float reloadTime = 0.8f;// reload 动画播放到多少％部分
    private bool hasReload;//判断 当前是否在换弹

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       hasReload = false;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       if (hasReload) return;
       if (stateInfo.normalizedTime >= reloadTime)
       {
            if (animator.GetComponent<Weapon_AutomaticGun>() != null)
            {
                animator.GetComponent<Weapon_AutomaticGun>().ShotGunReload();
            }
            hasReload = true;
       }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       hasReload = false;
    }
}
