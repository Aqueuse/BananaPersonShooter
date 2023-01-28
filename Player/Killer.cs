using Save;
using Settings;
using UnityEngine;

namespace Player {
    public class Killer : MonoBehaviour {
        private void OnTriggerEnter(Collider other) {
            if (other.tag.Equals("Player")) {
                GameManager.Instance.Death();
                BananaMan.Instance.gameObject.transform.position = GameSave.Instance.lastPositionOnMap;
            }
        }
    }
}
