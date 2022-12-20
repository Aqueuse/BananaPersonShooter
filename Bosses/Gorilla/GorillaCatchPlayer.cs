using Player;
using UnityEngine;

namespace Bosses.Gorilla {
    public class GorillaCatchPlayer : MonoBehaviour {
        private void OnTriggerEnter(Collider other) {
            if (other.CompareTag("Player")) {
                BananaMan.Instance.GetComponent<PlayerController>().PlayerRagdollAgainstCollider(GetComponent<SphereCollider>(), 100);
            }
        }
    }
}