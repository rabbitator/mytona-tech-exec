using System.Threading.Tasks;
using MyTonaTechExec.EventBus.Messages;
using MyTonaTechExec.PlayerUnit;
using MyTonaTechExec.Projectiles;
using UnityEngine;
using UnityEngine.Serialization;

namespace MyTonaTechExec.Weapon
{
    public class RocketLauncher : PlayerWeapon
    {
        public override WeaponType Type => WeaponType.RocketLauncher;

        [FormerlySerializedAs("BulletPrefab")]
        [SerializeField]
        public Projectile _bulletPrefab;
        [FormerlySerializedAs("Reload")]
        [SerializeField]
        public float _reload = 1f;
        [FormerlySerializedAs("FirePoint")]
        [SerializeField]
        public Transform _firePoint;
        [FormerlySerializedAs("VFX")]
        [SerializeField]
        public ParticleSystem _vfx;

        private float _lastTime;

        protected override void Awake()
        {
            base.Awake();
            _lastTime = Time.time - _reload;
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

            Instantiate(_bulletPrefab, _firePoint.position, transform.rotation);
            _vfx.Play();
        }
    }
}