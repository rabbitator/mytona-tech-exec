using UnityEngine;

namespace MyTonaTechExec.MobUnit
{
	public class MobAnimator : MonoBehaviour,IMobComponent
	{
		public Animator Animator;
		public string AttackTrigger = "MeleeAttack";

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
	}
}