using UnityEngine;

namespace UI.InGame {
    public class UICanvasBuilding : MonoBehaviour {
        private void Update() {
            var bananaManPosition = ObjectsReference.Instance.bananaMan.transform.position;
            transform.LookAt(new Vector3(bananaManPosition.x, transform.position.y, bananaManPosition.z));
        }
    }
}