using UnityEditor;
using UnityEngine;

public class SimulatePhysicsEditor : EditorWindow {
    private bool isSimulatingPhysic;
    
    private void OnGUI() {
        isSimulatingPhysic = GUILayout.Toggle(isSimulatingPhysic, "simulate physics");
    }

    void Update() {
        if (isSimulatingPhysic) StepPhysics();
    }

    private void StepPhysics() {
        Physics.autoSimulation = false;
        Physics.Simulate(Time.fixedDeltaTime);
        Physics.autoSimulation = true;
    }

    [MenuItem("Tools/Scene Physics")]
    private static void OpenWindow() {
        GetWindow<SimulatePhysicsEditor>(false, "Physics", true);
    }
}