using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class MobSpawner : Handler<SpawnMobMessage>
{
	public Mob[] Prefabs;

	protected override void Awake()
	{
		base.Awake();
		EventBus.Sub(() => EventBus<SpawnMobMessage>.Unsub(HandleMessage), EventBus.PLAYER_DEATH);
	}

	public override void HandleMessage(SpawnMobMessage message)
	{
		var position = new Vector3(Random.value * 11 - 6,1,Random.value * 11 - 6);
		Instantiate(Prefabs[message.Type], position, Quaternion.identity);
	}
}