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

            _mobAnimator.StartAttackAnimation();
            _mobMover.Active = false;

            yield return new WaitForSeconds(_attackDelay);

            var playerDistance = (transform.position - Player.Instance.transform.position).Flat().magnitude;
            if (playerDistance <= _damageDistance)
            {
                Player.Instance.TakeDamage(_mob.Damage);
            }

            _mobMover.Active = true;
            _attacking = false;
            _attackCoroutine = null;
        }
    }
}