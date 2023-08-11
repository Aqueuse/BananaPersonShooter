using PrefabSpawner;
using UnityEditor;
using UnityEngine;

namespace Editor {
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
}