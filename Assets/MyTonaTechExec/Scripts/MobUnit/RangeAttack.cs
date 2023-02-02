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
    }
}