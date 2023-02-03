using System;
using UnityEngine;

namespace MyTonaTechExec.MobUnit
{
	public class MobAnimator : MonoBehaviour,IMobComponent
	{
		public Animator Animator;
		public string AttackTrigger = "MeleeAttack";

		private void Awake()
		{
			EventBus.EventBus.Sub(PlayerDeath, EventBus.EventBus.PLAYER_DEATH);
		}

		private void OnDestroy()
		{
			EventBus.EventBus.Unsub(PlayerDeath, EventBus.EventBus.PLAYER_DEATH);
		}

		public void StartAttackAnimation()
		{
			Animator.SetTrigger(AttackTrigger);
		}
	
		public void SetIsRun(bool isRun)
		{
			Animator.SetBool("Run",isRun);
		}
	
		public void OnDeath()
		{
			Animator.SetTrigger("Death");
		}

		private void PlayerDeath()
		{
			SetIsRun(false);
		}
	}
}