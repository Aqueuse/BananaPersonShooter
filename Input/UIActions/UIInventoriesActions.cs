using Cameras;
using Data;
using Enums;
using UI.InGame.Inventory;
using UnityEngine;

namespace Input.UIActions {
    public class UIInventoriesActions : MonoBehaviour {
        private bool _scrolledUp;
        private bool _scrolledDown;
        public Vector2 scrollSlotsValue;

        private bool _scrolledLeft;
        private bool _scrolledRight;
        
        private float _counter;
        
        private const float _upDownSpeed = 0.2f;
        
        public bool _leftTriggerActivated;

        private float leftRotateIncrementer;
        private float rightRotateIncrementer;
        private float topRotateIncrementer;
        private float downRotateIncrementer;
        
        private MainCamera mainCamera;
        
        public CameraOneAxisRotation activatedOneAxisRotationCamera;

        private ItemScriptableObject selectedSlotScriptableObject; 
        
        private void Start() {
            mainCamera = ObjectsReference.Instance.mainCamera;
            activatedOneAxisRotationCamera = ObjectsReference.Instance.topDownCamera;
        }

        private void Update() {
            Hide_Interface();
            
            Switch_To_Right();
            Switch_To_Left();
            
            Switch_To_BananasInventory();
            Switch_To_Raw_MaterialsInventory();
            Switch_To_IngredientsInventory();
            Switch_To_BlueprintsInventory();

            SwitchCameras();

            Scroll_Slots();
            AssignToSlots();
            AssignToSelectedSlot();

            QuickBuild();

            if (mainCamera.cameraMode == CAMERA_MODE.PLAYER_VIEW) return;
            MoveCamera();
            RotateCamera();
            UpCamera();
            DownCamera();
            AccelerateCamera();
        }

