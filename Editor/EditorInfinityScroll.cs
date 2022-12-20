using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEditor;


namespace InfinityScroll
{
    [CustomEditor(typeof(InfinityScroll))]
    public class Editor_InfinityScroll : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {

            var t = ((InfinityScroll)target);
            if (GUILayout.Button("Reload the items"))
            {
                t.ReloadItems();
            }

            DrawDefaultInspector();
        }
    }
}