using MyTonaTechExec.EventBus;
using MyTonaTechExec.EventBus.Messages;
using UnityEngine;
using UnityEngine.Serialization;

namespace MyTonaTechExec.PlayerUnit
{
    public class PlayerAnimator : MonoBehaviour
    {
        [FormerlySerializedAs("animator")]
        [SerializeField]
        private Animator _animator;

        private static readonly int AnimatorHashIsRun = Animator.StringToHash("IsRun");
        private static readonly int AnimatorHashDeath = Animator.StringToHash("Death");
        private static readonly int AnimatorHashShoot = Animator.StringToHash("Shoot");

        private void Awake()
        {
            EventBus<PlayerInputMessage>.Sub(PlayRun);
            EventBus.EventBus.Sub(AnimateDeath, EventBus.EventBus.PLAYER_DEATH);
        }

        private void OnDestroy()
        {
            EventBus<PlayerInputMessage>.Unsub(PlayRun);
            EventBus.EventBus.Unsub(AnimateDeath, EventBus.EventBus.PLAYER_DEATH);
        }

        private void PlayRun(PlayerInputMessage message)
        {
            _animator.SetBool(AnimatorHashIsRun, message.MovementDirection.sqrMagnitude > 0);
        }

        private void AnimateDeath()
        {
            _animator.SetTrigger(AnimatorHashDeath);
        }

        public void TriggerShoot()
        {
            _animator.SetTrigger(AnimatorHashShoot);
        }
    }
}