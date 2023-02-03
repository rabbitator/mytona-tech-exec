using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace MyTonaTechExec.Data
{
    [CreateAssetMenu(menuName = "Data/LevelData")]
    public class LevelData : ScriptableObject
    {
        public const int FieldSize = 12;

        [FormerlySerializedAs("Index")]
        [SerializeField]
        private int _index;
        [FormerlySerializedAs("Map")]
        [HideInInspector]
        [SerializeField]
        private bool[] _map = new bool[FieldSize * FieldSize];
        [FormerlySerializedAs("WaveDatas")]
        [SerializeField]
        private List<WaveData> _waveDatas;
        [FormerlySerializedAs("WaveInterval")]
        [SerializeField]
        private float _waveInterval = 5f;

        public int Index => _index;
        public bool[] Map => _map;
        public List<WaveData> WaveDatas => _waveDatas;
        public float WaveInterval => _waveInterval;
    }
}