using UnityEngine;

namespace InGame.MiniGames.SpaceTrafficControl {
    public class SpiraleCannonAspiration : MonoBehaviour {
        private void Update() {
            transform.Rotate(Vector3.down, 10);
        }
    }
}