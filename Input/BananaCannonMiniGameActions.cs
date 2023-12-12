using Game.BananaCannonMiniGame;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Input {
    public class BananaCannonMiniGameActions : MonoBehaviour {
        [SerializeField] private InputActionReference rotateCannonInputActionReference;
        [SerializeField] private InputActionReference shootInputActionReference;
        [SerializeField] private InputActionReference quitInputActionReference;

        private void OnEnable() {
            rotateCannonInputActionReference.action.Enable();
            rotateCannonInputActionReference.action.performed += RotateCannon;

            shootInputActionReference.action.Enable();
            shootInputActionReference.action.performed += Shoot;

            quitInputActionReference.action.Enable();
            quitInputActionReference.action.performed += Quit;
        }

        private void OnDisable() {
            rotateCannonInputActionReference.action.Disable();
            rotateCannonInputActionReference.action.performed -= RotateCannon;

            shootInputActionReference.action.Disable();
            shootInputActionReference.action.performed -= Shoot;

            quitInputActionReference.action.Disable();
            quitInputActionReference.action.performed -= Quit;
        }

        private void RotateCannon(InputAction.CallbackContext context) {
            BananaCannonMiniGameManager.Instance.MoveTarget(context.ReadValue<Vector2>().x, context.ReadValue<Vector2>().y);
        }

        private void Shoot(InputAction.CallbackContext context) {
            BananaCannonMiniGameManager.Instance.Shoot();
        }

        private void Quit(InputAction.CallbackContext context) {
            BananaCannonMiniGameManager.Instance.QuitMiniGame();
        }
    }
}