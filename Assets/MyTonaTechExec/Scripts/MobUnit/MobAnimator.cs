using UnityEngine;
using UnityEngine.Serialization;

namespace MyTonaTechExec.MobUnit
{
    public class MobAnimator : MonoBehaviour, IMobComponent
    {
        [FormerlySerializedAs("Animator")]
        [SerializeField]
        private Animator _animator;
        [FormerlySerializedAs("AttackTrigger")]
        [SerializeField]
        private string _attackTrigger = "MeleeAttack";

        private static readonly int AnimatorHashRun = Animator.StringToHash("Run");
        private static readonly int AnimatorHashDeath = Animator.StringToHash("Death");

        private void Awake()
        {
            EventBus.EventBus.Sub(PlayerDeath, EventBus.EventBus.PLAYER_DEATH);
        }

        private void OnDestroy()
        {
            EventBus.EventBus.Unsub(PlayerDeath, EventBus.EventBus.PLAYER_DEATH);
        }

        public void StartAttackAnimation()
        {
            _animator.SetTrigger(_attackTrigger);
        }

        public void SetIsRun(bool isRun)
        {
            _animator.SetBool(AnimatorHashRun, isRun);
        }

        public void OnDeath()
        {
            _animator.SetTrigger(AnimatorHashDeath);
        }

        private void PlayerDeath()
        {
            SetIsRun(false);
        }
    }
}