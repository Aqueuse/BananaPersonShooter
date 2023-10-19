using System.Collections.Generic;
using Enums;
using Player;
using UnityEngine;

namespace Input {
    public class GameActions : MonoBehaviour {
        public Vector2 move;
        public Vector2 scrollSlotsValue;

        private PlayerController _playerController;

        private bool _scrolledRight;
        private bool _scrolledLeft;

        public bool _leftTriggerActivated;
        public bool rightTriggerActivated;
        public bool leftClickActivated;
        
        private float leftRotateIncrementer;
        private float rightRotateIncrementer;
        private float topRotateIncrementer;
        private float downRotateIncrementer;
        
        private Dictionary<RotationAxis, Vector3> rotationAxisToVector3Direction;

        private const float _rotationSpeed = 0.2f;
        
        private void Start() {
            rotationAxisToVector3Direction = new Dictionary<RotationAxis, Vector3>() {
                {RotationAxis.Y, Vector3.up},
                {RotationAxis.Z, Vector3.forward}
            };

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
                SwitchToRightSlot();
                SwitchToLeftSlot();

                SwitchToSlotIndex0();
                SwitchToSlotIndex1();
                SwitchToSlotIndex2();
                SwitchToSlotIndex3();

                if (ObjectsReference.Instance.bananaMan.activeItemCategory == ItemCategory.BANANA) Shoot();

                Eat();

                if (ObjectsReference.Instance.bananaMan.activeItemCategory == ItemCategory.BUILDABLE) {
                    Build();
                    Rotate();
                    TranslateZBananaGunTarget();
                }

                TargetObject();
                Harvest();
                
                SwitchToConstructionMode();
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
            if (ObjectsReference.Instance.bananaMan.activeItemCategory == ItemCategory.BANANA &&
                UnityEngine.Input.GetKeyDown(KeyCode.R) || UnityEngine.Input.GetKeyDown(KeyCode.JoystickButton3)) {
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
            if (UnityEngine.Input.GetKeyDown(KeyCode.Escape) || UnityEngine.Input.GetKeyDown(KeyCode.JoystickButton7) ||
                UnityEngine.Input.GetKeyDown(KeyCode.JoystickButton15)) {
                ObjectsReference.Instance.inputManager.uiSchemaSwitcher.SwitchUISchema(UISchemaSwitchType.GAME_MENU);
                ObjectsReference.Instance.inputManager.SwitchContext(InputContext.UI);

                ObjectsReference.Instance.gameManager.PauseGame();
                ObjectsReference.Instance.uiManager.Show_game_menu();
            }
        }
        
        private void SwitchToLeftSlot() {
            if (UnityEngine.Input.GetKeyDown(KeyCode.JoystickButton4)) {
                ObjectsReference.Instance.uiQuickSlotsManager.Select_Lefter_Slot();
            }
        }
        
        private void SwitchToRightSlot() {
            if (UnityEngine.Input.GetKeyDown(KeyCode.JoystickButton5)) {
                ObjectsReference.Instance.uiQuickSlotsManager.Select_Righter_Slot();
            }
        }
        
        private void Scroll_Slots() {
            if (UnityEngine.Input.GetKey(KeyCode.LeftControl)) return;
            
            scrollSlotsValue = UnityEngine.Input.mouseScrollDelta;

            var scrollValue = scrollSlotsValue.y;
            if (scrollValue < 0) ObjectsReference.Instance.uiQuickSlotsManager.Select_Lefter_Slot();
            if (scrollValue > 0) ObjectsReference.Instance.uiQuickSlotsManager.Select_Righter_Slot();
        }

        private static void SwitchToSlotIndex0() {
            if (UnityEngine.Input.GetKeyDown(KeyCode.Alpha1)) {
                ObjectsReference.Instance.uiQuickSlotsManager.Switch_to_Slot_Index(0);
            }
        }

        private static void SwitchToSlotIndex1() {
            if (UnityEngine.Input.GetKeyDown(KeyCode.Alpha2)) {
                ObjectsReference.Instance.uiQuickSlotsManager.Switch_to_Slot_Index(1);
            }
        }

        private static void SwitchToSlotIndex2() {
            if (UnityEngine.Input.GetKeyDown(KeyCode.Alpha3)) {
                ObjectsReference.Instance.uiQuickSlotsManager.Switch_to_Slot_Index(2);
            }
        }

        private static void SwitchToSlotIndex3() {
            if (UnityEngine.Input.GetKeyDown(KeyCode.Alpha4)) {
                ObjectsReference.Instance.uiQuickSlotsManager.Switch_to_Slot_Index(3);
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
                ObjectsReference.Instance.throwBanana.LoadingGun();
                leftClickActivated = true;
            }

            if (UnityEngine.Input.GetKeyUp(KeyCode.Mouse0)) {
                ObjectsReference.Instance.bananaGun.UngrabBananaGun();
                leftClickActivated = false;
            }

            if (UnityEngine.Input.GetAxis("RightTrigger") != 0 && !rightTriggerActivated) {
                rightTriggerActivated = true;
                ObjectsReference.Instance.throwBanana.LoadingGun();
            }

            if (UnityEngine.Input.GetAxis("RightTrigger") == 0 && rightTriggerActivated) {
                ObjectsReference.Instance.bananaGun.UngrabBananaGun();
                rightTriggerActivated = false;
            }
        }

        private void Build() {
            if (UnityEngine.Input.GetKeyDown(KeyCode.Mouse0)) {
                ObjectsReference.Instance.bananaGun.GrabBananaGun();
                ObjectsReference.Instance.build.ActivateGhost(ObjectsReference.Instance.bananaMan.activeBuildableType);
                ObjectsReference.Instance.uInventoriesManager.GetCurrentUIHelper().show_build_helper();
            }
            
            if (UnityEngine.Input.GetKeyUp(KeyCode.Mouse0)) {
                ObjectsReference.Instance.build.ValidateBuildable();
                ObjectsReference.Instance.build.CancelGhost();
                ObjectsReference.Instance.bananaGun.UngrabBananaGun();
                ObjectsReference.Instance.uInventoriesManager.GetCurrentUIHelper().show_default_helper();
            }
            
            if (UnityEngine.Input.GetAxis("RightTrigger") != 0 && !rightTriggerActivated) {
                rightTriggerActivated = true;
                ObjectsReference.Instance.bananaGun.GrabBananaGun();
                ObjectsReference.Instance.build.ActivateGhost(ObjectsReference.Instance.bananaMan.activeBuildableType);
                ObjectsReference.Instance.uInventoriesManager.GetCurrentUIHelper().show_build_helper();
            }

            if (UnityEngine.Input.GetAxis("RightTrigger") == 0 && rightTriggerActivated) {
                ObjectsReference.Instance.build.ValidateBuildable();
                ObjectsReference.Instance.build.CancelGhost();
                ObjectsReference.Instance.bananaGun.UngrabBananaGun();
                ObjectsReference.Instance.uInventoriesManager.GetCurrentUIHelper().show_default_helper();
                rightTriggerActivated = false;
            }
        }
        
        private void Rotate() {
            if (UnityEngine.Input.GetAxis("DpadHorizontal") < 0) {
                leftRotateIncrementer += Time.deltaTime;
                if (leftRotateIncrementer >= _rotationSpeed) {
                    leftRotateIncrementer = 0;
                    ObjectsReference.Instance.build.RotateGhost(rotationAxisToVector3Direction[RotationAxis.Y]);
                }

                return;
            }

            if (UnityEngine.Input.GetAxis("DpadHorizontal") > 0) {
                rightRotateIncrementer += Time.deltaTime;
                if (rightRotateIncrementer >= _rotationSpeed) {
                    rightRotateIncrementer = 0;
                    ObjectsReference.Instance.build.RotateGhost(-rotationAxisToVector3Direction[RotationAxis.Y]);
                }
            }

            if (UnityEngine.Input.GetKeyDown(KeyCode.LeftArrow)) {
                ObjectsReference.Instance.build.RotateGhost(-rotationAxisToVector3Direction[RotationAxis.Y]);
                return;
            }

            if (UnityEngine.Input.GetKeyDown(KeyCode.RightArrow)) {
                ObjectsReference.Instance.build.RotateGhost(rotationAxisToVector3Direction[RotationAxis.Y]);
            }

            if (ObjectsReference.Instance.bananaMan.activeBuildableType == BuildableType.PLATEFORM) {
                if (UnityEngine.Input.GetAxis("DpadVertical") < 0) {
                    topRotateIncrementer += Time.deltaTime;
                    if (topRotateIncrementer >= _rotationSpeed) {
                        topRotateIncrementer = 0;
                        ObjectsReference.Instance.build.RotateGhost(rotationAxisToVector3Direction[RotationAxis.Z]);
                    }

                    return;
                }

                if (UnityEngine.Input.GetAxis("DpadVertical") > 0) {
                    downRotateIncrementer += Time.deltaTime;
                    if (downRotateIncrementer >= _rotationSpeed) {
                        downRotateIncrementer = 0;
                        ObjectsReference.Instance.build.RotateGhost(-rotationAxisToVector3Direction[RotationAxis.Z]);
                    }

                    return;
                }

                if (UnityEngine.Input.GetKeyDown(KeyCode.UpArrow)) {
                    ObjectsReference.Instance.build.RotateGhost(rotationAxisToVector3Direction[RotationAxis.Z]);
                    return;
                }

                if (UnityEngine.Input.GetKeyDown(KeyCode.DownArrow)) {
                    ObjectsReference.Instance.build.RotateGhost(-rotationAxisToVector3Direction[RotationAxis.Z]);
                }
            }
        }

        private void TranslateZBananaGunTarget() {
            scrollSlotsValue = UnityEngine.Input.mouseScrollDelta;
            var scrollValue = scrollSlotsValue.y;

            if (UnityEngine.Input.GetKey(KeyCode.LeftControl) && scrollValue > 0) {
                var placementLocalPosition = ObjectsReference.Instance.uiHud.buildablePlacementTransform.localPosition;
                if (placementLocalPosition.z < 25) {
                    placementLocalPosition.z += 1f;
                    ObjectsReference.Instance.uiHud.buildablePlacementTransform.localPosition = placementLocalPosition;
                }
            } 
            
            if (UnityEngine.Input.GetKeyDown(KeyCode.JoystickButton3)) {
                var placementLocalPosition = ObjectsReference.Instance.uiHud.buildablePlacementTransform.localPosition;
                if (placementLocalPosition.z < 25) {
                    placementLocalPosition.z += 1f;
                    ObjectsReference.Instance.uiHud.buildablePlacementTransform.localPosition = placementLocalPosition;
                }
            }
            
            if (UnityEngine.Input.GetKey(KeyCode.LeftControl) && scrollValue < 0) {
                var placementLocalPosition = ObjectsReference.Instance.uiHud.buildablePlacementTransform.localPosition;
                if (placementLocalPosition.z > 4) {
                    placementLocalPosition.z -= 1f;
                    ObjectsReference.Instance.uiHud.buildablePlacementTransform.localPosition = placementLocalPosition;
                }
            }
            
            if (UnityEngine.Input.GetKeyDown(KeyCode.JoystickButton2)) {
                var placementLocalPosition = ObjectsReference.Instance.uiHud.buildablePlacementTransform.localPosition;
                if (placementLocalPosition.z > 4) {
                    placementLocalPosition.z -= 1f;
                    ObjectsReference.Instance.uiHud.buildablePlacementTransform.localPosition = placementLocalPosition;
                }
            }
        }
        
        private void TargetObject() {
            if (UnityEngine.Input.GetKeyDown(KeyCode.Mouse1)) {
                ObjectsReference.Instance.bananaGun.GrabBananaGun();

                ObjectsReference.Instance.harvest.isDirectHarvestActivated = true;
            }
            
            if (UnityEngine.Input.GetKeyUp(KeyCode.Mouse1)) {
                ObjectsReference.Instance.bananaGun.UngrabBananaGun();
                
                ObjectsReference.Instance.harvest.isDirectHarvestActivated = false;
            }
            
            if (UnityEngine.Input.GetAxis("LeftTrigger") != 0 && !_leftTriggerActivated) {
                _leftTriggerActivated = true;
                ObjectsReference.Instance.bananaGun.GrabBananaGun();

                ObjectsReference.Instance.harvest.isDirectHarvestActivated = true;
            }

            if (UnityEngine.Input.GetAxis("LeftTrigger") == 0 && _leftTriggerActivated) {
                ObjectsReference.Instance.bananaGun.UngrabBananaGun();
                
                ObjectsReference.Instance.harvest.isDirectHarvestActivated = false;
                _leftTriggerActivated = false;
            }
        }

        private void Harvest() {
            if (UnityEngine.Input.GetKeyDown(KeyCode.F) || UnityEngine.Input.GetKeyDown(KeyCode.JoystickButton1)) {
                ObjectsReference.Instance.harvest.harvest();
            }
        }
        
        private void SwitchToConstructionMode() {
            if (UnityEngine.Input.GetKeyDown(KeyCode.Tab) || UnityEngine.Input.GetKeyDown(KeyCode.JoystickButton6)) {
                ObjectsReference.Instance.gestionMode.SwitchToGestionMode();
            }
        }

        // private void OnDisable() {
        //     ObjectsReference.Instance.playerController.ResetPlayer();
        // }
    }
}