using System;
using System.Collections;
using MyTonaTechExec.PlayerUnit;
using MyTonaTechExec.Utils;
using UnityEngine;

namespace MyTonaTechExec.MobUnit
{
    [RequireComponent(typeof(MobMover))]
    [RequireComponent(typeof(Mob))]
    public class MeleeAttack : MonoBehaviour, IAttack, IMobComponent
    {
        public float AttackDistance = 1f;
        public float DamageDistance = 1f;
        public float AttackDelay = 1f;

        private MobMover mover;
        private Mob mob;
        private MobAnimator mobAnimator;
        private bool attacking;
        private Coroutine _attackCoroutine;

        public event Action OnAttack;

        private void Awake()
        {
            mob = GetComponent<Mob>();
            mover = GetComponent<MobMover>();
            mobAnimator = GetComponent<MobAnimator>();
            EventBus.EventBus.Sub(OnDeath, EventBus.EventBus.PLAYER_DEATH);
        }

        private void OnDestroy()
        {
            EventBus.EventBus.Unsub(OnDeath, EventBus.EventBus.PLAYER_DEATH);
        }

        private void Update()
        {
            if (attacking) return;

            var playerDistance = (transform.position - Player.Instance.transform.position).Flat().magnitude;
            if (!(playerDistance <= AttackDistance)) return;
            
            attacking = true;
            _attackCoroutine = StartCoroutine(Attack());
        }

        private IEnumerator Attack()
        {
            OnAttack?.Invoke();

            mobAnimator.StartAttackAnimation();
            mover.Active = false;

            yield return new WaitForSeconds(AttackDelay);

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
}