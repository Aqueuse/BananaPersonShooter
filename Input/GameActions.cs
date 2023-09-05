using Enums;
using Player;
using UnityEngine;

namespace Input {
    public class GameActions : MonoBehaviour {
        public Vector2 move;
        public Vector2 scrollSlotsValue;
        
        private PlayerController _playerController;

        private bool _scrolledUp;
        private bool _scrolledDown;
        
        public bool _leftTriggerActivated;
        public bool rightTriggerActivated;
        public bool leftClickActivated;
        
        private void Start() {
            move = new Vector2();

            scrollSlotsValue = new Vector2();

            _playerController = ObjectsReference.Instance.playerController;
            
            _leftTriggerActivated = false;
        }

        private void Update() {
            Move();
            Jump();
            Run();
            Roll();
            
            Interact();
            Grab();
            Release();

            PauseGame();

            ZoomDezoomCamera();
            
            if (ObjectsReference.Instance.bananaMan.tutorialFinished) {
                Scroll_Slots();
                SwitchToUpperSlot();
                SwitchToLowerSlot();
        
                SwitchToSlotIndex0();
                SwitchToSlotIndex1();
                SwitchToSlotIndex2();
                SwitchToSlotIndex3();

                Shoot();
                Eat();

                if (_playerController.isGrounded) CheckBuildMode();
            }
        }
        
        private void Move() {
            if (!_playerController.canMove) move = Vector2.zero;

            else {
                move.x = UnityEngine.Input.GetAxis("Horizontal");
                move.y = UnityEngine.Input.GetAxis("Vertical");
            }
        }

        private void Jump() {
            if (!_playerController.canMove) return;
            
            if (UnityEngine.Input.GetKeyDown(KeyCode.Space) || UnityEngine.Input.GetKeyDown(KeyCode.JoystickButton0)) {
                _playerController.PlayerJump();
            }
        }

        private void Run() {
            if (
                UnityEngine.Input.GetKeyDown(KeyCode.LeftShift) || 
                UnityEngine.Input.GetKeyDown(KeyCode.JoystickButton4) ||
                UnityEngine.Input.GetKeyDown(KeyCode.JoystickButton8)
                ) {
                _playerController.PlayerSprint();
            }

            if (UnityEngine.Input.GetKeyUp(KeyCode.LeftShift) || UnityEngine.Input.GetKeyUp(KeyCode.JoystickButton4)) {
                _playerController.PlayerStopSprint();
            }
        }

        private void Roll() {
            if (UnityEngine.Input.GetKeyDown(KeyCode.LeftControl) || UnityEngine.Input.GetKeyDown(KeyCode.JoystickButton1)) {
                _playerController.PlayerRoll();
            }
        }

        private static void Eat() {
            if (ObjectsReference.Instance.bananaMan.activeItemCategory == ItemCategory.BANANA && UnityEngine.Input.GetKeyDown(KeyCode.R) || UnityEngine.Input.GetKeyDown(KeyCode.JoystickButton3)) {
                ObjectsReference.Instance.bananaMan.GainHealth();
            }
        }

        private static void Grab() {
            if (UnityEngine.Input.GetKeyDown(KeyCode.F) || UnityEngine.Input.GetKeyDown(KeyCode.JoystickButton5)) {
                if (!ObjectsReference.Instance.interactionsManager.isGrabbing) {
                    ObjectsReference.Instance.interactionsManager.Grab();
                }
            }
        }

        private static void Release() {
            if (UnityEngine.Input.GetKeyUp(KeyCode.F) || UnityEngine.Input.GetKeyUp(KeyCode.JoystickButton2)) {
                ObjectsReference.Instance.interactionsManager.Release();
            }
        }

        private static void Interact() {
            if (UnityEngine.Input.GetKeyDown(KeyCode.E) || UnityEngine.Input.GetKeyDown(KeyCode.JoystickButton2)) {
                ObjectsReference.Instance.interactionsManager.Validate();
            }
        }

