using MyTonaTechExec.Data;
using UnityEditor;
using UnityEngine;

namespace MyTonaTechExec.Scripts.Editor
{
    [CustomEditor(typeof(LevelData))]
    public class LevelEditor : UnityEditor.Editor
    {
        private int rows;
        private int columns;
        private bool[] selectedButtons;
        private Rect[] buttonRects;
        private LevelData data;

        private void OnEnable()
        {
            data = (LevelData)target;
            rows = columns = LevelData.FieldSize;
            selectedButtons = data.Map;
        }

        public override void OnInspectorGUI()
        {
            var width = EditorGUIUtility.currentViewWidth;
            var buttonWidth = (int)(width / columns - 5);

            GUILayout.BeginVertical();

            for (var i = 0; i < columns; i++)
            {
                GUILayout.BeginHorizontal();

                for (var j = 0; j < rows; j++)
                {
                    var index = LevelData.FieldSize * i + j;
                    selectedButtons[index] = CustomToggle(selectedButtons[index], buttonWidth, buttonWidth);
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
            for (var i = 0; i < selectedButtons.Length; i++)
            {
                selectedButtons[i] = condition;
            }
        }

        private void Save()
        {
            data.Map = selectedButtons;
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