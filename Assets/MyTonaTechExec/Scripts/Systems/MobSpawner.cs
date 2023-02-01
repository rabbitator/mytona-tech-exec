using MyTonaTechExec.Data;
using MyTonaTechExec.EventBus;
using MyTonaTechExec.EventBus.Messages;
using MyTonaTechExec.MobUnit;
using UnityEngine;
using Random = UnityEngine.Random;

namespace MyTonaTechExec.Systems
{
    public class MobSpawner : Handler<SpawnMobMessage>
    {
        public Mob[] Prefabs;

        protected override void Awake()
        {
            base.Awake();
            EventBus.EventBus.Sub(() => EventBus<SpawnMobMessage>.Unsub(HandleMessage), EventBus.EventBus.PLAYER_DEATH);
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
}