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
        
        private bool _leftTriggerActivated;
        public bool rightTriggerActivated;

        public bool leftClickActivated;

        private void Start() {
            move = new Vector2();

            scrollSlotsValue = new Vector2();

            _playerController = ObjectsReference.Instance.bananaMan.GetComponent<PlayerController>();

            _leftTriggerActivated = false;
            rightTriggerActivated = false;
        }

        private void Update() {
            Move();
            Jump();
            Run();
            Roll();
            Eat();
            
            Interact();
            PauseGame();
            ShowTutorial();

            ZoomDezoomCamera();

            if (!ObjectsReference.Instance.gameData.bananaManSavedData.playerAdvancements.Contains(AdvancementState.GET_BANANAGUN)) return;
            
            Show_Inventory();

            Scroll_Slots();
            SwitchToUpperSlot();
            SwitchToLowerSlot();
            
            SwitchToSlotIndex0();
            SwitchToSlotIndex1();
            SwitchToSlotIndex2();
            SwitchToSlotIndex3();
            
            Aspire();
        }
        
        private void Move() {
            move.x = UnityEngine.Input.GetAxis("Horizontal");
            move.y = UnityEngine.Input.GetAxis("Vertical");
        }

        private void Jump() {
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

        private void Eat() {
            if (ObjectsReference.Instance.bananaMan.activeItemCategory == ItemCategory.BANANA && UnityEngine.Input.GetKeyDown(KeyCode.R) || UnityEngine.Input.GetKeyDown(KeyCode.JoystickButton3)) {
                ObjectsReference.Instance.bananaMan.GainHealth();
            }
        }

        private void Interact() {
            if (UnityEngine.Input.GetKeyDown(KeyCode.E) || UnityEngine.Input.GetKeyDown(KeyCode.JoystickButton2)) {
                ObjectsReference.Instance.itemsManager.Validate();
            }
        }

        private void Show_Inventory() {
            if (UnityEngine.Input.GetKeyDown(KeyCode.I) || UnityEngine.Input. GetAxis("DpadHorizontal") < 0) {
                ObjectsReference.Instance.uiManager.Show_Hide_interface();
            }
        }

        private void PauseGame() {
            if (UnityEngine.Input.GetKeyDown(KeyCode.Escape) || UnityEngine.Input.GetKeyDown(KeyCode.JoystickButton7)) {
                ObjectsReference.Instance.inputManager.uiSchemaContext = UISchemaSwitchType.GAME_MENU;
                ObjectsReference.Instance.gameManager.PauseGame(true);
                ObjectsReference.Instance.uiManager.Show_game_menu();
            }
        }

        private void ShowTutorial() {
            if (UnityEngine.Input.GetKeyDown(KeyCode.H) || UnityEngine.Input.GetKeyDown(KeyCode.JoystickButton6)) {
                ObjectsReference.Instance.inputManager.uiSchemaContext = UISchemaSwitchType.TUTORIAL;
                ObjectsReference.Instance.gameManager.PauseGame(true);
                ObjectsReference.Instance.tutorialsManager.Show_Help();
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
            
            float scrollValue = scrollSlotsValue.y;
            if (scrollValue < 0) ObjectsReference.Instance.uiSlotsManager.Select_Upper_Slot();
            if (scrollValue > 0) ObjectsReference.Instance.uiSlotsManager.Select_Lower_Slot();
        }

        private void Aspire() {
            if (UnityEngine.Input.GetKeyDown(KeyCode.Mouse1)) {
                ObjectsReference.Instance.bananaGunGet.StartToGet();
            }

            if (UnityEngine.Input.GetKeyUp(KeyCode.Mouse1)) {
                ObjectsReference.Instance.bananaGunGet.CancelGet();
            }

            if (UnityEngine.Input.GetAxis("LeftTrigger") <= -0.1 && !_leftTriggerActivated) {
                _leftTriggerActivated = true;
                ObjectsReference.Instance.bananaGunGet.StartToGet();
            }

            if (UnityEngine.Input.GetAxis("LeftTrigger") >= 0 && _leftTriggerActivated)  {
                _leftTriggerActivated = false;
                ObjectsReference.Instance.bananaGunGet.CancelGet();
            }
        }

        private void SwitchToSlotIndex0() {
            if (UnityEngine.Input.GetKeyDown(KeyCode.Alpha1)) {
                ObjectsReference.Instance.uiSlotsManager.Switch_to_Slot_Index(0);
            }
        }

        private void SwitchToSlotIndex1() {
            if (UnityEngine.Input.GetKeyDown(KeyCode.Alpha2)) {
                ObjectsReference.Instance.uiSlotsManager.Switch_to_Slot_Index(1);
            }
        }
        
        private void SwitchToSlotIndex2() {
            if (UnityEngine.Input.GetKeyDown(KeyCode.Alpha3)) {
                ObjectsReference.Instance.uiSlotsManager.Switch_to_Slot_Index(2);
            }
        }
        
        private void SwitchToSlotIndex3() {
            if (UnityEngine.Input.GetKeyDown(KeyCode.Alpha4)) {
                ObjectsReference.Instance.uiSlotsManager.Switch_to_Slot_Index(3);
            }
        }
        
        private void ZoomDezoomCamera() {
            scrollSlotsValue = UnityEngine.Input.mouseScrollDelta;

            float scrollValue = scrollSlotsValue.y;

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

        private void OnDisable() {
            ObjectsReference.Instance.bananaMan.GetComponent<PlayerController>().ResetPlayer();
        }
    }
}
