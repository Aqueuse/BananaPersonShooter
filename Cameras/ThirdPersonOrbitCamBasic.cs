using System;
using Settings;
using UnityEngine;
using UnityEngine.InputSystem;

// This class corresponds to the 3rd person camera features.
namespace Cameras {
    public class ThirdPersonOrbitCamBasic : MonoBehaviour {
        public Transform player; // Player's reference.
        [SerializeField] private Vector3 pivotOffset; // Offset to repoint the camera.
    
        Vector2 _lookPosition;
    
        [SerializeField] private Vector3 camOffset; // Offset to relocate the camera related to the player position.

        [SerializeField] private float smooth ; // Speed of camera responsiveness.
        public float horizontalSensibility; // Horizontal turn speed.
        public float verticalSensibility; // Vertical turn speed.
        public float maxVerticalAngle = 30f; // Camera max clamp angle. 
        public float minVerticalAngle = -60f; // Camera min clamp angle.
        
        private float _angleH; // Float to store camera horizontal angle related to mouse movement.
        private float _angleV; // Float to store camera vertical angle related to mouse movement.
        private Transform _cam; // This transform.
        private Vector3 _smoothPivotOffset; // Camera current pivot offset on interpolation.
        private Vector3 _smoothCamOffset; // Camera current offset on interpolation.
        private Vector3 _targetPivotOffset; // Camera pivot offset target to iterpolate.
        private Vector3 _targetCamOffset; // Camera offset target to interpolate.
        private float _defaultFOV; // Default camera Field of View.
        private float _targetFOV; // Target camera Field of View.
        private float _targetMaxVerticalAngle; // Custom camera max vertical clamp angle.
        private bool _isCustomOffset; // Boolean to determine whether or not a custom camera offset is being used.

        public LayerMask layerMaskExcludeCollisionWithCamera;

        // Get the camera horizontal angle.
        public float GetH {
            get {
                return _angleH;
            }
        }

        private void Start() {
            // Reference to the camera transform.
            _cam = transform;

            // Vertical and Horizontal sensibility
            horizontalSensibility = GameSettings.Instance.horizontalCameraSensibility;
            verticalSensibility = GameSettings.Instance.verticalCameraSensibility;
            
            // Set camera default position.
            _cam.position = player.position + Quaternion.identity * pivotOffset + Quaternion.identity * camOffset;
            _cam.rotation = Quaternion.identity;

            // Set up references and default values.
            _smoothPivotOffset = pivotOffset;
            _smoothCamOffset = camOffset;
            _defaultFOV = _cam.GetComponent<Camera>().fieldOfView;
            _angleH = player.eulerAngles.y;

            ResetTargetOffsets();
            ResetFOV();
            ResetMaxVerticalAngle();

            // Check for no vertical offset.
            if (camOffset.y > 0)
                Debug.LogWarning("Vertical Cam Offset (Y) will be ignored during collisions!\n" +
                                 "It is recommended to set all vertical offset in Pivot Offset.");
        }

        private void OnEnable() {
            // Vertical and Horizontal sensibility
            horizontalSensibility = GameSettings.Instance.horizontalCameraSensibility;
            verticalSensibility = GameSettings.Instance.verticalCameraSensibility;
        }

        void Update() {
            // Get mouse movement to orbit the camera.
            _angleH += _lookPosition.x * horizontalSensibility;
            _angleV += _lookPosition.y * verticalSensibility;
            
            // Set vertical movement limit.
            _angleV = Mathf.Clamp(_angleV, minVerticalAngle, _targetMaxVerticalAngle);
            
            // Set camera orientation.
            Quaternion camYRotation = Quaternion.Euler(0, _angleH, 0);
            Quaternion aimRotation = Quaternion.Euler(-_angleV, _angleH, 0);
            _cam.rotation = aimRotation;

            // Set FOV.
            _cam.GetComponent<Camera>().fieldOfView =
                Mathf.Lerp(_cam.GetComponent<Camera>().fieldOfView, _targetFOV, Time.deltaTime);

            // Test for collision with the environment based on current camera position.
            Vector3 baseTempPosition = player.position + camYRotation * _targetPivotOffset;
            Vector3 noCollisionOffset = _targetCamOffset;
            while (noCollisionOffset.magnitude >= 0.2f) {
                if (DoubleViewingPosCheck(baseTempPosition + aimRotation * noCollisionOffset))
                    break;
                noCollisionOffset -= noCollisionOffset.normalized * 0.2f;
            }

            if (noCollisionOffset.magnitude < 0.2f)
                noCollisionOffset = Vector3.zero;

            // No intermediate position for custom offsets, go to 1st person.
            bool customOffsetCollision = _isCustomOffset && noCollisionOffset.sqrMagnitude < _targetCamOffset.sqrMagnitude;

            // Repostition the camera.
            _smoothPivotOffset = Vector3.Lerp(_smoothPivotOffset, customOffsetCollision ? pivotOffset : _targetPivotOffset,
                smooth * Time.deltaTime);
            _smoothCamOffset = Vector3.Lerp(_smoothCamOffset, customOffsetCollision ? Vector3.zero : noCollisionOffset,
                smooth * Time.deltaTime);

            _cam.position = player.position + camYRotation * _smoothPivotOffset + aimRotation * _smoothCamOffset;
        }

