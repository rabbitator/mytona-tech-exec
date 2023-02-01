using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class Mob : MonoBehaviour
{
    public float Damage = 1;
    public float MoveSpeed = 3.5f;
    public float Health = 3;
    public float MaxHealth = 3;

    private event EventHandler<(float, float)> _onHpChange;
    public event EventHandler<(float, float)> OnHpChange
    {
        add => _onHpChange += value;
        remove => _onHpChange -= value;
    }

    private CancellationTokenSource _cts = new CancellationTokenSource();

    private const float LayingDeadTime = 3.0f;
    private const float DivingTime = 3.0f;
    private const float DivingDepth = -2.0f;

    private void Awake()
    {
        _cts = new CancellationTokenSource();
    }

    private void OnDestroy()
    {
        _cts.Cancel();
        _cts = null;
    }

    public void TakeDamage(float amount)
    {
        if (Health <= 0) return;

        Health -= amount;
        _onHpChange?.Invoke(this, (Health, -amount));

        if (Health <= 0) Death();
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

        _onHpChange?.Invoke(this, (Health, amount));
    }

    private async void Death()
    {
        EventBus.Pub(EventBus.MOB_KILLED);
        var components = GetComponents<IMobComponent>();
        foreach (var component in components)
        {
            component.OnDeath();
        }

        GetComponent<Collider>().enabled = false;
        GetComponent<Rigidbody>().isKinematic = true;

        await Task.WhenAny(Task.Delay(TimeSpan.FromSeconds(LayingDeadTime), _cts.Token));
        await Task.WhenAny(DiveUnderground(_cts.Token));

        Destroy(gameObject);
    }

    private async Task DiveUnderground(CancellationToken ct)
    {
        var startTime = Time.time;
        var startPosition = transform.position;
        var targetPosition = transform.position + Vector3.up * DivingDepth;

        while (!ct.IsCancellationRequested && Time.time < startTime + DivingTime)
        {
            var normalizedTime = (Time.time - startTime) / DivingTime;
            transform.position = Vector3.Lerp(startPosition, targetPosition, normalizedTime);

            await Task.Yield();
        }
    }
}