using UnityEngine;
using System;

public class AnimatorBehaviour : StateMachineBehaviour
{
    float enterTime = 0.0f;
	public float NormalizedTime { get; private set; }
	public bool IsTransition { get; private set; }
	public Action EndCallBack {private get;set;}
	 = ()=>{/* Debug.LogWarning("empty animator callback");*/
	 };

	public virtual void StateEnter(Animator animator
	, AnimatorStateInfo stateinfo
	, int layerIndex)
	{
	}
	public override void OnStateEnter(Animator animator
	, AnimatorStateInfo stateInfo
	, int layerIndex)
	{
		NormalizedTime = 0.0f;
		IsTransition = 
		animator.IsInTransition(layerIndex);
		enterTime = Time.time;
		StateEnter(animator, stateInfo, layerIndex);
	}


	public virtual void StateUpdate(Animator animator
	, AnimatorStateInfo stateinfo
	, int layerIndex)
	{
	}

	public override void OnStateUpdate(Animator animator
	, AnimatorStateInfo stateInfo
	, int layerIndex)
	{
        if (IsTransition == false)
		{
		  NormalizedTime = 
		  ((Time.time - enterTime) * stateInfo.speed) 
		  / stateInfo.length;
        }
		IsTransition = animator.IsInTransition(layerIndex);
		StateUpdate(animator, stateInfo, layerIndex);
	}

    public virtual void StateExit(Animator animator
        , AnimatorStateInfo stateinfo
        , int layerIndex)
    {
    }

	public override void OnStateExit(Animator animator
	, AnimatorStateInfo stateInfo
	, int layerIndex)
	{
		StateExit(animator, stateInfo, layerIndex);
	}

	public void ResetTime()
	{
		enterTime = Time.time;
		NormalizedTime = 0.0f;
		EndCallBack();
	}
}
