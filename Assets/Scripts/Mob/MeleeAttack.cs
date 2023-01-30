using System;
using System.Collections;
using Unity.Mathematics;
using UnityEngine;

[RequireComponent(typeof(MobMover))]
[RequireComponent(typeof(Mob))]
public class MeleeAttack : MonoBehaviour, IMobComponent
{
	public float AttackDistance = 1f;
	public float DamageDistance = 1f;

	public float AttackDelay = 1f;
    
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
		EventBus.Sub(OnDeath,EventBus.PLAYER_DEATH);
	}

	private void OnDestroy()
	{
		EventBus.Unsub(OnDeath,EventBus.PLAYER_DEATH);
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
		if (playerDistance <= DamageDistance)
		{
			Player.Instance.TakeDamage(mob.Damage);
		}
		mover.Active = true;
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