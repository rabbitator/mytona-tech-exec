using System;
using System.Collections;
using MyTonaTechExec.PlayerUnit;
using MyTonaTechExec.Utils;
using UnityEngine;
using UnityEngine.Serialization;

namespace MyTonaTechExec.MobUnit
{
    [RequireComponent(typeof(MobMover))]
    [RequireComponent(typeof(Mob))]
    public class MobAttack : MonoBehaviour, IMobComponent
    {
        [FormerlySerializedAs("AttackDistance")]
        [SerializeField]
        protected float _attackDistance = 5f;
        [FormerlySerializedAs("AttackDelay")]
        [SerializeField]
        protected float _attackDelay = .5f;
        [FormerlySerializedAs("AttackCooldown")]
        [SerializeField]
        protected float _attackCooldown = 2f;

        protected MobMover _mobMover;
        protected Mob _mob;
        protected MobAnimator _mobAnimator;
        protected bool _attacking;
        protected Coroutine _attackCoroutine;

        private event EventHandler _onAttack;
        public event EventHandler OnAttack
        {
            add => _onAttack += value;
            remove => _onAttack -= value;
        }

        private void Awake()
        {
            _mob = GetComponent<Mob>();
            _mobMover = GetComponent<MobMover>();
            _mobAnimator = GetComponent<MobAnimator>();
            EventBus.EventBus.Sub(OnDeath, EventBus.EventBus.PLAYER_DEATH);
        }

        private void OnDestroy()
        {
            EventBus.EventBus.Unsub(OnDeath, EventBus.EventBus.PLAYER_DEATH);
        }

        private void Update()
        {
            if (_attacking) return;

            var distanceToPlayer = (transform.position - Player.Instance.transform.position).Flat().magnitude;
            if (distanceToPlayer > _attackDistance) return;

            _attacking = true;
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