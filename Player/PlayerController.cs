using UnityEngine;
using UnityEngine.InputSystem;

namespace Player; 

public class PlayerController : MonoBehaviour {
    [Header("Movement Settings")] 
    public float baseMovementSpeed = 6f;
    public float sprintMovementSpeed = 9f;

    private TpsPlayerAnimator _tpsPlayerAnimator;

    //Stored Values
    private Vector3 _rawInputMovement;
    private Transform _mainCameraTransform;
        
    private void Start() {
        _tpsPlayerAnimator = GetComponentInChildren<TpsPlayerAnimator>();

        if (Camera.main != null) _mainCameraTransform = Camera.main.transform;
    }

    private void FixedUpdate() {
        if (GameManager.Instance.isPlaying) {
            Quaternion playerRotation = new Quaternion(0, _mainCameraTransform.transform.rotation.y, 0,
                _mainCameraTransform.rotation.w);

            transform.position += playerRotation * _rawInputMovement * (baseMovementSpeed * Time.deltaTime);

            // orientation of the player based on rawInputMovement values
            // +-----+------+-----+-----+
            // | Z\X |  -1  |  0  |  1  |
            // +-----+------+-----+-----+
            // |   1 |  -45 |   0 |  45 |
            // |   0 |  -90 |   0 |  90 |
            // | -1  | -135 | 180 | 135 |
            // +-----+------+-----+-----+

            if (_rawInputMovement != Vector3.zero) {
                switch (_rawInputMovement.z) {
                    case 1 when _rawInputMovement.x < 0:
                        transform.rotation = Quaternion.AngleAxis(-45, Vector3.up) * playerRotation;
                        break;
                    case 1 when _rawInputMovement.x == 0:
                        transform.rotation = playerRotation;
                        break;
                    case 1 when _rawInputMovement.x > 0:
                        transform.rotation = Quaternion.AngleAxis(45, Vector3.up) * playerRotation;
                        break;
                    case 0 when _rawInputMovement.x < 0:
                        transform.rotation = Quaternion.AngleAxis(-90, Vector3.up) * playerRotation;
                        break;
                    case 0 when _rawInputMovement.x == 0:
                        transform.rotation = playerRotation;
                        break;
                    case 0 when _rawInputMovement.x > 0:
                        transform.rotation = Quaternion.AngleAxis(90, Vector3.up) * playerRotation;
                        break;
                    case -1 when _rawInputMovement.x < 0:
                        transform.rotation = Quaternion.AngleAxis(-135, Vector3.up) * playerRotation;
                        break;
                    case -1 when _rawInputMovement.x == 0:
                        transform.rotation = Quaternion.AngleAxis(180, Vector3.up) * playerRotation;
                        break;
                    case -1 when _rawInputMovement.x > 0:
                        transform.rotation = Quaternion.AngleAxis(135, Vector3.up) * playerRotation;
                        break;
                }
            }

            _tpsPlayerAnimator.UpdateMovementAnimation(_rawInputMovement.z * baseMovementSpeed,
                _rawInputMovement.x * baseMovementSpeed);
        }
    }
        
    public void RotateForward() {
        transform.rotation = new Quaternion(0, _mainCameraTransform.transform.rotation.y, 0, _mainCameraTransform.rotation.w);
    }
        
    public void PlayerJump(InputAction.CallbackContext value) {
        if (value.performed && _rawInputMovement.z >= 0) {
            _tpsPlayerAnimator.Jump();
        }
    }

    public void PlayerMovement(InputAction.CallbackContext callbackContext) {
        Vector2 inputMovement = callbackContext.ReadValue<Vector2>();
        _rawInputMovement = new Vector3(inputMovement.x, 0, inputMovement.y); // Y en input => Z pour le player (forward)
    }
        
    public void PlayerSprint(InputAction.CallbackContext value) {
        baseMovementSpeed = sprintMovementSpeed;
        _tpsPlayerAnimator.GetComponent<Animator>().speed = 1.5f;

        if (value.canceled) {
            baseMovementSpeed = 6f;
            _tpsPlayerAnimator.GetComponent<Animator>().speed = 1f;
        }
    }
}