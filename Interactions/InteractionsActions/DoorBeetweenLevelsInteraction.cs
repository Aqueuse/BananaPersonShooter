using Items;
using UnityEngine;

namespace Interactions.InteractionsActions {
    public class DoorBeetweenLevelsInteraction : MonoBehaviour {
        public static void Activate(GameObject interactedObject) {
            ObjectsReference.Instance.audioManager.PlayEffect(EffectType.OPEN_DOOR, 0);

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
