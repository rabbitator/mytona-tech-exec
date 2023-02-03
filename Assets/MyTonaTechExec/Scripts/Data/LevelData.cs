using System.Collections.Generic;
using UnityEngine;

namespace MyTonaTechExec.Data
{
    [CreateAssetMenu(menuName = "Data/LevelData")]
    public class LevelData : ScriptableObject
    {
        public int Index;
        public bool[] Map = new bool[FieldSize * FieldSize];
        public List<WaveData> WaveDatas;
        public float WaveInterval = 5f;
        
        public const int FieldSize = 12;
    }
}