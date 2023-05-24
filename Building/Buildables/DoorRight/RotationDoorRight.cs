using UnityEngine;

namespace Building.Buildables.DoorRight {
    public class RotationDoorRight : MonoBehaviour {
        public Transform pivot;
        public DoorState doorState = DoorState.CLOSE;
        public float rotationSpeed = 2;
        
        [SerializeField] private OpenDoorRight _openDoorRight;
        [SerializeField] private CloseDoorRight closeDoorRight;
        
        public void Action() {
            if (doorState == DoorState.OPEN) {
                _openDoorRight.enabled = false;
                closeDoorRight.enabled = true;
            }
            else {
                _openDoorRight.enabled = true;
                closeDoorRight.enabled = false;
            }
        }

        public void Idle() {
            _openDoorRight.enabled = false;
            closeDoorRight.enabled = false;
        }
    }
}
