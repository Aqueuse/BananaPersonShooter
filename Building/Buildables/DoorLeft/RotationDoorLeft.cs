using UnityEngine;

namespace Building.Buildables.DoorLeft {
    public class RotationDoorLeft : MonoBehaviour {
        public Transform pivot;
        public DoorState doorState = DoorState.CLOSE;
        public float rotationSpeed = 2;
        
        [SerializeField] private OpenDoorLeft openDoorLeft;
        [SerializeField] private CloseDoorLeft closeDoorLeft;
        
        public void Action() {
            if (doorState == DoorState.OPEN) {
                openDoorLeft.enabled = false;
                closeDoorLeft.enabled = true;
            }
            else {
                openDoorLeft.enabled = true;
                closeDoorLeft.enabled = false;
            }
        }

        public void Idle() {
            openDoorLeft.enabled = false;
            closeDoorLeft.enabled = false;
        }
    }
}
