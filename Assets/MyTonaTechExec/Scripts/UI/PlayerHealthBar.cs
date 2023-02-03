using MyTonaTechExec.PlayerUnit;
using TMPro;
using UnityEngine;

namespace MyTonaTechExec.UI
{
    public class PlayerHealthBar : MonoBehaviour
    {
        public GameObject Bar;
        public SpriteRenderer BarImg;
        public TMP_Text Text;
        private float maxHP;
        private Player player;

        private void Awake()
        {
            player = GetComponent<Player>();
            player.OnHPChange += OnHPChange;

            OnHPChange(null, (player.Health, 0));
        }

        public void OnDeath()
        {
            Bar.SetActive(false);
        }

        private void LateUpdate()
        {
            Bar.transform.rotation = Camera.main.transform.rotation;
        }

        private void OnHPChange(object obj, (float health, float diff) args)
        {
            var (health, _) = args;
            var frac = health / player.MaxHealth;
            Text.text = $"{health:####}/{player.MaxHealth:####}";
            BarImg.size = new Vector2(frac, BarImg.size.y);
            var pos = BarImg.transform.localPosition;
            pos.x = -(1 - frac) / 2;
            BarImg.transform.localPosition = pos;
            if (health <= 0)
            {
                Bar.SetActive(false);
            }
        }
    }
}