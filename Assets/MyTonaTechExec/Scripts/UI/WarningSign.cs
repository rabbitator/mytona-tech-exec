using System;
using MyTonaTechExec.MobUnit;
using UnityEngine;

namespace MyTonaTechExec.UI
{
    [RequireComponent(typeof(Animator))]
    public class WarningSign : MonoBehaviour
    {
        [SerializeField]
        private MobAttack _mobAttack;
        
        private Animator _animator;

        private readonly int _animatorHashActivate = Animator.StringToHash("Active");

        private void Awake()
        {
            _animator = GetComponent<Animator>();

            _mobAttack.OnAttack += MobAttackHandler;
        }

        private void MobAttackHandler(object sender, EventArgs eventArgs)
        {
            _animator.SetTrigger(_animatorHashActivate);
        }
    }
}