using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    [SerializeField]
    private Animator animator;

    private void Awake()
    {
        EventBus<PlayerInputMessage>.Sub(PlayRun);
        EventBus.Sub(AnimateDeath, EventBus.PLAYER_DEATH);
    }

    private void OnDestroy()
    {
        EventBus<PlayerInputMessage>.Unsub(PlayRun);
        EventBus.Unsub(AnimateDeath, EventBus.PLAYER_DEATH);
    }

    private void PlayRun(PlayerInputMessage message)
    {
        animator.SetBool("IsRun", message.MovementDirection.sqrMagnitude > 0);
    }

    private void AnimateDeath()
    {
        animator.SetTrigger("Death");
    }

    public void TriggerShoot()
    {
        animator.SetTrigger("Shoot");
    }
}