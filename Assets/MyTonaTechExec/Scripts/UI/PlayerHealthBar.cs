using MyTonaTechExec.PlayerUnit;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace MyTonaTechExec.UI
{
    public class PlayerHealthBar : MonoBehaviour
    {
        [FormerlySerializedAs("Bar")]
        [SerializeField]
        private GameObject _bar;
        [FormerlySerializedAs("BarImg")]
        [SerializeField]
        private SpriteRenderer _barImg;
        [FormerlySerializedAs("Text")]
        [SerializeField]
        private TMP_Text _text;

        private float _maxHp;
        private Player _player;

        private void Awake()
        {
            _player = GetComponent<Player>();
            _player.OnHPChange += OnHPChange;

            OnHPChange(null, (_player.Health, 0));
        }

        public void OnDeath()
        {
            _bar.SetActive(false);
        }

        private void LateUpdate()
        {
            _bar.transform.rotation = Camera.main.transform.rotation;
        }

        private void OnHPChange(object obj, (float health, float diff) args)
        {
            var (health, _) = args;
            var frac = health / _player.MaxHealth;
            _text.text = $"{health:####}/{_player.MaxHealth:####}";
            _barImg.size = new Vector2(frac, _barImg.size.y);
            var pos = _barImg.transform.localPosition;
            pos.x = -(1 - frac) / 2;
            _barImg.transform.localPosition = pos;
            if (health <= 0)
            {
                _bar.SetActive(false);
            }
        }
    }
}