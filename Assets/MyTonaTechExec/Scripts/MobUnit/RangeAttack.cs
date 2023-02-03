using System.Collections;
using MyTonaTechExec.PlayerUnit;
using MyTonaTechExec.Projectiles;
using MyTonaTechExec.Utils;
using UnityEngine;

namespace MyTonaTechExec.MobUnit
{
    public class RangeAttack : MobAttack
    {
        [Space, SerializeField]
        private Projectile Bullet;
        
        protected override IEnumerator Attack()
        {
            _ = base.Attack();
            
            _mobAnimator.StartAttackAnimation();
            _mobMover.Active = false;
            
            yield return new WaitForSeconds(_attackDelay);
            
            var playerDistance = (transform.position - Player.Instance.transform.position).Flat().magnitude;
            if (playerDistance <= _attackDistance)
            {
                var playerDirection = (Player.Instance.transform.position - transform.position).Flat().normalized;
                var bullet = Instantiate(Bullet, transform.position, Quaternion.LookRotation(playerDirection, Vector3.up));
                bullet.Damage = _mob.Damage;
            }

            _mobMover.Active = true;
            yield return new WaitForSeconds(_attackCooldown);
            _attacking = false;
            _attackCoroutine = null;
        }
    }
}