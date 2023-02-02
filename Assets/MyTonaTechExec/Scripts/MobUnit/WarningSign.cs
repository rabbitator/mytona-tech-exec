using UnityEngine;

namespace MyTonaTechExec.MobUnit
{
    [RequireComponent(typeof(Animator))]
    public class WarningSign : MonoBehaviour
    {
        private IAttack _mobAttack;
        private Animator _animator;
        private Camera _camera;

        private readonly int _animatorHashActivate = Animator.StringToHash("Active");

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _mobAttack = GetComponentInParent<IAttack>();
            _camera = Camera.main;

            _mobAttack.OnAttack += MobAttackHandler;
        }

        private void Update()
        {
            transform.rotation = _camera.transform.rotation;
        }

        private void MobAttackHandler()
        {
            _animator.SetTrigger(_animatorHashActivate);
        }
    }
}