using TMPro;
using UnityEngine;

public class HealthBar : MonoBehaviour, IMobComponent
{
    public GameObject Bar;
    public SpriteRenderer BarImg;
    public TMP_Text Text;
    private float maxHP;

    private void Awake()
    {
        var mob = GetComponent<Mob>();
        maxHP = mob.MaxHealth;
        mob.OnHPChange += OnHPChange;
    }

    public void OnDeath()
    {
        Bar.SetActive(false);
    }

    private void LateUpdate()
    {
        Bar.transform.rotation = Camera.main.transform.rotation;
    }

    private void OnHPChange(object _, (float health, float diff) args)
    {
        var (health, _) = args;
        var frac = health / maxHP;
        Text.text = $"{health:####}/{maxHP:####}";
        BarImg.size = new Vector2(frac, BarImg.size.y);
        var pos = BarImg.transform.localPosition;
        pos.x = -(1 - frac) / 2;
        BarImg.transform.localPosition = pos;
    }
}