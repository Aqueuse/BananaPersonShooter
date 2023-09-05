using System.Collections.Generic;
using Enums;
using UnityEngine;

namespace Input.UIActions {
    public class UIInventoryActions : MonoBehaviour {
        private bool _scrolledUp;
        private bool _scrolledDown;

        private bool _scrolledLeft;
        private bool _scrolledRight;

        public Vector2 scrollSlotsValue;
        
        private float _counter;
        
        public bool rightTriggerActivated;
        
        private float leftRotateIncrementer;
        private float rightRotateIncrementer;
        private float topRotateIncrementer;
        private float downRotateIncrementer;

        private const float _rotationSpeed = 0.2f;
        
        private Dictionary<RotationAxis, Vector3> rotationAxisToVector3Direction;

        private void Start() {
            rotationAxisToVector3Direction = new Dictionary<RotationAxis, Vector3>() {
                {RotationAxis.Y, Vector3.up},
                {RotationAxis.Z, Vector3.forward}
            };
        }

        private void Update() {
            Hide_Interface();
            
            Switch_To_Right();
            Switch_To_Left();
            
            Switch_To_BananasInventory();
            Switch_To_Raw_MaterialsInventory();
            Switch_To_IngredientsInventory();
            Switch_To_BlueprintsInventory();
            
            Harvest();
            TranslateZBananaGunTarget();
                    
            Build();
            Rotate();
        }

        private void Hide_Interface() {
            if (UnityEngine.Input.GetKeyUp(KeyCode.Mouse1)) {
                ObjectsReference.Instance.uiManager.Hide_Interface();
                ObjectsReference.Instance.bananaGun.UngrabBananaGun();
            }
            
            if (UnityEngine.Input.GetAxis("RightTrigger") == 0 && rightTriggerActivated)  {
                ObjectsReference.Instance.uiManager.Hide_Interface();
                ObjectsReference.Instance.bananaGun.UngrabBananaGun();
                rightTriggerActivated = false;
            }
        }

        private static void Switch_To_Left() {
            if (UnityEngine.Input.GetKeyDown(KeyCode.JoystickButton4)) {
                ObjectsReference.Instance.uInventoriesManager.Switch_To_Left_Tab();
            }
        }

        private static void Switch_To_Right() {
            if (UnityEngine.Input.GetKeyDown(KeyCode.JoystickButton5)) {
                ObjectsReference.Instance.uInventoriesManager.Switch_To_Right_Tab();
            }
        }
        
        private void Switch_To_BananasInventory() {
            if (UnityEngine.Input.GetKeyDown(KeyCode.Alpha1)) {
                ObjectsReference.Instance.uInventoriesManager.Switch_To_Tab(ItemCategory.BANANA);
            }
        }
        
        private void Switch_To_Raw_MaterialsInventory() {
            if (UnityEngine.Input.GetKeyDown(KeyCode.Alpha2)) {
                ObjectsReference.Instance.uInventoriesManager.Switch_To_Tab(ItemCategory.RAW_MATERIAL);
            }
        }

        private void Switch_To_IngredientsInventory() {
            if (UnityEngine.Input.GetKeyDown(KeyCode.Alpha3)) {
                ObjectsReference.Instance.uInventoriesManager.Switch_To_Tab(ItemCategory.INGREDIENT);
            }
        }

        private void Switch_To_BlueprintsInventory() {
            if (UnityEngine.Input.GetKeyDown(KeyCode.Alpha4)) {
                ObjectsReference.Instance.uInventoriesManager.Switch_To_Tab(ItemCategory.BUILDABLE);
            }
        }
        
        private static void Harvest() {
            if (UnityEngine.Input.GetKeyDown(KeyCode.F) || UnityEngine.Input.GetKeyDown(KeyCode.JoystickButton1)) {
                ObjectsReference.Instance.bananaGunGet.Harvest();
            }
        }
        
        private static void Build() {
            if (UnityEngine.Input.GetKeyDown(KeyCode.E) || UnityEngine.Input.GetKeyDown(KeyCode.JoystickButton0)) {
                ObjectsReference.Instance.slotSwitch.ValidateBuildable();
            }
        }

