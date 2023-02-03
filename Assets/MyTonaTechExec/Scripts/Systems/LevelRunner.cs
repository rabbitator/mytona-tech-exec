using System;
using System.Collections;
using System.Collections.Generic;
using MyTonaTechExec.Data;
using MyTonaTechExec.EventBus;
using MyTonaTechExec.EventBus.Messages;
using UnityEngine;
using UnityEngine.Serialization;

namespace MyTonaTechExec.Systems
{
    public class LevelRunner : MonoBehaviour
    {
        [FormerlySerializedAs("MaxMobCount")]
        [SerializeField]
        private int _maxMobCount = 5;
        [FormerlySerializedAs("SpawnInterval")]
        [SerializeField]
        private float _spawnInterval = 2f;

        private List<LevelData> _levelDatas = null;
        private int _mobsCount = 0;

        private void Awake()
        {
            _levelDatas = new List<LevelData>(Resources.LoadAll<LevelData>("Data"));
            EventBus.EventBus.Sub(MobKilled, EventBus.EventBus.MOB_KILLED);
            EventBus<SpawnMobMessage>.Sub(MobSpawned);
            EventBus<LoadLevelMessage>.Sub(LoadLevelMessage);
        }

        private void Start()
        {
            EventBus<LoadLevelMessage>.Pub(new LoadLevelMessage(0));
        }

        private void OnDestroy()
        {
            EventBus.EventBus.Unsub(MobKilled, EventBus.EventBus.MOB_KILLED);
            EventBus<SpawnMobMessage>.Unsub(MobSpawned);
            EventBus<LoadLevelMessage>.Unsub(LoadLevelMessage);
        }

        private void LoadLevel(int index)
        {
            var level = _levelDatas.Find(l => l.Index == index);
            if (level == null)
            {
                EventBus.EventBus.Pub(EventBus.EventBus.PLAYER_WON);
                return;
            }

            StartCoroutine(Waves(level.WaveDatas, level.WaveInterval, level.Index));
            EventBus<FieldCreateMessage>.Pub(new FieldCreateMessage()
            {
                Field = GetMap(level.Map)
            });
        }

        private void MobKilled()
        {
            _mobsCount--;
        }

        private void MobSpawned(SpawnMobMessage message)
        {
            _mobsCount++;
        }

        private void LoadLevelMessage(LoadLevelMessage message)
        {
            LoadLevel(message.LevelIndex);
        }

        private bool[,] GetMap(bool[] array)
        {
            var map = new bool[LevelData.FieldSize, LevelData.FieldSize];

            for (var i = 0; i < LevelData.FieldSize; i++)
            {
                for (var j = 0; j < LevelData.FieldSize; j++)
                {
                    var index = LevelData.FieldSize * i + j;
                    map[i, j] = array[index];
                }
            }

            return map;
        }

        private IEnumerator Waves(List<WaveData> waveDatas, float interval, int level)
        {
            foreach (var waveData in waveDatas)
            {
                foreach (var keyValue in waveData.WaveMobNCount)
                {
                    for (var i = 0; i < keyValue.y; i++)
                    {
                        while (_mobsCount >= _maxMobCount)
                        {
                            yield return new WaitForSeconds(_spawnInterval);
                        }

                        EventBus<SpawnMobMessage>.Pub(new SpawnMobMessage()
                        {
                            Type = keyValue.x
                        });

                        yield return new WaitForSeconds(_spawnInterval);
                    }
                }

                yield return new WaitForSeconds(interval);
            }

            yield return new WaitForSeconds(interval);

            EventBus<LoadLevelMessage>.Pub(new LoadLevelMessage(level + 1));
        }
    }
}