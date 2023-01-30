using System;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
	public Animator Animator;
	
	private void Awake()
	{
		EventBus<PlayerInputMessage>.Sub((message) =>
		{
			Animator.SetBool("IsRun",message.MovementDirection.sqrMagnitude > 0);
		});
		EventBus.Sub(AnimateDeath,EventBus.PLAYER_DEATH);
	}

	private void OnDestroy()
	{
		EventBus.Unsub(AnimateDeath,EventBus.PLAYER_DEATH);
	}

	private void AnimateDeath()
	{
		Animator.SetTrigger("Death");
	}

	public void TriggerShoot()
	{
		Animator.SetTrigger("Shoot");
	}
}