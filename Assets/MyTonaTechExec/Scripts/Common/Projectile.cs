using MyTonaTechExec.PlayerUnit;
using UnityEngine;

namespace MyTonaTechExec.Common
{
    public sealed class Projectile : MonoBehaviour
    {
        public float Damage;
        public float Speed = 8;
        public bool DamagePlayer = false;
        public bool DamageMob;
        public float TimeToLive = 5f;
        private float timer = 0f;
        private bool destroyed = false;

        private void OnTriggerEnter(Collider other)
        {
            if (destroyed)
            {
                return;
            }

            if (DamagePlayer && other.CompareTag("Player"))
            {
                other.GetComponent<Player>().TakeDamage(Damage);
                destroyed = true;
            }

            if (DamageMob && other.CompareTag("Mob"))
            {
                var mob = other.GetComponent<MobUnit.Mob>();
                mob.TakeDamage(Damage);
                destroyed = true;
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
    }
}