using MyTonaTechExec.MobUnit;
using MyTonaTechExec.PlayerUnit;
using UnityEngine;
using UnityEngine.Serialization;

namespace MyTonaTechExec.Projectiles
{
    [RequireComponent(typeof(Collider))]
    public class Projectile : MonoBehaviour
    {
        [FormerlySerializedAs("Damage")]
        [SerializeField]
        protected float _damage;
        [FormerlySerializedAs("Speed")]
        [SerializeField]
        protected float _speed = 8;
        [FormerlySerializedAs("DamagePlayer")]
        [SerializeField]
        protected bool _damagePlayer;
        [FormerlySerializedAs("DamageMob")]
        [SerializeField]
        protected bool _damageMob;
        [FormerlySerializedAs("TimeToLive")]
        [SerializeField]
        protected float _timeToLive = 5f;
        [SerializeField]
        private GameObject _trailVFX;
        [SerializeField]
        private GameObject _impactVFX;

        protected Collider _trigger;
        protected bool _destroyed;
        private MeshRenderer _meshRenderer;
        private float _timer;
        private bool _radiusIncreased;

        public float Damage
        {
            get => _damage;
            set => _damage = value;
        }

        private void Awake()
        {
            _meshRenderer = GetComponentInChildren<MeshRenderer>();
            _trigger = GetComponent<Collider>();

            if (_trailVFX != null)
            {
                _trailVFX.SetActive(true);
            }

            if (_impactVFX != null)
            {
                _impactVFX.SetActive(false);
            }
        }

        protected virtual void OnTriggerEnter(Collider other)
        {
            if (_destroyed)
            {
                return;
            }

            var damagedPlayer = _damagePlayer && other.CompareTag("Player");
            var damagedMob = _damageMob && other.CompareTag("Mob");

            if (damagedPlayer)
            {
                other.GetComponent<Player>().TakeDamage(_damage);
                _destroyed = true;
            }

            if (damagedMob)
            {
                other.GetComponent<Mob>().TakeDamage(_damage);
                _destroyed = true;
            }

            if (_destroyed)
            {
                ActivateImpactVisual();
            }
        }

        private void Update()
        {
            if (!_destroyed)
            {
                transform.position += transform.forward * (_speed * Time.deltaTime);
            }

            _timer += Time.deltaTime;
            if (_timer > _timeToLive)
            {
                Destroy(gameObject);
            }
        }

        protected void ActivateImpactVisual()
        {
            if (_trailVFX != null)
            {
                _trailVFX.SetActive(false);
            }

            if (_impactVFX != null)
            {
                _impactVFX.SetActive(true);
            }

            if (_meshRenderer != null)
            {
                _meshRenderer.enabled = false;
            }
        }
    }
}