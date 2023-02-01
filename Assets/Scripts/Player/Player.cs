using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Instance;
    public float Damage = 1;
    public float MoveSpeed = 3.5f;
    public float Health = 3;
    public float MaxHealth = 3;

    private event EventHandler<PlayerWeapon.WeaponType> _onWeaponChange;
    public event EventHandler<PlayerWeapon.WeaponType> OnWeaponChange
    {
        add => _onWeaponChange += value;
        remove => _onWeaponChange -= value;
    }

    private event EventHandler<(float, float)> _onHPChange;
    public event EventHandler<(float, float)> OnHPChange
    {
        add => _onHPChange += value;
        remove => _onHPChange -= value;
    }

    private void Awake()
    {
        if (Instance != null)
        {
            DestroyImmediate(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }

    public void TakeDamage(float amount)
    {
        if (Health <= 0)
            return;
        Health -= amount;
        if (Health <= 0)
        {
            EventBus.Pub(EventBus.PLAYER_DEATH);
        }

        _onHPChange?.Invoke(this, (Health, -amount));
    }

    public void Heal(float amount)
    {
        if (Health <= 0)
            return;
        Health += amount;
        if (Health > MaxHealth)
        {
            Health = MaxHealth;
        }

        _onHPChange?.Invoke(this, (Health, amount));
    }


    public void Upgrade(float hp, float dmg, float ms)
    {
        Damage += dmg;
        Health += hp;
        MaxHealth += hp;
        MoveSpeed += ms;
        _onHPChange?.Invoke(this, (Health, 0));
    }

    public void ChangeWeapon(PlayerWeapon.WeaponType type)
    {
        _onWeaponChange?.Invoke(this, type);
    }
}