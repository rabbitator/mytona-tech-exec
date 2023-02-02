using System;
using System.Collections;
using MyTonaTechExec.PlayerUnit;
using MyTonaTechExec.Utils;
using UnityEngine;

namespace MyTonaTechExec.MobUnit
{
    [RequireComponent(typeof(MobMover))]
    [RequireComponent(typeof(Mob))]
    public class MobAttack : MonoBehaviour, IMobComponent
    {
        [SerializeField]
        protected float AttackDistance = 5f;
        [SerializeField]
        protected float AttackDelay = .5f;
        [SerializeField]
        protected float AttackCooldown = 2f;

        protected MobMover mover;
        protected Mob mob;
        protected MobAnimator mobAnimator;
        protected bool attacking;
        protected Coroutine _attackCoroutine;

        private event EventHandler _onAttack;
        public event EventHandler OnAttack
        {
            add => _onAttack += value;
            remove => _onAttack -= value;
        }

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

            var distanceToPlayer = (transform.position - Player.Instance.transform.position).Flat().magnitude;
            if (distanceToPlayer > AttackDistance) return;

            attacking = true;
            _attackCoroutine = StartCoroutine(Attack());
        }

        protected virtual IEnumerator Attack()
        {
            _onAttack?.Invoke(this, EventArgs.Empty);

            return null;
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