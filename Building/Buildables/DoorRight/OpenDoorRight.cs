using UnityEngine;

namespace Building.Buildables.DoorRight {
    public class OpenDoorRight : MonoBehaviour {
        private Transform doorTransform;
        private RotationDoorRight _rotationDoor;
        
        private float _rotationStep;
        
        private void Start() {
            doorTransform = transform;
            _rotationDoor = GetComponent<RotationDoorRight>();
            _rotationStep = 0;
        }

        private void Update() {
            _rotationStep += Time.deltaTime * _rotationDoor.rotationSpeed;
            doorTransform.RotateAround(_rotationDoor.pivot.position, Vector3.up, _rotationStep);
            
            if (Quaternion.Angle(_rotationDoor.pivot.localRotation, doorTransform.localRotation) >= 90) {
                _rotationDoor.doorState = DoorState.OPEN;
                _rotationStep = 0;
                _rotationDoor.Idle();
            }
        }
    }
}