        private static void PauseGame() {
            if (UnityEngine.Input.GetKeyDown(KeyCode.Escape) || UnityEngine.Input.GetKeyDown(KeyCode.JoystickButton7) || UnityEngine.Input.GetKeyDown(KeyCode.JoystickButton15)) {
                
                ObjectsReference.Instance.inputManager.uiSchemaSwitcher.SwitchUISchema(UISchemaSwitchType.GAME_MENU);
                ObjectsReference.Instance.inputManager.SwitchContext(InputContext.UI);
                
                ObjectsReference.Instance.gameManager.PauseGame();
                ObjectsReference.Instance.uiManager.Show_game_menu();
            }
        }

        private void SwitchToUpperSlot() {
            if (UnityEngine.Input.GetAxis("DpadVertical") > 0 && !_scrolledUp) {
                ObjectsReference.Instance.uiSlotsManager.Select_Upper_Slot();
                _scrolledUp = true;
            }

            if (UnityEngine.Input.GetAxis("DpadVertical") == 0) {
                _scrolledUp = false;
                _scrolledDown = false;
            }
        }
    
        private void SwitchToLowerSlot() {
            if (UnityEngine.Input.GetAxis("DpadVertical") < 0 && !_scrolledDown) {
                ObjectsReference.Instance.uiSlotsManager.Select_Lower_Slot();
                _scrolledDown = true;
            }
        }
    
        private void Scroll_Slots() {
            scrollSlotsValue = UnityEngine.Input.mouseScrollDelta;
            
           var scrollValue = scrollSlotsValue.y;
            if (scrollValue < 0) ObjectsReference.Instance.uiSlotsManager.Select_Upper_Slot();
            if (scrollValue > 0) ObjectsReference.Instance.uiSlotsManager.Select_Lower_Slot();
        }
        
        private static void SwitchToSlotIndex0() {
            if (UnityEngine.Input.GetKeyDown(KeyCode.Alpha1)) {
                ObjectsReference.Instance.uiSlotsManager.Switch_to_Slot_Index(0);
            }
        }

        private static void SwitchToSlotIndex1() {
            if (UnityEngine.Input.GetKeyDown(KeyCode.Alpha2)) {
                ObjectsReference.Instance.uiSlotsManager.Switch_to_Slot_Index(1);
            }
        }
        
        private static void SwitchToSlotIndex2() {
            if (UnityEngine.Input.GetKeyDown(KeyCode.Alpha3)) {
                ObjectsReference.Instance.uiSlotsManager.Switch_to_Slot_Index(2);
            }
        }
        
        private static void SwitchToSlotIndex3() {
            if (UnityEngine.Input.GetKeyDown(KeyCode.Alpha4)) {
                ObjectsReference.Instance.uiSlotsManager.Switch_to_Slot_Index(3);
            }
        }
        
        private void ZoomDezoomCamera() {
            scrollSlotsValue = UnityEngine.Input.mouseScrollDelta;

            var scrollValue = scrollSlotsValue.y;

            if (UnityEngine.Input.GetKey(KeyCode.LeftShift) && scrollValue > 0) {
                ObjectsReference.Instance.mainCamera.ZoomCamera();
            }

            if (UnityEngine.Input.GetKey(KeyCode.LeftShift) && scrollValue < 0) {
                ObjectsReference.Instance.mainCamera.DezoomCamera();
            }

            if (UnityEngine.Input.GetKey(KeyCode.JoystickButton8) && UnityEngine.Input.GetAxis("Right Stick Y") > 0) {
                ObjectsReference.Instance.mainCamera.ZoomCamera();
            }

            if (UnityEngine.Input.GetKey(KeyCode.JoystickButton8) && UnityEngine.Input.GetAxis("Right Stick Y") < 0) {
                ObjectsReference.Instance.mainCamera.DezoomCamera();
            }
        }

