using Player;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Input {
    public class GameActions : MonoBehaviour {
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
        public InputActionReference eatActionReference;
        
        public InputActionReference grabActionReference;

        public InputActionReference interactActionReference;
        public InputActionReference pauseGameActionReference;
        
        public InputActionReference shootOrConfirmBuildActionReference;
 
        public InputActionReference buildActionReference;
        public InputActionReference targetObjectActionReference;
        public InputActionReference harvestActionReference;

        public InputActionReference rotateGhostActionReference;
        public InputActionReference moveAwayCloserGhostActionReference;

        public InputActionReference switchToInventoryActionReference;

        private void Start() {
            move = new Vector2();
            scrollSlotsValue = new Vector2();

            _playerController = ObjectsReference.Instance.playerController;
        }
        
        private void OnEnable() {
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

            eatActionReference.action.Enable();
            eatActionReference.action.performed += Eat;

            grabActionReference.action.Enable();
            grabActionReference.action.performed += Grab;
            grabActionReference.action.canceled += Grab;

            interactActionReference.action.Enable();
            interactActionReference.action.performed += Interact;
            
            pauseGameActionReference.action.Enable();
            pauseGameActionReference.action.performed += PauseGame;
            
            shootOrConfirmBuildActionReference.action.Enable();
            shootOrConfirmBuildActionReference.action.performed += ShootOrConfirmBuild;
            shootOrConfirmBuildActionReference.action.canceled += ShootOrConfirmBuild;

            buildActionReference.action.Enable();
            buildActionReference.action.performed += Build;
            buildActionReference.action.canceled += Build;

            targetObjectActionReference.action.Enable();
            targetObjectActionReference.action.performed += TargetObject;
            targetObjectActionReference.action.canceled += TargetObject;
            
            harvestActionReference.action.Enable();
            harvestActionReference.action.performed += Harvest;
            
            rotateGhostActionReference.action.Enable();
            rotateGhostActionReference.action.performed += Rotate;
            
            moveAwayCloserGhostActionReference.action.Enable();
            moveAwayCloserGhostActionReference.action.performed += MoveAwayCloserGhostTarget;
            
            switchToInventoryActionReference.action.Enable();
            switchToInventoryActionReference.action.performed += SwitchToInventory;
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
            
            eatActionReference.action.Disable();
            eatActionReference.action.performed -= Eat;
            
            grabActionReference.action.Disable();
            grabActionReference.action.performed -= Grab;
            grabActionReference.action.canceled -= Grab;

            interactActionReference.action.Disable();
            interactActionReference.action.performed -= Interact;
            
            pauseGameActionReference.action.Disable();
            pauseGameActionReference.action.performed -= PauseGame;
            
            shootOrConfirmBuildActionReference.action.Disable();
            shootOrConfirmBuildActionReference.action.performed -= ShootOrConfirmBuild;
            shootOrConfirmBuildActionReference.action.canceled -= ShootOrConfirmBuild;

            buildActionReference.action.Disable();
            buildActionReference.action.performed -= Build;
            buildActionReference.action.canceled -= Build;

            targetObjectActionReference.action.Disable();
            targetObjectActionReference.action.performed -= TargetObject;
            targetObjectActionReference.action.canceled -= TargetObject;
            
            harvestActionReference.action.Disable();
            harvestActionReference.action.performed -= Harvest;

            rotateGhostActionReference.action.Disable();
            rotateGhostActionReference.action.performed -= Rotate;
            
            moveAwayCloserGhostActionReference.action.Disable();
            moveAwayCloserGhostActionReference.action.performed -= MoveAwayCloserGhostTarget;
            
            switchToInventoryActionReference.action.Disable();
            switchToInventoryActionReference.action.performed -= SwitchToInventory;
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

        private static void Eat(InputAction.CallbackContext context) {
            ObjectsReference.Instance.bananaMan.GainHealth();
        }

        private static void Grab(InputAction.CallbackContext context) {
            if (context.performed) ObjectsReference.Instance.interactionsManager.Grab(); 
            if (context.canceled) ObjectsReference.Instance.interactionsManager.Release();
        }
        
        private static void Interact(InputAction.CallbackContext context) {
            ObjectsReference.Instance.interactionsManager.Validate();
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
        
        private void ShootOrConfirmBuild(InputAction.CallbackContext context) {
            if (!ObjectsReference.Instance.bananaMan.tutorialFinished) return;

            ObjectsReference.Instance.bananaMan.activeItem = ObjectsReference.Instance.scriptableObjectManager.GetBananaScriptableObject(BananaType.CAVENDISH);
            
            if (context.performed) {
                if (ObjectsReference.Instance.build.isActivated) {
                    ObjectsReference.Instance.build.ValidateBuildable();                    
                    ObjectsReference.Instance.build.CancelGhost();
                    ObjectsReference.Instance.bananaGun.UngrabBananaGun();
                }

                else {
                    ObjectsReference.Instance.uiTools.ZoomShootIcon();
                    ObjectsReference.Instance.bananaGun.GrabBananaGun();

                    if (ObjectsReference.Instance.bananasInventory.GetQuantity(ObjectsReference.Instance.bananaMan.activeItem.bananaType) <= 0) return;
                
                    ObjectsReference.Instance.throwBanana.LoadingGun();
                    ObjectsReference.Instance.uiCrosshairs.SetCrosshair(ObjectsReference.Instance.bananaMan.activeItem.bananaType);
                }
            }

            if (context.canceled) {
                ObjectsReference.Instance.uiTools.UnzoomIcons();
                ObjectsReference.Instance.bananaGun.UngrabBananaGun();

                ObjectsReference.Instance.throwBanana.CancelThrow();
            }
        }
        
        private void Build(InputAction.CallbackContext context) {
            if (!ObjectsReference.Instance.bananaMan.tutorialFinished) return;

            if (context.performed) {
                ObjectsReference.Instance.uiTools.ZoomPlaceIcon();
                ObjectsReference.Instance.uInventoriesManager.GetCurrentUIHelper().ShowBuildHelper();

                ObjectsReference.Instance.bananaGun.GrabBananaGun();

                ObjectsReference.Instance.build.ActivatePlateformGhost();
            }

            if (context.canceled) {
                ObjectsReference.Instance.uInventoriesManager.GetCurrentUIHelper().ShowDefaultHelper();
                ObjectsReference.Instance.uiTools.UnzoomIcons();

                ObjectsReference.Instance.bananaGun.UngrabBananaGun();
                
                ObjectsReference.Instance.build.CancelGhost();
            }
        }
        
        private void TargetObject(InputAction.CallbackContext context) {
            if (!ObjectsReference.Instance.bananaMan.tutorialFinished) return;

            if (context.performed) {
                ObjectsReference.Instance.bananaGun.GrabBananaGun();
                ObjectsReference.Instance.uiTools.ZoomTakeIcon();
                
                ObjectsReference.Instance.harvest.isDirectHarvestActivated = true;
            }

            if (context.canceled) {
                ObjectsReference.Instance.uiTools.UnzoomIcons();
                ObjectsReference.Instance.descriptionsManager.HideAllPanels();

                ObjectsReference.Instance.bananaGun.UngrabBananaGun();
                
                ObjectsReference.Instance.harvest.isDirectHarvestActivated = false;
            }
        }

        private void Harvest(InputAction.CallbackContext context) {
            if (!ObjectsReference.Instance.bananaMan.tutorialFinished) return;
            
            ObjectsReference.Instance.harvest.harvest();
        }
        
        private void Rotate(InputAction.CallbackContext context) {
            if (!ObjectsReference.Instance.build.isActivated) return;

            var contextValue = context.ReadValue<Vector2>(); 
            
            if (contextValue.x < 0) {
                ObjectsReference.Instance.build.RotateGhost(Vector3.left);
            }

            if (contextValue.x > 0) {
                ObjectsReference.Instance.build.RotateGhost(Vector3.right);
            }
            
            if (contextValue.y < 0) {
                ObjectsReference.Instance.build.RotateGhost(Vector3.back);
            }

            if (contextValue.y > 0) {
                ObjectsReference.Instance.build.RotateGhost(Vector3.forward);
            }
        }

        private void MoveAwayCloserGhostTarget(InputAction.CallbackContext context) {
            if (!ObjectsReference.Instance.build.isActivated) return;

            var placementLocalPosition = ObjectsReference.Instance.uiHud.buildablePlacementTransform.localPosition;

            if (context.ReadValue<Vector2>().y > 1) {
                if (placementLocalPosition.z < 25) {
                    placementLocalPosition.z += 1f;
                    ObjectsReference.Instance.uiHud.buildablePlacementTransform.localPosition = placementLocalPosition;
                }
            }

            if (context.ReadValue<Vector2>().y < 1) {
                if (placementLocalPosition.z > 4) {
                    placementLocalPosition.z -= 1f;
                    ObjectsReference.Instance.uiHud.buildablePlacementTransform.localPosition = placementLocalPosition;
                }
            }
        }
        
        private void ZoomDezoomCamera(InputAction.CallbackContext context) {
            bool isPressedleftAlt = Keyboard.current.leftAltKey.isPressed;
            bool isLeftStickPressed = Gamepad.current.leftStickButton.isPressed; 
            
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

        private void SwitchToInventory(InputAction.CallbackContext context) {
            ObjectsReference.Instance.inputManager.SwitchContext(InputContext.INVENTORY);
            ObjectsReference.Instance.uiManager.ShowInventories();
        }
    }
}