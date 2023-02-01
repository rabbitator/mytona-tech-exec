using System;
using System.Collections.Generic;
using UnityEngine;

public class Mob : MonoBehaviour
{
    public float Damage = 1;
    public float MoveSpeed = 3.5f;
    public float Health = 3;
    public float MaxHealth = 3;

    private event EventHandler<(float, float)> _onHPChange;
    public event EventHandler<(float, float)> OnHPChange
    {
        add => _onHPChange += value;
        remove => _onHPChange -= value;
    }

    public void TakeDamage(float amount)
    {
        if (Health <= 0)
            return;
        Health -= amount;
        _onHPChange?.Invoke(this, (Health, -amount));
        if (Health <= 0)
        {
            Death();
        }
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

    public void Death()
    {
        EventBus.Pub(EventBus.MOB_KILLED);
        var components = GetComponents<IMobComponent>();
        foreach (var component in components)
        {
            component.OnDeath();
        }

        GetComponent<Collider>().enabled = false;
        GetComponent<Rigidbody>().isKinematic = true;
    }
}