        private void Shoot() {
            if (UnityEngine.Input.GetKeyDown(KeyCode.Mouse0)) {
                ObjectsReference.Instance.bananaGunPut.LoadingGun();
                leftClickActivated = true;
            }
            
            if (UnityEngine.Input.GetKeyUp(KeyCode.Mouse0)) {
                ObjectsReference.Instance.bananaGun.UngrabBananaGun();
                leftClickActivated = false;
            }
            
            if (UnityEngine.Input.GetAxis("RightTrigger") != 0 && !rightTriggerActivated) {
                rightTriggerActivated = true;
                ObjectsReference.Instance.bananaGunPut.LoadingGun();
            }
            
            if (UnityEngine.Input.GetAxis("RightTrigger") == 0 && rightTriggerActivated)  {
                ObjectsReference.Instance.bananaGun.UngrabBananaGun();
                rightTriggerActivated = false;
            }
        }
        
        private void CheckBuildMode() {
            if (UnityEngine.Input.GetKeyDown(KeyCode.Mouse1)) {
                ObjectsReference.Instance.bananaGun.GrabBananaGun();
                
                if (ObjectsReference.Instance.bananaMan.activeItemCategory == ItemCategory.BUILDABLE) {
                    ObjectsReference.Instance.uiCrosshairs.SetCrosshair(ItemCategory.BUILDABLE, BananaType.EMPTY);

                    ObjectsReference.Instance.uInventoriesManager.GetCurrentUIHelper().show_build_helper();
                    ObjectsReference.Instance.slotSwitch.ActivateGhost();
                }
            }

            if (UnityEngine.Input.GetKeyUp(KeyCode.Mouse1)) {
                ObjectsReference.Instance.bananaGun.UngrabBananaGun();
                
                ObjectsReference.Instance.uiCrosshairs.SetCrosshair(ItemCategory.EMPTY, BananaType.EMPTY);

                if (ObjectsReference.Instance.bananaMan.activeItemCategory == ItemCategory.BANANA) ObjectsReference.Instance.uInventoriesManager.GetCurrentUIHelper().show_banana_helper();
                else {
                    ObjectsReference.Instance.uInventoriesManager.GetCurrentUIHelper().show_default_helper();
                }
               
                ObjectsReference.Instance.slotSwitch.CancelGhost();
            }

            if (UnityEngine.Input.GetAxis("LeftTrigger") <= -0.1 &&
                !ObjectsReference.Instance.gameActions._leftTriggerActivated) {
                _leftTriggerActivated = true;
                
                ObjectsReference.Instance.bananaGun.GrabBananaGun();
                
                if (ObjectsReference.Instance.bananaMan.activeItemCategory == ItemCategory.BUILDABLE) {
                    ObjectsReference.Instance.slotSwitch.ActivateGhost();
                    ObjectsReference.Instance.uInventoriesManager.GetCurrentUIHelper().show_build_helper();

                    ObjectsReference.Instance.uiCrosshairs.SetCrosshair(ItemCategory.BUILDABLE, BananaType.EMPTY);
                }
            }

            if (UnityEngine.Input.GetAxis("LeftTrigger") >= 0 && 
                ObjectsReference.Instance.gameActions._leftTriggerActivated) {
                _leftTriggerActivated = false;
                
                ObjectsReference.Instance.bananaGun.UngrabBananaGun();
                
                ObjectsReference.Instance.uiCrosshairs.SetCrosshair(ItemCategory.EMPTY, BananaType.EMPTY);
                
                if (ObjectsReference.Instance.bananaMan.activeItemCategory == ItemCategory.BANANA) ObjectsReference.Instance.uInventoriesManager.GetCurrentUIHelper().show_banana_helper();
                else {
                    ObjectsReference.Instance.uInventoriesManager.GetCurrentUIHelper().show_default_helper();
                }

                ObjectsReference.Instance.slotSwitch.CancelGhost();
            }
        }

        private void OnDisable() {
            ObjectsReference.Instance.playerController.ResetPlayer();
        }
    }
}
