using InGame.Player;
using UnityEngine;
using UnityEngine.InputSystem;

namespace SharedInputs {
    public class GameActions : InputActions {
        public Vector2 move;
        public Vector2 scrollSlotsValue;

        private PlayerController _playerController;

        private bool _scrolledRight;
        private bool _scrolledLeft;

        private float leftRotateIncrementer;
        private float rightRotateIncrementer;
        private float topRotateIncrementer;
        private float downRotateIncrementer;

        public InputActionReference moveActionReference;
        public InputActionReference jumpActionReference;
        public InputActionReference runActionReference;
        public InputActionReference rollActionReference;

        public InputActionReference grabActionReference;
        public InputActionReference interactActionReference;
    
        public InputActionReference openBananaGunUIActionReference;
        public InputActionReference openInventoryActionReference;
        public InputActionReference ShowHideMapActionReference;

        public InputActionReference pauseGameActionReference;

        private void Start() {
            move = new Vector2();
            scrollSlotsValue = new Vector2();

            _playerController = ObjectsReference.Instance.playerController;
        }

        private void OnEnable() {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;

            ObjectsReference.Instance.cameraPlayer.SetNormalSensibility();

            ObjectsReference.Instance.playerController.canMove = true;
            ObjectsReference.Instance.bananaMan.GetComponent<Rigidbody>().isKinematic = false;

            ObjectsReference.Instance.uiManager.canvasGroupsByUICanvasType[UICanvasGroupType.CROSSHAIRS].alpha = 1;
            
            ObjectsReference.Instance.bananaGunActionsSwitch.gameObject.SetActive(true);

            moveActionReference.action.Enable();
            moveActionReference.action.performed += Move;
            moveActionReference.action.canceled += Move;

            jumpActionReference.action.Enable();
            jumpActionReference.action.performed += Jump;

            runActionReference.action.Enable();
            runActionReference.action.performed += Run;
            runActionReference.action.canceled += Run;

            rollActionReference.action.Enable();
            rollActionReference.action.performed += Roll;

            interactActionReference.action.Enable();
            interactActionReference.action.performed += Interact;

            grabActionReference.action.Enable();
            grabActionReference.action.performed += Grab;
            grabActionReference.action.canceled += Grab;
        
            openBananaGunUIActionReference.action.Enable();
            openBananaGunUIActionReference.action.performed += OpenBananaGunUI;
            
            openInventoryActionReference.action.Enable();
            openInventoryActionReference.action.performed += OpenInventories;
            
            ShowHideMapActionReference.action.Enable();
            ShowHideMapActionReference.action.performed += ShowHideMap;
            
            pauseGameActionReference.action.Enable();
            pauseGameActionReference.action.performed += PauseGame;
        }

        private void OnDisable() {
            jumpActionReference.action.Disable();
            jumpActionReference.action.performed -= Jump;

            moveActionReference.action.Disable();
            moveActionReference.action.performed -= Move;
            moveActionReference.action.canceled -= Move;

            runActionReference.action.Disable();
            runActionReference.action.performed -= Run;

            rollActionReference.action.Disable();
            rollActionReference.action.performed -= Roll;

            interactActionReference.action.Disable();
            interactActionReference.action.performed -= Interact;

            grabActionReference.action.Disable();
            grabActionReference.action.performed -= Grab;
            grabActionReference.action.canceled -= Grab;
        
            openBananaGunUIActionReference.action.Disable();
            openBananaGunUIActionReference.action.performed -= OpenBananaGunUI;

            openInventoryActionReference.action.Disable();
            openInventoryActionReference.action.performed -= OpenInventories;
            
            ShowHideMapActionReference.action.Disable();
            ShowHideMapActionReference.action.performed -= ShowHideMap;
            
            pauseGameActionReference.action.Disable();
            pauseGameActionReference.action.performed -= PauseGame;
        }

        private void Move(InputAction.CallbackContext context) {
            if (!_playerController.canMove) move = Vector2.zero;

            move.x = context.ReadValue<Vector2>().x;
            move.y = context.ReadValue<Vector2>().y;
        }

        private void Jump(InputAction.CallbackContext context) {
            if (!_playerController.canMove) return;

            if (context.performed) {
                _playerController.PlayerJump();
            }
        }

        private void Run(InputAction.CallbackContext context) {
            if (context.performed) {
                _playerController.PlayerSprint();
            }

            if (context.canceled) {
                _playerController.PlayerStopSprint();
            }
        }

        private void Roll(InputAction.CallbackContext context) {
            if (_playerController.isGrounded) _playerController.PlayerRoll();
        }

        private static void Interact(InputAction.CallbackContext context) {
            ObjectsReference.Instance.interact.Validate();
        }

        private static void Grab(InputAction.CallbackContext context) {
            if (ObjectsReference.Instance.bananaMan.bananaGunMode != BananaGunMode.SCAN) {
                if (context.performed) ObjectsReference.Instance.grab.DoGrab(); 
                if (context.canceled) ObjectsReference.Instance.grab.Release();
            }
        }
    
        private static void OpenBananaGunUI(InputAction.CallbackContext context) {
            if (!ObjectsReference.Instance.bananaMan.tutorialFinished) return;

            ObjectsReference.Instance.uiManager.ShowBananaGunUI();
            ObjectsReference.Instance.uiManager.SwitchToMinichimpPerspective();
            
            ObjectsReference.Instance.gameManager.gameContext = GameContext.IN_MINICHIMP_VIEW;

            ObjectsReference.Instance.bananaGunActionsSwitch.gameObject.SetActive(false);
        }

        private static void OpenInventories(InputAction.CallbackContext context) {
            if (!ObjectsReference.Instance.bananaMan.tutorialFinished) return;
            
            ObjectsReference.Instance.uiManager.ShowInventory();
            ObjectsReference.Instance.cameraPlayer.Set0Sensibility();
            ObjectsReference.Instance.inputManager.SwitchContext(InputContext.INVENTORIES);
            ObjectsReference.Instance.gameManager.gameContext = GameContext.IN_INVENTORIES;
            
            ObjectsReference.Instance.bananaGunActionsSwitch.gameObject.SetActive(false);
        }

        private static void ShowHideMap(InputAction.CallbackContext context) {
            ObjectsReference.Instance.uiManager.ShowHideMap();
        }
        
        private void ZoomDezoomCamera(InputAction.CallbackContext context) {
            var isPressedleftAlt = Keyboard.current.leftAltKey.isPressed;
            var isLeftStickPressed = Gamepad.current.leftStickButton.isPressed; 

            if (!isPressedleftAlt || !isLeftStickPressed) return;

            scrollSlotsValue = context.ReadValue<Vector2>();
            var scrollValue = scrollSlotsValue.y;

            if (scrollValue > 0) {
                ObjectsReference.Instance.cameraPlayer.ZoomCamera();
            }

            if (scrollValue < 0) {
                ObjectsReference.Instance.cameraPlayer.DezoomCamera();
            }
        }

        private static void PauseGame(InputAction.CallbackContext context) {
            if (ObjectsReference.Instance.bananaMan.isGrabingBananaGun)
                ObjectsReference.Instance.build.CancelBuild();

            else {
                ObjectsReference.Instance.inputManager.SwitchContext(InputContext.UI);

                ObjectsReference.Instance.gameManager.PauseGame();
                ObjectsReference.Instance.uiManager.ShowGameMenu();
            }
        }
    }
}