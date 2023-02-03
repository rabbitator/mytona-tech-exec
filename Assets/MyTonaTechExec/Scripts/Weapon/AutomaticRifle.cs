using System.Threading.Tasks;
using MyTonaTechExec.EventBus.Messages;
using MyTonaTechExec.PlayerUnit;
using MyTonaTechExec.Projectiles;
using UnityEngine;
using UnityEngine.Serialization;

namespace MyTonaTechExec.Weapon
{
    public class AutomaticRifle : PlayerWeapon
    {
        public override WeaponType Type => WeaponType.AutomaticRifle;
        
        [FormerlySerializedAs("BulletPrefab")]
        [SerializeField]
        private Projectile _bulletPrefab;
        [FormerlySerializedAs("Reload")]
        [SerializeField]
        private float _reload = 1f;
        [FormerlySerializedAs("FirePoint")]
        [SerializeField]
        private Transform _firePoint;
        [FormerlySerializedAs("VFX")]
        [SerializeField]
        private ParticleSystem _vfx;

        private float _lastTime;

        protected override void Awake()
        {
            base.Awake();
            _lastTime = Time.time - _reload;
        }

        protected virtual float GetDamage()
        {
            return GetComponent<Player>().Damage / 5f;
        }

        protected override async void Fire(PlayerInputMessage message)
        {
            if (Time.time - _reload < _lastTime)
            {
                return;
            }

            if (!message.Fire)
            {
                return;
            }

            _lastTime = Time.time;
            GetComponent<PlayerAnimator>().TriggerShoot();

            await Task.Delay(16);

            var bullet = Instantiate(_bulletPrefab, _firePoint.position, transform.rotation);
            bullet.Damage = GetDamage();
            _vfx.Play();
        }
    }
}