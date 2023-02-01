using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/LevelData")]
public class LevelData : ScriptableObject
{
    public const int FieldSize = 12;

    public int Index;

    public bool[,] GetMap()
    {
        var map = new bool[12, 12];
        var lines = CharMap.Split('\n', '\r');
        for (var i = 0; i < 12; i++)
        {
            for (var j = 0; j < 12; j++)
            {
                map[i, j] = lines[i][j] == '1';
            }
        }

        return map;
    }

    [TextArea(16, 16)]
    public string CharMap;

    public List<WaveData> WaveDatas;
    public float WaveInterval = 5f;
}