﻿using System.Threading.Tasks;
using MyTonaTechExec.Common;
using MyTonaTechExec.EventBus;
using MyTonaTechExec.EventBus.Messages;
using MyTonaTechExec.PlayerUnit;
using UnityEngine;

namespace MyTonaTechExec.Weapon
{
	public class Rifle : PlayerWeapon
	{
		public override WeaponType Type => WeaponType.Rifle;
		public Projectile BulletPrefab;
		public float Reload = 1f;
		public Transform FirePoint;
		public ParticleSystem VFX;

		protected float lastTime;

		protected override void Awake()
		{
			base.Awake();
			EventBus<PlayerInputMessage>.Sub(Fire);
			lastTime = Time.time - Reload;
		}

		protected virtual float GetDamage()
		{
			return GetComponent<Player>().Damage;
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
			bullet.Damage = GetDamage();
			VFX.Play();
		}
	}
}