        private void TranslateZBananaGunTarget() {
            scrollSlotsValue = UnityEngine.Input.mouseScrollDelta;
            var scrollValue = scrollSlotsValue.y;

            if (UnityEngine.Input.GetKeyDown(KeyCode.JoystickButton3) || scrollValue > 0) {
                var bananaGunLocalPosition = ObjectsReference.Instance.slotSwitch.plateformPlacementTransform.localPosition;
                if (bananaGunLocalPosition.z < 25) {
                    bananaGunLocalPosition.z += 1f;
                    ObjectsReference.Instance.slotSwitch.plateformPlacementTransform.localPosition = bananaGunLocalPosition;
                }
            }
            
            if (UnityEngine.Input.GetKeyDown(KeyCode.JoystickButton2) || scrollValue < 0) {
                var bananaGunLocalPosition = ObjectsReference.Instance.slotSwitch.plateformPlacementTransform.localPosition;
                if (bananaGunLocalPosition.z > 4) {
                    bananaGunLocalPosition.z -= 1f;
                    ObjectsReference.Instance.slotSwitch.plateformPlacementTransform.localPosition = bananaGunLocalPosition;
                }
            }
        }

        private void Rotate() {
            if (UnityEngine.Input.GetAxis("DpadHorizontal") < 0) {
                leftRotateIncrementer += Time.deltaTime;
                if (leftRotateIncrementer >= _rotationSpeed) {
                    leftRotateIncrementer = 0;
                    ObjectsReference.Instance.slotSwitch.RotateGhost(rotationAxisToVector3Direction[RotationAxis.Y]);
                }
                return;
            }
            if (UnityEngine.Input.GetAxis("DpadHorizontal") > 0) {
                rightRotateIncrementer += Time.deltaTime;
                if (rightRotateIncrementer >= _rotationSpeed) {
                    rightRotateIncrementer = 0;
                    ObjectsReference.Instance.slotSwitch.RotateGhost(-rotationAxisToVector3Direction[RotationAxis.Y]);
                }
            }

            if (UnityEngine.Input.GetKeyDown(KeyCode.LeftArrow)) {
                ObjectsReference.Instance.slotSwitch.RotateGhost(-rotationAxisToVector3Direction[RotationAxis.Y]);
                return;
            }

            if (UnityEngine.Input.GetKeyDown(KeyCode.RightArrow)) {
                ObjectsReference.Instance.slotSwitch.RotateGhost(rotationAxisToVector3Direction[RotationAxis.Y]);
            }
            
            if (ObjectsReference.Instance.bananaMan.activeBuildableType == BuildableType.PLATEFORM) {
                if (UnityEngine.Input.GetAxis("DpadVertical") < 0) {
                    topRotateIncrementer += Time.deltaTime;
                    if (topRotateIncrementer >= _rotationSpeed) {
                        topRotateIncrementer = 0;
                        ObjectsReference.Instance.slotSwitch.RotateGhost(rotationAxisToVector3Direction[RotationAxis.Z]);
                    }
                    return;
                }
            
                if (UnityEngine.Input.GetAxis("DpadVertical") > 0) {
                    downRotateIncrementer += Time.deltaTime;
                    if (downRotateIncrementer >= _rotationSpeed) {
                        downRotateIncrementer = 0;
                        ObjectsReference.Instance.slotSwitch.RotateGhost(-rotationAxisToVector3Direction[RotationAxis.Z]);
                    }
                    return;
                }
                
                if (UnityEngine.Input.GetKeyDown(KeyCode.UpArrow)) {
                    ObjectsReference.Instance.slotSwitch.RotateGhost(rotationAxisToVector3Direction[RotationAxis.Z]);
                    return;
                }

                if (UnityEngine.Input.GetKeyDown(KeyCode.DownArrow)) {
                    ObjectsReference.Instance.slotSwitch.RotateGhost(-rotationAxisToVector3Direction[RotationAxis.Z]);
                }
            }
        }
    }
}
