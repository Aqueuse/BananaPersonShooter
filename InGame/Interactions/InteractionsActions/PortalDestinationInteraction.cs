using UnityEngine;

namespace InGame.Interactions.InteractionsActions {
    public class PortalDestinationInteraction : Interaction {
        public RegionType regionName;
        public Vector3 destinationPosition;
        public Quaternion destinationRotation;
        public string portalUuid;
    
        public override void Activate(GameObject interactedGameObject) {
            // tp banana man
        }
    }
}
