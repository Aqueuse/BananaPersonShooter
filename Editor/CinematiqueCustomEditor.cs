using Cinematique;
using UnityEditor;
using UnityEngine;
using VFX;

namespace Editor {
    public class CinematiqueCustomEditor : EditorWindow {
        private bool isTeleportUp;
        private bool isTeleportDown;
        private bool fire1;
        private bool fire2;
        private bool fire3;
        
        [MenuItem("Window/Cinematique")]

        public static void ShowWindow() {
            //Show existing window instance. If one doesn't exist, make one.
            GetWindow(typeof(CinematiqueCustomEditor));
        }
        
        private void OnGUI() {
            GUILayout.Label ("Edit TimeScale", EditorStyles.boldLabel);
            isTeleportUp = GUILayout.Button("teleport up");
            isTeleportDown = GUILayout.Button("teleport down");

            fire1 = GUILayout.Button("fire1");
            fire3 = GUILayout.Button("fire2");
            fire2 = GUILayout.Button("fire3");

            if (fire1) {
                GameObject.FindWithTag("laser1").GetComponent<LaserShoot>().Fire();
            }
            
            if (fire2) {
                GameObject.FindWithTag("laser2").GetComponent<LaserShoot>().Fire();
            }
            
            if (fire3) {
                GameObject.FindWithTag("laser3").GetComponent<LaserShoot>().Fire();
            }
            
            if (isTeleportUp) {
                GameObject.FindWithTag("teleportation").GetComponent<Teleportation>().TeleportUp();
            }

            else {
                if (isTeleportDown) {
                    GameObject.FindWithTag("teleportation").GetComponent<Teleportation>().TeleportDown();
                }
            }
            
            
        }
    }
}