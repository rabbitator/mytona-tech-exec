using System.Collections;
using MyTonaTechExec.Common;
using MyTonaTechExec.PlayerUnit;
using MyTonaTechExec.Utils;
using UnityEngine;

namespace MyTonaTechExec.MobUnit
{
	[RequireComponent(typeof(MobMover))]
	[RequireComponent(typeof(Mob))]
	public class RangeAttack : MonoBehaviour,IMobComponent
	{
		public float AttackDistance = 5f;
		public float AttackDelay = .5f;
		public float AttackCooldown = 2f;
		public Projectile Bullet;
    
		private MobMover mover;
		private Mob mob;
		private MobAnimator mobAnimator;
		private bool attacking = false;
		private Coroutine _attackCoroutine = null;

		private void Awake()
		{
			mob = GetComponent<Mob>();
			mover = GetComponent<MobMover>();
			mobAnimator = GetComponent<MobAnimator>();
			EventBus.EventBus.Sub(OnDeath,EventBus.EventBus.PLAYER_DEATH);
		}

		private void OnDestroy()
		{
			EventBus.EventBus.Unsub(OnDeath,EventBus.EventBus.PLAYER_DEATH);
		}

		private void Update()
		{
			if (attacking)
			{
				return;
			}
			var playerDistance = (transform.position - Player.Instance.transform.position).Flat().magnitude;
			if (playerDistance <= AttackDistance)
			{
				attacking = true;
				_attackCoroutine = StartCoroutine(Attack());
			}
		}

		private IEnumerator Attack()
		{
			mobAnimator.StartAttackAnimation();
			mover.Active = false;
			yield return  new WaitForSeconds(AttackDelay);
			var playerDistance = (transform.position - Player.Instance.transform.position).Flat().magnitude;
			if (playerDistance <= AttackDistance)
			{
				var bullet = Instantiate(Bullet, transform.position,
					Quaternion.LookRotation((Player.Instance.transform.position - transform.position).Flat().normalized,
						Vector3.up));
				bullet.Damage = mob.Damage;
			}
			mover.Active = true;
			yield return  new WaitForSeconds(AttackCooldown);
			attacking = false;
			_attackCoroutine = null;
		}
	
		public void OnDeath()
		{
			enabled = false;
			if (_attackCoroutine != null)
			{
				StopCoroutine(_attackCoroutine);
			}
		}
	}
}