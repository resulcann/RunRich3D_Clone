using UnityEngine;
using UnityEditor;

#if UNITY_EDITOR
[CustomEditor(typeof(Level))]
public class LevelInspector : Editor
{
    public override void OnInspectorGUI()
    {
        Level level = (Level)target;
        EditorUtility.SetDirty(level);

        level.levelIndex = EditorGUILayout.IntField("Level Index", level.levelIndex);
        level.levelPrefab = EditorGUILayout.ObjectField("Level Prefab To Spawn", level.levelPrefab,typeof(GameObject), true) as GameObject;
        

        EditorGUILayout.BeginHorizontal("Box");
            if(GUILayout.Button("Create Level"))
            {
                level.CreateLevel();
            }

            if(GUILayout.Button("Destroy Level"))
            {
                level.DestroyLevel();
            }
        EditorGUILayout.EndHorizontal();

    }
}
#endif