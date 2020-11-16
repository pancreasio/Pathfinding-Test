using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolBehaviour : StateMachineBehaviour
{
    protected Transform thisTransform;
    protected Animator currentAnimator;
    protected bool isSeeker;
    protected Seeker seeker;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);
        currentAnimator = animator;
        thisTransform = animator.transform;
        seeker = thisTransform.GetComponent<Seeker>();
        if (seeker != null)
        {
            seeker.BeginPatrol();
            seeker.OnMineFound += MineFound;
            isSeeker = true;
        }
        else
        {
            Debug.Log("not a seeker");
            isSeeker = false;
        }
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateUpdate(animator, stateInfo, layerIndex);
    }

    private void MineFound()
    {
        if (isSeeker)
        {
            seeker.OnMineFound -= MineFound;
            seeker.StopPatrol();
            currentAnimator.SetTrigger("MARK");
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
