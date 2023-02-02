using MyTonaTechExec.MobUnit;
using MyTonaTechExec.PlayerUnit;
using UnityEngine;

namespace MyTonaTechExec.Projectiles
{
    [RequireComponent(typeof(Collider))]
    public class Projectile : MonoBehaviour
    {
        public float Damage;
        public float Speed = 8;
        public bool DamagePlayer;
        public bool DamageMob;
        public float TimeToLive = 5f;
        public GameObject _trailVFX;
        public GameObject _impactVFX;

        protected Collider _trigger;
        protected bool destroyed;
        private MeshRenderer _meshRenderer;
        private float timer;
        private bool _radiusIncreased;

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
            if (destroyed)
            {
                return;
            }

            var damagedPlayer = DamagePlayer && other.CompareTag("Player");
            var damagedMob = DamageMob && other.CompareTag("Mob");

            if (damagedPlayer)
            {
                other.GetComponent<Player>().TakeDamage(Damage);
                destroyed = true;
            }

            if (damagedMob)
            {
                other.GetComponent<Mob>().TakeDamage(Damage);
                destroyed = true;
            }

            if (destroyed)
            {
                ActivateImpactVisual();
            }
        }

        private void Update()
        {
            if (!destroyed)
            {
                transform.position += transform.forward * (Speed * Time.deltaTime);
            }

            timer += Time.deltaTime;
            if (timer > TimeToLive)
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