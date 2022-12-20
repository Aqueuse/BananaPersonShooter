using System.Globalization;
using UnityEditor;
using UnityEngine;

namespace Editor {
    public class SlowMotion : EditorWindow {
        private float _myTimeScale = 1f;
        private bool _setTimebuttonActivated;
        private bool _resetTimebuttonActivated;
        
        [MenuItem("Window/SlowMotion")]
        public static void ShowWindow() {
            //Show existing window instance. If one doesn't exist, make one.
            GetWindow(typeof(SlowMotion));
        }
        
        private void OnGUI() {
            GUILayout.Label ("TimeScale", EditorStyles.boldLabel);
            GUILayout.Label (Time.timeScale.ToString(CultureInfo.InvariantCulture));
            
            GUILayout.Label ("Edit TimeScale", EditorStyles.boldLabel);
            _myTimeScale = EditorGUILayout.FloatField("1f", _myTimeScale);
            _setTimebuttonActivated = GUILayout.Button("set");
            _resetTimebuttonActivated = GUILayout.Button("reset");

            if (_setTimebuttonActivated) Time.timeScale = _myTimeScale;
            else {
                if (_resetTimebuttonActivated) Time.timeScale = 1f;
            }
        }
    }
}
