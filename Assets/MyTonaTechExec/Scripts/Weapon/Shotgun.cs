using System.Threading.Tasks;
using MyTonaTechExec.EventBus.Messages;
using MyTonaTechExec.PlayerUnit;
using MyTonaTechExec.Projectiles;
using UnityEngine;
using UnityEngine.Serialization;

namespace MyTonaTechExec.Weapon
{
    public class Shotgun : PlayerWeapon
    {
        public override WeaponType Type => WeaponType.Shotgun;

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
            return GetComponent<Player>().Damage;
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
            var directions = SpreadDirections(transform.rotation.eulerAngles, 3, 20);
            foreach (var direction in directions)
            {
                var bullet = Instantiate(_bulletPrefab, _firePoint.position, Quaternion.Euler(direction));
                bullet.Damage = GetDamage();
            }

            _vfx.Play();
        }

        private Vector3[] SpreadDirections(Vector3 direction, int num, int spreadAngle)
        {
            Vector3[] result = new Vector3[num];
            result[0] = new Vector3(0, direction.y - (num - 1) * spreadAngle / 2, 0);
            for (int i = 1; i < num; i++)
            {
                result[i] = result[i - 1] + new Vector3(0, spreadAngle, 0);
            }

            return result;
        }
    }
}