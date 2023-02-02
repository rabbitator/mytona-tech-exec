using System.Collections;
using MyTonaTechExec.PlayerUnit;
using MyTonaTechExec.Utils;
using UnityEngine;

namespace MyTonaTechExec.MobUnit
{
    public class MeleeAttack : MobAttack
    {
        [Space, SerializeField]
        private float _damageDistance = 1.0f;
        
        protected override IEnumerator Attack()
        {
            _ = base.Attack();

            mobAnimator.StartAttackAnimation();
            mover.Active = false;

            yield return new WaitForSeconds(AttackDelay);

            var playerDistance = (transform.position - Player.Instance.transform.position).Flat().magnitude;
            if (playerDistance <= _damageDistance)
            {
                Player.Instance.TakeDamage(mob.Damage);
            }

            mover.Active = true;
            attacking = false;
            _attackCoroutine = null;
        }
    }
}