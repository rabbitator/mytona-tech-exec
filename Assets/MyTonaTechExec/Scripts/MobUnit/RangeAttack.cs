using System;
using System.Collections;
using MyTonaTechExec.PlayerUnit;
using MyTonaTechExec.Projectiles;
using MyTonaTechExec.Utils;
using UnityEngine;

namespace MyTonaTechExec.MobUnit
{
    [RequireComponent(typeof(MobMover))]
    [RequireComponent(typeof(Mob))]
    public class RangeAttack : MonoBehaviour, IAttack, IMobComponent
    {
        public float AttackDistance = 5f;
        public float AttackDelay = .5f;
        public float AttackCooldown = 2f;
        public Projectile Bullet;

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

            var distanceToPlayer = (transform.position - Player.Instance.transform.position).Flat().magnitude;
            if (distanceToPlayer > AttackDistance) return;

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
            if (playerDistance <= AttackDistance)
            {
                var playerDirection = (Player.Instance.transform.position - transform.position).Flat().normalized;
                var bullet = Instantiate(Bullet, transform.position, Quaternion.LookRotation(playerDirection, Vector3.up));
                bullet.Damage = mob.Damage;
            }

            mover.Active = true;
            yield return new WaitForSeconds(AttackCooldown);
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