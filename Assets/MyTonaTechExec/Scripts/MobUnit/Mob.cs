using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Serialization;

namespace MyTonaTechExec.MobUnit
{
    public class Mob : MonoBehaviour
    {
        [FormerlySerializedAs("Damage")]
        [SerializeField]
        private float _damage = 1;
        [FormerlySerializedAs("MoveSpeed")]
        [SerializeField]
        private float _moveSpeed = 3.5f;
        [FormerlySerializedAs("Health")]
        [SerializeField]
        private float _health = 3;
        [FormerlySerializedAs("MaxHealth")]
        [SerializeField]
        private float _maxHealth = 3;

        private const float LayingDeadTime = 3.0f;
        private const float DivingTime = 3.0f;
        private const float DivingDepth = -2.0f;

        private CancellationTokenSource _cts = new CancellationTokenSource();

        public float Damage => _damage;
        public float Health => _health;
        public float MaxHealth => _maxHealth;

        private event EventHandler<(float, float)> _onHpChange;
        public event EventHandler<(float, float)> OnHpChange
        {
            add => _onHpChange += value;
            remove => _onHpChange -= value;
        }

        private void Awake()
        {
            _cts = new CancellationTokenSource();
        }

        private void OnDestroy()
        {
            _cts.Cancel();
        }

        public void TakeDamage(float amount)
        {
            if (_health <= 0) return;

            _health -= amount;
            _onHpChange?.Invoke(this, (_health, -amount));

            if (_health <= 0) Death();
        }

        public void Heal(float amount)
        {
            if (_health <= 0)
                return;
            _health += amount;
            if (_health > _maxHealth)
            {
                _health = _maxHealth;
            }

            _onHpChange?.Invoke(this, (_health, amount));
        }

        private async void Death()
        {
            EventBus.EventBus.Pub(EventBus.EventBus.MOB_KILLED);
            var components = GetComponents<IMobComponent>();
            foreach (var component in components)
            {
                component.OnDeath();
            }

            GetComponent<Collider>().enabled = false;
            GetComponent<Rigidbody>().isKinematic = true;

            await Task.WhenAny(Task.Delay(TimeSpan.FromSeconds(LayingDeadTime), _cts.Token));
            await Task.WhenAny(DiveUnderground(_cts.Token));

            if (!_cts.Token.IsCancellationRequested) Destroy(gameObject);
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
}