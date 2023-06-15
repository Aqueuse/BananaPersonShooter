using UnityEditor;
using UnityEngine;

namespace Tweaks {
    public class PhysicsEditorMode : EditorWindow {
        private void OnGUI() {
            if (GUILayout.Button("Start")) {
                EnablePhysic();
            }
            if (GUILayout.Button("Stop")) {
                DisablePhysic();
            }
        }
    
        private void EnablePhysic() {
            #if UNITY_EDITOR
                Physics.autoSimulation = false;
                EditorApplication.update += OnEditorUpdate;
            #endif
        }

        private void DisablePhysic() {
            #if UNITY_EDITOR
                EditorApplication.update -= OnEditorUpdate;
                Physics.autoSimulation = true;
            #endif
        }

        protected virtual void OnEditorUpdate() {
            StepPhysics();
        }
    
        private void StepPhysics() {
            Physics.autoSimulation = false;
            Physics.Simulate(Time.fixedDeltaTime);
            Physics.autoSimulation = true;
        }
    
        [MenuItem("Tools/Scene Physics")]
        private static void OpenWindow() {
            GetWindow<PhysicsEditorMode>(false, "Physics", true);
        }
    }
}