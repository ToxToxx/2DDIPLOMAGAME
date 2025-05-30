namespace SpawnLogic
{
    // Assets/Editor/SpawnManagerEditor.cs
    using UnityEditor;
    using UnityEngine;

    [CustomEditor(typeof(JsonSpawnManager))]
    public class SpawnManagerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            var mgr = (JsonSpawnManager)target;
            if (GUILayout.Button("Sync Pools with JSON"))
            {
                mgr.SyncWithJson();
                EditorUtility.SetDirty(mgr);
            }
        }
    }

}
