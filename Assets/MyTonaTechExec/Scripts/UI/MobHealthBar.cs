using MyTonaTechExec.MobUnit;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace MyTonaTechExec.UI
{
    public class MobHealthBar : MonoBehaviour, IMobComponent
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

        private float maxHP;

        private void Awake()
        {
            var mob = GetComponent<Mob>();
            maxHP = mob.MaxHealth;
            mob.OnHpChange += OnHPChange;

            OnHPChange(null, (mob.Health, 0));
        }

        public void OnDeath()
        {
            _bar.SetActive(false);
        }

        private void OnHPChange(object _, (float health, float diff) args)
        {
            var (health, _) = args;
            var frac = health / maxHP;
            _text.text = $"{health:####}/{maxHP:####}";
            _barImg.size = new Vector2(frac, _barImg.size.y);
            var pos = _barImg.transform.localPosition;
            pos.x = -(1 - frac) / 2;
            _barImg.transform.localPosition = pos;
        }
    }
}