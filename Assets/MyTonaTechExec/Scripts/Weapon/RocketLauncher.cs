﻿using System.Threading.Tasks;
using MyTonaTechExec.EventBus.Messages;
using MyTonaTechExec.PlayerUnit;
using MyTonaTechExec.Projectiles;
using UnityEngine;

namespace MyTonaTechExec.Weapon
{
	public class RocketLauncher : PlayerWeapon
	{
		public override WeaponType Type => WeaponType.RocketLauncher;
		public Projectile BulletPrefab;
		public float Reload = 1f;
		public Transform FirePoint;
		public ParticleSystem VFX;

		protected float lastTime;

		protected override void Awake()
		{
			base.Awake();
			lastTime = Time.time - Reload;
		}

		protected override async void Fire(PlayerInputMessage message)
		{
			if (Time.time - Reload < lastTime)
			{
				return;
			}

			if (!message.Fire)
			{
				return;
			}

			lastTime = Time.time;
			GetComponent<PlayerAnimator>().TriggerShoot();

			await Task.Delay(16);

			var bullet = Instantiate(BulletPrefab, FirePoint.position, transform.rotation);
			VFX.Play();
		}
	}
}