using InGame.Items.ItemsBehaviours;
using UnityEngine;

namespace InGame.MiniGames.SpaceTrafficControl {
    public class AspirationCone : MonoBehaviour {
        private void OnTriggerEnter(Collider other) {
            if (TryGetComponent(out SpaceshipDebrisBehaviour spaceshipDebrisBehaviour)) {
                spaceshipDebrisBehaviour.isAttracted = true;
            }
        }
        
        private void OnTriggerExit(Collider other) {
            if (TryGetComponent(out SpaceshipDebrisBehaviour spaceshipDebrisBehaviour)) {
                spaceshipDebrisBehaviour.isAttracted = false;
            }
        }
    }
}