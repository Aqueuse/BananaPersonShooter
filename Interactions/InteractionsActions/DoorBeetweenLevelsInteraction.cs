using UnityEngine;

namespace Interactions.InteractionsActions {
    public class DoorBeetweenLevelsInteraction : Interact {
        public override void Activate(GameObject interactedObject) {
            ObjectsReference.Instance.audioManager.PlayEffect(SoundEffectType.OPEN_DOOR, 0);

            if (interactedObject != null) 
                ObjectsReference.Instance.scenesSwitch.SwitchScene(
                    interactedObject.GetComponent<Door>().destinationMap.ToUpper(), 
                    spawnPoint: interactedObject.GetComponent<Door>().spawnPoint, 
                    false, 
                    false
                );
        }
    }
}