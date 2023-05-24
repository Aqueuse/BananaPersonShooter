using UnityEngine;

namespace Building.Buildables.DoorLeft {
    public class CloseDoorLeft : MonoBehaviour {
        private Transform doorTransform;
        private RotationDoorLeft _rotationDoor;

        private float _rotationStep;
        
        private void Start() {
            doorTransform = transform;
            _rotationDoor = GetComponent<RotationDoorLeft>();
            _rotationStep = 0;
        }
        
        private void Update() {
            _rotationStep -= Time.deltaTime * _rotationDoor.rotationSpeed;
            doorTransform.RotateAround(_rotationDoor.pivot.position, Vector3.up, _rotationStep);
            
            if (Quaternion.Angle(_rotationDoor.pivot.localRotation, doorTransform.localRotation) >= 90) {
                _rotationDoor.doorState = DoorState.CLOSE;
                _rotationStep = 0;
                _rotationDoor.Idle();
            }
        }
    }
}
