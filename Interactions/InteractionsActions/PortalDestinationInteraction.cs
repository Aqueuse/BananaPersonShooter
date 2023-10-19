using Enums;
using UnityEngine;

namespace Interactions.InteractionsActions {
    public class PortalDestinationInteraction : MonoBehaviour {
        public string sceneName;
        public Vector3 destinationPosition;
        public Quaternion destinationRotation;
        public string portalUuid;
    
        public void Activate() {
            ObjectsReference.Instance.scenesSwitch.teleportDestination = destinationPosition;
            ObjectsReference.Instance.scenesSwitch.teleportRotation = destinationRotation;
            ObjectsReference.Instance.scenesSwitch.SwitchScene(sceneName, SpawnPoint.TELEPORTATION, true, false);
        }
    }
}
