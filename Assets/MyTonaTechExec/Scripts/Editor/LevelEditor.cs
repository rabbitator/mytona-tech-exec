using MyTonaTechExec.Data;
using UnityEditor;
using UnityEngine;

namespace MyTonaTechExec.Scripts.Editor
{
    [CustomEditor(typeof(LevelData))]
    public class LevelEditor : UnityEditor.Editor
    {
        private LevelData data;

        private void OnEnable()
        {
            data = (LevelData)target;
        }

        public override void OnInspectorGUI()
        {
            var width = EditorGUIUtility.currentViewWidth;
            var buttonWidth = (int)(width / LevelData.FieldSize - 5);

            GUILayout.BeginVertical();

            for (var i = 0; i < LevelData.FieldSize; i++)
            {
                GUILayout.BeginHorizontal();

                for (var j = 0; j < LevelData.FieldSize; j++)
                {
                    var index = LevelData.FieldSize * i + j;
                    data.Map[index] = CustomToggle(data.Map[index], buttonWidth, buttonWidth);
                }

                GUILayout.EndHorizontal();
            }

            GUILayout.EndVertical();

            GUI.backgroundColor = 0.8f * Color.white;

            GUILayout.Space(10);
            EditorGUILayout.BeginHorizontal();

            if (GUILayout.Button("Select All")) SelectAll(true);
            if (GUILayout.Button("Deselect All")) SelectAll(false);
            if (GUILayout.Button("Save")) Save();

            EditorGUILayout.EndHorizontal();

            GUILayout.Space(10);
            DrawDefaultInspector();
        }

        private void SelectAll(bool condition)
        {
            for (var i = 0; i < data.Map.Length; i++)
            {
                data.Map[i] = condition;
            }
        }

        private void Save()
        {
            EditorUtility.SetDirty(data);
        }

        private bool CustomToggle(bool myToggle, int height, int width)
        {
            GUI.backgroundColor = myToggle ? 0.4f * Color.white : Color.white;
            if (GUILayout.Button("", GUILayout.Height(height), GUILayout.Width(width)))
            {
                myToggle = !myToggle;
            }

            return myToggle;
        }
    }
}