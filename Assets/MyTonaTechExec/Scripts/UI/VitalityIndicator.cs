using System;
using MyTonaTechExec.MobUnit;
using MyTonaTechExec.PlayerUnit;
using TMPro;
using UnityEngine;

namespace MyTonaTechExec.UI
{
    [RequireComponent(typeof(Animator))]
    public class VitalityIndicator : MonoBehaviour
    {
        [Header("Character"), SerializeField]
        private GameObject _character;

        [Space, Header("UI"), SerializeField]
        private TMP_Text _text;
        [SerializeField]
        private Color _addColor = Color.green;
        [SerializeField]
        private Color _subtractColor = Color.red;

        private Animator _animator;
        private Player _player;
        private Mob _mob;

        private static readonly int AnimatorHashActive = Animator.StringToHash("Active");

        private void Awake()
        {
            if (_character == null)
            {
                throw new NullReferenceException($"({nameof(VitalityIndicator)}) {nameof(_character)} field is empty!)");
            }

            _animator = GetComponent<Animator>();
            _player = _character.GetComponent<Player>();
            _mob = _character.GetComponent<Mob>();

            if (_player != null) _player.OnHPChange += HpChangeHandler;
            if (_mob != null) _mob.OnHpChange += HpChangeHandler;

            if (_player == null && _mob == null)
            {
                throw new Exception($"({nameof(VitalityIndicator)}) Can't get {nameof(Player)} nor {nameof(Mob)} component from {_character.name}!");
            }
        }

        private void OnDestroy()
        {
            if (_player != null) _player.OnHPChange -= HpChangeHandler;
            if (_mob != null) _mob.OnHpChange -= HpChangeHandler;
        }

        private void HpChangeHandler(object sender, (float, float) e)
        {
            var (_, difference) = e;

            var positive = difference >= 0;
            _text.text = $"{(positive ? "+" : "")}{difference}";
            _text.color = positive ? _addColor : _subtractColor;
            _animator.SetTrigger(AnimatorHashActive);
        }
    }
}