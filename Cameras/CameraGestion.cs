using Gestion;
using UnityEngine;

namespace Cameras {
    public class CameraGestion : MonoBehaviour {
        [SerializeField] private Transform cameraGestionYaxisTransform;
        
        public float cameraSpeed;
        public float cameraYposition = 250;
        
        private Transform _transform;
        public Vector3 cameraMovement;

        private Vector3 cameraPosition;

        private Collider cameraBounds;
        
        private void OnEnable() {
            _transform = transform;
            ResetPosition();
            
            cameraSpeed = ObjectsReference.Instance.gameSettings.lookSensibility*3;
            cameraBounds = MapItems.Instance.cameraBounds;
        }

        private void Update() {
            _transform.Translate(cameraMovement * cameraSpeed, _transform);
            
            if (!cameraBounds.bounds.Contains(cameraGestionYaxisTransform.position))
                _transform.Translate(-cameraMovement * cameraSpeed, _transform);
        }

        public void Rotate(Vector2 rotation) {
            _transform.Rotate(0, rotation.x*cameraSpeed, 0);
            cameraGestionYaxisTransform.Rotate(-rotation.y*cameraSpeed, 0, 0);
        } 
        
        public void Move(float Xmovement, float Zmovement) {
            cameraMovement.x = Xmovement * cameraSpeed;
            cameraMovement.y = 0;
            cameraMovement.z = Zmovement * cameraSpeed;
        }

        public void MoveUpDown(float Ymovement) {
            if (cameraYposition is < 300 and > 200) {
                cameraMovement.y = Ymovement*2;
            }
            else {
                cameraMovement.y = 0;
            }
        }
        
        public void CancelMove() {
            cameraMovement = Vector3.zero;
        }

        public void CancelMoveUpDown() {
            cameraMovement.y = 0;
        }

        public void ResetPosition() {
            var cameraTransform = ObjectsReference.Instance.bananaMan.transform.position;
            cameraTransform.y = cameraYposition;
            transform.position = cameraTransform;
        }
    }
}
