using UnityEngine;

public class AnimatorBehaviourBase : StateMachineBehaviour
{
	float enterTime;
	public float NormalizedTime { get; private set; }
	public bool IsTransition { get; private set; }

	public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		NormalizedTime = 0.0f;
		IsTransition = animator.IsInTransition(layerIndex);
		enterTime = Time.time;
		StateEnter(animator, stateInfo, layerIndex);
	}

	public virtual void StateEnter(Animator animator, AnimatorStateInfo stateinfo, int layerIndex)
	{
	}

	public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		if(!IsTransition) NormalizedTime = stateInfo.normalizedTime - enterTime;
		IsTransition = animator.IsInTransition(layerIndex);
		StateUpdate(animator, stateInfo, layerIndex);
	}

	public virtual void StateUpdate(Animator animator, AnimatorStateInfo stateinfo, int layerIndex)
	{
	}

	public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		StateExit(animator, stateInfo, layerIndex);
	}

	public virtual void StateExit(Animator animator, AnimatorStateInfo stateinfo, int layerIndex)
	{
	}

	public void ResetTime(float time)
	{
		enterTime = Time.time;
		NormalizedTime = 0.0f;
	}
}