        private void Hide_Interface() {
            if (UnityEngine.Input.GetKeyDown(KeyCode.Tab) || UnityEngine.Input.GetKeyDown(KeyCode.JoystickButton6)) {
                ObjectsReference.Instance.uiManager.Hide_Interface();
                ObjectsReference.Instance.gestionMode.CloseGestionMode();
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
            if (UnityEngine.Input.GetKeyDown(KeyCode.F1)) {
                ObjectsReference.Instance.uInventoriesManager.Switch_To_Tab(ItemCategory.BANANA);
            }
        }
        
        private void Switch_To_Raw_MaterialsInventory() {
            if (UnityEngine.Input.GetKeyDown(KeyCode.F2)) {
                ObjectsReference.Instance.uInventoriesManager.Switch_To_Tab(ItemCategory.RAW_MATERIAL);
            }
        }

        private void Switch_To_IngredientsInventory() {
            if (UnityEngine.Input.GetKeyDown(KeyCode.F3)) {
                ObjectsReference.Instance.uInventoriesManager.Switch_To_Tab(ItemCategory.INGREDIENT);
            }
        }

        private void Switch_To_BlueprintsInventory() {
            if (UnityEngine.Input.GetKeyDown(KeyCode.F4)) {
                ObjectsReference.Instance.uInventoriesManager.Switch_To_Tab(ItemCategory.BUILDABLE);
            }
        }
        
        private void Scroll_Slots() {
            if (UnityEngine.Input.GetKey(KeyCode.LeftControl)) return;
            
            scrollSlotsValue = UnityEngine.Input.mouseScrollDelta;

            var scrollValue = scrollSlotsValue.y;
            if (scrollValue < 0) ObjectsReference.Instance.uiQuickSlotsManager.Select_Lefter_Slot();
            if (scrollValue > 0) ObjectsReference.Instance.uiQuickSlotsManager.Select_Righter_Slot();
        }
        
        private void AssignToSlots() {
            if (UnityEngine.Input.GetKeyDown(KeyCode.Alpha1)) {
                PutInQuickSlot(0);
            }
            if (UnityEngine.Input.GetKeyDown(KeyCode.Alpha2)) {
                PutInQuickSlot(1);
            }
            if (UnityEngine.Input.GetKeyDown(KeyCode.Alpha3)) {
                PutInQuickSlot(2);
            }
            if (UnityEngine.Input.GetKeyDown(KeyCode.Alpha4)) {
                PutInQuickSlot(3);
            }
        }

        private void AssignToSelectedSlot() {
            if (UnityEngine.Input.GetKeyDown(KeyCode.E)) {
                PutInQuickSlot(ObjectsReference.Instance.uiQuickSlotsManager.selectedSlotIndex);
            }
        }
        
        private void PutInQuickSlot(int quickslotNumber) {
            if (ObjectsReference.Instance.uInventoriesManager.GetLastSelectedItem() != null) {
                selectedSlotScriptableObject = ObjectsReference.Instance.uInventoriesManager.GetLastSelectedItem().GetComponent<UInventorySlot>().itemScriptableObject; 
                ObjectsReference.Instance.uiQuickSlotsManager.uiQuickSlotsScripts[quickslotNumber].SetSlot(selectedSlotScriptableObject); 
            }
        }

        private void QuickBuild() {
            if (UnityEngine.Input.GetKeyDown(KeyCode.JoystickButton0)) {
                ObjectsReference.Instance.build.QuickBuild();
            }
        }
        
        /// /// CAMERAS
        private void SwitchCameras() {
            if (UnityEngine.Input.GetKeyDown(KeyCode.F5)) {
                mainCamera.Switch_To_Camera_View(CAMERA_MODE.PLAYER_VIEW);
            }
            if (UnityEngine.Input.GetKeyDown(KeyCode.F6)) {
                mainCamera.Switch_To_Camera_View(CAMERA_MODE.TOP_DOWN_VIEW);
            }
        }

        private void MoveCamera() {
            activatedOneAxisRotationCamera.Move(
                UnityEngine.Input.GetAxis("Horizontal"),
                UnityEngine.Input.GetAxis("Vertical"));
        }

        private void UpCamera() {
            if (UnityEngine.Input.GetKeyDown(KeyCode.Space)) activatedOneAxisRotationCamera.cameraY += 1f;
            if (UnityEngine.Input.GetKeyUp(KeyCode.Space)) activatedOneAxisRotationCamera.cameraY = 0f;
            
            if (UnityEngine.Input.GetAxis("DpadVertical") > 0) {
                activatedOneAxisRotationCamera.cameraY += 1f * _upDownSpeed;
            }
            
            if (UnityEngine.Input.GetAxis("DpadVertical") == 0) {
                activatedOneAxisRotationCamera.cameraY = 0f;
            }
        }
        
        private void DownCamera() {
            if (UnityEngine.Input.GetKeyDown(KeyCode.LeftControl)) activatedOneAxisRotationCamera.cameraY -= 1f;
            if (UnityEngine.Input.GetKeyUp(KeyCode.LeftControl)) activatedOneAxisRotationCamera.cameraY = 0f;
            
            if (UnityEngine.Input.GetAxis("DpadVertical") < 0) {
                activatedOneAxisRotationCamera.cameraY -= 1f * _upDownSpeed;
            }
        }

        private void AccelerateCamera() {
            if (UnityEngine.Input.GetKeyDown(KeyCode.LeftShift)) activatedOneAxisRotationCamera.cameraSpeed += 10f;
            if (UnityEngine.Input.GetKeyUp(KeyCode.LeftShift)) activatedOneAxisRotationCamera.cameraSpeed = 8f;
            
            if (UnityEngine.Input.GetAxis("LeftTrigger") != 0 && !_leftTriggerActivated) {
                _leftTriggerActivated = true;
                activatedOneAxisRotationCamera.cameraSpeed += 10f;
            }

            if (UnityEngine.Input.GetAxis("LeftTrigger") == 0 && _leftTriggerActivated) {
                activatedOneAxisRotationCamera.cameraSpeed = 8f;
                _leftTriggerActivated = false;
            }
        }

        private void RotateCamera() {
            if (UnityEngine.Input.GetKey(KeyCode.Q)) {
                activatedOneAxisRotationCamera.Rotate(-1);
            }
            else if (UnityEngine.Input.GetKey(KeyCode.E)) {
                activatedOneAxisRotationCamera.Rotate(1);
            }

            if (ObjectsReference.Instance.inputManager.schemaContext != SchemaContext.GAMEPAD) return;
            
            if (UnityEngine.Input.GetAxis("Mouse X") < 0) {
                activatedOneAxisRotationCamera.Rotate(-1);
            }

            if (UnityEngine.Input.GetAxis("Mouse X") > 0) {
                activatedOneAxisRotationCamera.Rotate(1);
            }
        }
    }
}
