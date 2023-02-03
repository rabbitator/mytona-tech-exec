using System.Collections.Generic;
using MyTonaTechExec.MobUnit;
using MyTonaTechExec.PlayerUnit;
using UnityEngine;

namespace MyTonaTechExec.Projectiles
{
    public class ExplosiveProjectile : Projectile
    {
        [Space, SerializeField]
        private float _explosionRadius = 5.0f;

        private readonly HashSet<Collider> _gatheredObjects = new HashSet<Collider>();
        private int _physicsFramesCount;

        private const int WaitPhysicsFramesCount = 5;

        protected override void OnTriggerEnter(Collider other)
        {
            if (!_destroyed)
            {
                IncreaseDamageRadius();
                _destroyed = true;
            }

            _gatheredObjects.Add(other);
        }

        private void FixedUpdate()
        {
            if (!_destroyed) return;

            _physicsFramesCount++;
            if (_physicsFramesCount == WaitPhysicsFramesCount)
            {
                PerformDamage();
            }
        }

        private void PerformDamage()
        {
            foreach (var gatheredObject in _gatheredObjects)
            {
                var player = gatheredObject.GetComponent<Player>();
                if (_damagePlayer && player != null)
                {
                    player.TakeDamage(Damage);
                    continue;
                }

                var mob = gatheredObject.GetComponent<Mob>();
                if (_damageMob && mob != null)
                {
                    mob.TakeDamage(Damage);
                }
            }

            ActivateImpactVisual();
        }

        private void IncreaseDamageRadius()
        {
            switch (_trigger.GetType())
            {
                case { } sphereCollider when sphereCollider == typeof(SphereCollider):
                    ((SphereCollider)_trigger).radius = _explosionRadius;
                    break;
                case { } boxCollider when boxCollider == typeof(SphereCollider):
                    ((BoxCollider)_trigger).size = 2.0f * Vector3.one * _explosionRadius;
                    break;
            }
        }
    }
}