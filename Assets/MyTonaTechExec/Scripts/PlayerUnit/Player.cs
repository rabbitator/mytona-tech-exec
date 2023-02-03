using System;
using MyTonaTechExec.Weapon;
using UnityEngine;
using UnityEngine.Serialization;

namespace MyTonaTechExec.PlayerUnit
{
    public class Player : MonoBehaviour
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

        public static Player Instance;

        public float Damage => _damage;
        public float MoveSpeed => _moveSpeed;
        public float Health => _health;
        public float MaxHealth => _maxHealth;

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
            if (_health <= 0)
                return;
            _health -= amount;
            if (_health <= 0)
            {
                EventBus.EventBus.Pub(EventBus.EventBus.PLAYER_DEATH);
            }

            _onHPChange?.Invoke(this, (_health, -amount));
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

            _onHPChange?.Invoke(this, (_health, amount));
        }


        public void Upgrade(float hp, float dmg, float ms)
        {
            _damage += dmg;
            _health += hp;
            _maxHealth += hp;
            _moveSpeed += ms;
            _onHPChange?.Invoke(this, (_health, 0));
        }

        public void ChangeWeapon(PlayerWeapon.WeaponType type)
        {
            _onWeaponChange?.Invoke(this, type);
        }
    }
}