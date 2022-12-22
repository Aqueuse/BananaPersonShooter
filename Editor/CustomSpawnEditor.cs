 using System;
 using JetBrains.Annotations;
 using PrefabSpawner;
 using UnityEngine;
 using UnityEditor;
 
[CustomEditor(typeof(Spawner))]
public class customSpawnInspector: UnityEditor.Editor {
	public override void OnInspectorGUI() {
		DrawDefaultInspector();
		
		Spawner spawner = (Spawner)target;

		if(GUILayout.Button("Spawn Prefabs")) {
			spawner.SpawnThemAll();
		}
		
		if (GUILayout.Button("Remove Prefabs")) {
			spawner.RemoveThemAll();
		}
	}
}