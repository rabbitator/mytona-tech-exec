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

    protected override void HandleMessage(SpawnMobMessage message)
    {
        var position = GetRandomPosition();
        Instantiate(Prefabs[message.Type], position, Quaternion.identity);
    }

    private static Vector3 GetRandomPosition()
    {
        var randomX = LevelData.FieldSize * (Random.value - 0.5f);
        var randomZ = LevelData.FieldSize * (Random.value - 0.5f);

        return new Vector3(randomX, 1, randomZ);
    }
}