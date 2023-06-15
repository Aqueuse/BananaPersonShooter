using System.Collections.Generic;
using Enums;
using Player;
using UI.Tutorials;
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
        
        private float leftRotateIncrementer;
        private float rightRotateIncrementer;
        private float topRotateIncrementer;
        private float downRotateIncrementer;

        private const float _rotationSpeed = 0.2f;
        
        private RotationAxis rotationAxis = RotationAxis.Y;
        private Dictionary<RotationAxis, Vector3> rotationAxisToVector3Direction;

        public bool isBuildModeActivated;

        private void Start() {
            move = new Vector2();

            scrollSlotsValue = new Vector2();

            _playerController = ObjectsReference.Instance.playerController;
            
            _leftTriggerActivated = false;

            isBuildModeActivated = false;
            
            rotationAxisToVector3Direction = new Dictionary<RotationAxis, Vector3>() {
                {RotationAxis.Y, Vector3.up},
                {RotationAxis.Z, Vector3.forward}
            };
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
            ShowTutorial();

            ZoomDezoomCamera();
            
            if (ObjectsReference.Instance.gameData.bananaManSavedData.playerAdvancements
                .Contains(AdvancementState.GET_BANANAGUN)) {
                Scroll_Slots();
                SwitchToUpperSlot();
                SwitchToLowerSlot();
            
                SwitchToSlotIndex0();
                SwitchToSlotIndex1();
                SwitchToSlotIndex2();
                SwitchToSlotIndex3();
                
                if (!ObjectsReference.Instance.uiManager.Is_Interface_Visible()) {
                    Show_Inventory();

                    Shoot();
                    Reload();
                    Eat();

                    if (_playerController.isGrounded) CheckBuildMode();
                }
            
                if (isBuildModeActivated) {
                    Harvest();
                    SwitchRotationAxis();
                    Build();
                    Rotate();
                }
            }
        }
        
        private void Move() {
            if (ObjectsReference.Instance.bananaMan.activeItemCategory == ItemCategory.BUILDABLE && !_playerController.canMove) move = Vector2.zero;

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
            if (UnityEngine.Input.GetKeyDown(KeyCode.F) || UnityEngine.Input.GetKeyDown(KeyCode.JoystickButton2)) {
                if (!ObjectsReference.Instance.itemsManager.isGrabbing) {
                    ObjectsReference.Instance.itemsManager.Grab();
                }
            }
        }

        private static void Release() {
            if (UnityEngine.Input.GetKeyUp(KeyCode.F) || UnityEngine.Input.GetKeyUp(KeyCode.JoystickButton2)) {
                ObjectsReference.Instance.itemsManager.Release();
            }
        }

        private static void Interact() {
            if (UnityEngine.Input.GetKeyDown(KeyCode.E) || UnityEngine.Input.GetKeyDown(KeyCode.JoystickButton2)) {
                ObjectsReference.Instance.itemsManager.Validate();
            }
        }

        private static void Show_Inventory() {
            if (UnityEngine.Input.GetKeyDown(KeyCode.I) || UnityEngine.Input. GetAxis("DpadHorizontal") < 0) {
                ObjectsReference.Instance.uiManager.Show_Hide_interface();
            }
        }

        private static void PauseGame() {
            if (UnityEngine.Input.GetKeyDown(KeyCode.Escape) || UnityEngine.Input.GetKeyDown(KeyCode.JoystickButton7)) {
                ObjectsReference.Instance.inputManager.uiSchemaContext = UISchemaSwitchType.GAME_MENU;
                ObjectsReference.Instance.gameManager.PauseGame(true);
                ObjectsReference.Instance.uiManager.Show_game_menu();
            }
        }

        private static void ShowTutorial() {
            if (UnityEngine.Input.GetKeyDown(KeyCode.H) || UnityEngine.Input.GetKeyDown(KeyCode.JoystickButton6)) {
                ObjectsReference.Instance.inputManager.uiSchemaContext = UISchemaSwitchType.TUTORIAL;
                ObjectsReference.Instance.gameManager.PauseGame(true);
                TutorialsManager.Show_Help();
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

        private static void Reload() {
            if (UnityEngine.Input.GetKeyDown(KeyCode.R) || UnityEngine.Input.GetKeyDown(KeyCode.JoystickButton2)) {
                // reload
            }
        }

        private void CheckBuildMode() {
            if (UnityEngine.Input.GetKeyDown(KeyCode.Mouse1)) {
                ObjectsReference.Instance.bananaGun.GrabBananaGun();
                _playerController.StopPlayer();
                
                ObjectsReference.Instance.uiCrosshair.SetCrosshair(ItemCategory.BUILDABLE, ItemType.EMPTY);
                
                ObjectsReference.Instance.uiManager.canvasGroupsByUICanvasType[UICanvasGroupType.BANANAGUN_HELPER].alpha = 1f;
                if (ObjectsReference.Instance.bananaMan.activeItemCategory == ItemCategory.BUILDABLE) {
                    ObjectsReference.Instance.uiManager.canvasGroupsByUICanvasType[UICanvasGroupType.BUILD_HELPER].alpha = 1f;
                }
                
                if (ObjectsReference.Instance.bananaMan.activeItemCategory == ItemCategory.BUILDABLE) ObjectsReference.Instance.slotSwitch.ActivateGhost();
                
                isBuildModeActivated = true;
            }

            if (UnityEngine.Input.GetKeyUp(KeyCode.Mouse1)) {
                ObjectsReference.Instance.bananaGun.UngrabBananaGun();
                _playerController.canMove = true;
                
                ObjectsReference.Instance.bananaGun.UnhighlightSelectedObject();
                ObjectsReference.Instance.uiCrosshair.SetCrosshair(ItemCategory.EMPTY, ItemType.EMPTY);
                
                ObjectsReference.Instance.uiManager.canvasGroupsByUICanvasType[UICanvasGroupType.BANANAGUN_HELPER].alpha = 0f;
                ObjectsReference.Instance.uiManager.canvasGroupsByUICanvasType[UICanvasGroupType.BUILD_HELPER].alpha = 0f;

                ObjectsReference.Instance.slotSwitch.CancelGhost();
                
                isBuildModeActivated = false;
            }

            if (UnityEngine.Input.GetAxis("LeftTrigger") <= -0.1 &&
                !ObjectsReference.Instance.gameActions._leftTriggerActivated) {
                _leftTriggerActivated = true;
                
                ObjectsReference.Instance.bananaGun.GrabBananaGun();
                _playerController.StopPlayer();
                
                ObjectsReference.Instance.uiCrosshair.SetCrosshair(ItemCategory.BUILDABLE, ItemType.EMPTY);
                ObjectsReference.Instance.uiManager.canvasGroupsByUICanvasType[UICanvasGroupType.BUILD_HELPER].alpha = 1f;

                if (ObjectsReference.Instance.bananaMan.activeItemCategory == ItemCategory.BUILDABLE) ObjectsReference.Instance.slotSwitch.ActivateGhost();

                isBuildModeActivated = true;
            }

            if (UnityEngine.Input.GetAxis("LeftTrigger") >= 0 && 
                ObjectsReference.Instance.gameActions._leftTriggerActivated) {
                _leftTriggerActivated = false;
                
                ObjectsReference.Instance.bananaGun.UngrabBananaGun();
                _playerController.canMove = true;
                
                ObjectsReference.Instance.uiCrosshair.SetCrosshair(ItemCategory.EMPTY, ItemType.EMPTY);
                ObjectsReference.Instance.bananaGun.UnhighlightSelectedObject();
                ObjectsReference.Instance.uiManager.canvasGroupsByUICanvasType[UICanvasGroupType.BUILD_HELPER].alpha = 0f;

                ObjectsReference.Instance.slotSwitch.CancelGhost();

                isBuildModeActivated = false;
            }
        }
        
        private static void Harvest() {
            if (UnityEngine.Input.GetKeyDown(KeyCode.F) || UnityEngine.Input.GetKeyDown(KeyCode.JoystickButton2)) {
                ObjectsReference.Instance.bananaGunGet.Harvest();
            }
        }
        
        private static void Build() {
            if (UnityEngine.Input.GetKeyDown(KeyCode.E) || UnityEngine.Input.GetKeyDown(KeyCode.JoystickButton0)) {
                ObjectsReference.Instance.slotSwitch.ValidateBuildable();
            }
        }

        private void SwitchRotationAxis() {  // remembering that we are virtyally in a QWERTY keyboard all the time because Unity
            if (UnityEngine.Input.GetKeyDown(KeyCode.Q) || UnityEngine.Input.GetKeyDown(KeyCode.JoystickButton3)) {
                rotationAxis = RotationAxis.Y;
                ObjectsReference.Instance.uiManager.canvasGroupsByUICanvasType[UICanvasGroupType.BUILD_HELPER_Y_AXIS].alpha = 1f;
                ObjectsReference.Instance.uiManager.canvasGroupsByUICanvasType[UICanvasGroupType.BUILD_HELPER_Z_AXIS].alpha = 0f;
            }
            
            if (UnityEngine.Input.GetKeyDown(KeyCode.W) || UnityEngine.Input.GetKeyDown(KeyCode.JoystickButton1)) {
                rotationAxis = RotationAxis.Z;
                ObjectsReference.Instance.uiManager.canvasGroupsByUICanvasType[UICanvasGroupType.BUILD_HELPER_Y_AXIS].alpha = 0f;
                ObjectsReference.Instance.uiManager.canvasGroupsByUICanvasType[UICanvasGroupType.BUILD_HELPER_Z_AXIS].alpha = 1f;
            }
        }

        private void Rotate() {
            if (isBuildModeActivated) {
                if (UnityEngine.Input.GetAxis("Horizontal") < 0) {
                    leftRotateIncrementer += Time.deltaTime;
                    if (leftRotateIncrementer >= _rotationSpeed) {
                        leftRotateIncrementer = 0;
                        ObjectsReference.Instance.slotSwitch.RotateGhost(rotationAxisToVector3Direction[rotationAxis]);
                    }
                    return;
                }
                if (UnityEngine.Input.GetAxis("Horizontal") > 0) {
                    rightRotateIncrementer += Time.deltaTime;
                    if (rightRotateIncrementer >= _rotationSpeed) {
                        rightRotateIncrementer = 0;
                        ObjectsReference.Instance.slotSwitch.RotateGhost(-rotationAxisToVector3Direction[rotationAxis]);
                    }
                }
            }
        }

        private void OnDisable() {
            ObjectsReference.Instance.playerController.ResetPlayer();
        }
    }
}
