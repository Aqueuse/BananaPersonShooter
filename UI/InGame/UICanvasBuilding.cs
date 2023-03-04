using Player;
using UnityEngine;

namespace UI.InGame {
    public class UICanvasBuilding : MonoBehaviour {
        private void Update() {
            var bananaManPosition = BananaMan.Instance.transform.position;
            transform.LookAt(new Vector3(bananaManPosition.x, transform.position.y, bananaManPosition.z));
        }
    }
}