        public void Rotate(InputAction.CallbackContext context) {
            _lookPosition = context.action.ReadValue<Vector2>();
        }

        // Set camera offsets to custom values.
        public void SetTargetOffsets(Vector3 newPivotOffset, Vector3 newCamOffset) {
            _targetPivotOffset = newPivotOffset;
            _targetCamOffset = newCamOffset;
            _isCustomOffset = true;
        }

        // Reset camera offsets to default values.
        void ResetTargetOffsets() {
            _targetPivotOffset = pivotOffset;
            _targetCamOffset = camOffset;
            _isCustomOffset = false;
        }

        // Reset the camera vertical offset.
        public void ResetYCamOffset() {
            _targetCamOffset.y = camOffset.y;
        }

        // Set camera vertical offset.
        public void SetYCamOffset(float y) {
            _targetCamOffset.y = y;
        }

        // Set camera horizontal offset.
        public void SetXCamOffset(float x) {
            _targetCamOffset.x = x;
        }

        // Set custom Field of View.
        public void SetFOV(float customFOV) {
            this._targetFOV = customFOV;
        }

        // Reset Field of View to default value.
        void ResetFOV() {
            this._targetFOV = _defaultFOV;
        }

        // Set max vertical camera rotation angle.
        public void SetMaxVerticalAngle(float angle) {
            this._targetMaxVerticalAngle = angle;
        }

        // Reset max vertical camera rotation angle to default value.
        void ResetMaxVerticalAngle() {
            this._targetMaxVerticalAngle = maxVerticalAngle;
        }

        // Double check for collisions: concave objects doesn't detect hit from outside, so cast in both directions.
        bool DoubleViewingPosCheck(Vector3 checkPos) {
            return ViewingPosCheck(checkPos) && ReverseViewingPosCheck(checkPos);
        }

        // Check for collision from camera to player.
        bool ViewingPosCheck(Vector3 checkPos) {
            // Cast target and direction.
            Vector3 target = player.position + pivotOffset;
            Vector3 direction = target - checkPos;
            
            // If a raycast from the check position to the player hits something...
            if (Physics.SphereCast(checkPos, 0.2f, direction, out RaycastHit hit, direction.magnitude, layerMaskExcludeCollisionWithCamera)) {
                // ... if it is not the player...
                if (hit.transform != player && !hit.transform.GetComponent<Collider>().isTrigger) {
                    // This position isn't appropriate.
                    return false;
                }
            }

            // If we haven't hit anything or we've hit the player, this is an appropriate position.
            return true;
        }

        // Check for collision from player to camera.
        bool ReverseViewingPosCheck(Vector3 checkPos) {
            // Cast origin and direction.
            Vector3 origin = player.position + pivotOffset;
            Vector3 direction = checkPos - origin;
            if (Physics.SphereCast(origin, 0.2f, direction, out RaycastHit hit, direction.magnitude, layerMaskExcludeCollisionWithCamera)) {
                if (hit.transform != player && hit.transform != transform &&
                    !hit.transform.GetComponent<Collider>().isTrigger) {
                    return false;
                }
            }

            return true;
        }

        // Get camera magnitude.
        public float GetCurrentPivotMagnitude(Vector3 finalPivotOffset) {
            return Mathf.Abs((finalPivotOffset - _smoothPivotOffset).magnitude);
        }
    }
}