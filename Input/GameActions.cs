using Building;
using Enums;
using Items;
using Player;
using UI;
using UI.InGame;
using UnityEngine;

namespace Input {
    public class GameActions : MonoSingleton<GameActions> {
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

            _playerController = BananaMan.Instance.GetComponent<PlayerController>();

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
            Show_Inventory();
            PauseGame();

            Scroll_Slots();
            SwitchToUpperSlot();
            SwitchToLowerSlot();
            
            SwitchToSlotIndex0();
            SwitchToSlotIndex1();
            SwitchToSlotIndex2();
            SwitchToSlotIndex3();
            
            Aspire();
            Shoot();
        }
        
        private void Move() {
            // if (!BananaMan.Instance.isRagdoll) {
                move.x = UnityEngine.Input.GetAxis("Horizontal");
                move.y = UnityEngine.Input.GetAxis("Vertical");
            // }
            // else {
            //     move.x = 0;
            //     move.y = 0;
            // }
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
            if (UnityEngine.Input.GetKeyDown(KeyCode.R) || UnityEngine.Input.GetKeyDown(KeyCode.JoystickButton3)) {
                BananaMan.Instance.GainHealth();
            }
        }

        private void Interact() {
            if (UnityEngine.Input.GetKeyDown(KeyCode.E) || UnityEngine.Input.GetKeyDown(KeyCode.JoystickButton2)) {
                ItemsManager.Instance.Validate();
            }
        }

        private void Show_Inventory() {
            if (UnityEngine.Input.GetKeyDown(KeyCode.I) || UnityEngine.Input.GetKeyDown(KeyCode.JoystickButton6)) {
                UIManager.Instance.Show_Hide_inventory();
            }
        }

        private void PauseGame() {
            if (UnityEngine.Input.GetKeyDown(KeyCode.Escape) || UnityEngine.Input.GetKeyDown(KeyCode.JoystickButton7)) {
                UIManager.Instance.Show_game_menu();
            }
        }

        private void SwitchToUpperSlot() {
            if (UnityEngine.Input.GetAxis("DpadVertical") > 0 && !_scrolledUp) {
                UISlotsManager.Instance.Select_Upper_Slot();
                _scrolledUp = true;
            }

            if (UnityEngine.Input.GetAxis("DpadVertical") == 0) {
                _scrolledUp = false;
                _scrolledDown = false;
            }
        }
    
        private void SwitchToLowerSlot() {
            if (UnityEngine.Input.GetAxis("DpadVertical") < 0 && !_scrolledDown) {
                UISlotsManager.Instance.Select_Lower_Slot();
                _scrolledDown = true;
            }
        }
    
        public void Scroll_Slots() {
            scrollSlotsValue = UnityEngine.Input.mouseScrollDelta;
            
            float scrollValue = scrollSlotsValue.y;
            if (scrollValue < 0) UISlotsManager.Instance.Select_Upper_Slot();
            if (scrollValue > 0) UISlotsManager.Instance.Select_Lower_Slot();
        }

        private void Aspire() {
            if (UnityEngine.Input.GetKeyDown(KeyCode.Mouse1)) {
                MoverGet.Instance.StartToGet();
            }

            if (UnityEngine.Input.GetKeyUp(KeyCode.Mouse1)) {
                MoverGet.Instance.CancelGet();
            }

            if (UnityEngine.Input.GetAxis("LeftTrigger") <= -0.1 && !_leftTriggerActivated) {
                _leftTriggerActivated = true;
                MoverGet.Instance.StartToGet();
            }

            if (UnityEngine.Input.GetAxis("LeftTrigger") >= 0 && _leftTriggerActivated)  {
                _leftTriggerActivated = false;
                MoverGet.Instance.CancelGet();
            }
        }

        private void SwitchToSlotIndex0() {
            if (UnityEngine.Input.GetKeyDown(KeyCode.Alpha1)) {
                UISlotsManager.Instance.Switch_to_Slot_Index(0);
            }
        }

        private void SwitchToSlotIndex1() {
            if (UnityEngine.Input.GetKeyDown(KeyCode.Alpha2)) {
                UISlotsManager.Instance.Switch_to_Slot_Index(1);
            }
        }
        
        private void SwitchToSlotIndex2() {
            if (UnityEngine.Input.GetKeyDown(KeyCode.Alpha3)) {
                UISlotsManager.Instance.Switch_to_Slot_Index(2);
            }
        }
        
        private void SwitchToSlotIndex3() {
            if (UnityEngine.Input.GetKeyDown(KeyCode.Alpha4)) {
                UISlotsManager.Instance.Switch_to_Slot_Index(3);
            }
        }
        
        private void Shoot() {
            if (BananaMan.Instance.activeItemThrowableCategory == ItemThrowableCategory.BANANA) {
                if (UnityEngine.Input.GetKeyDown(KeyCode.Mouse0)) {
                    leftClickActivated = true;
                    MoverPut.Instance.LoadingGun();
                }

                if (UnityEngine.Input.GetKeyUp(KeyCode.Mouse0)) {
                    Mover.Instance.CancelMover();
                }

                if (UnityEngine.Input.GetAxis("RightTrigger") != 0 && !rightTriggerActivated) {
                    rightTriggerActivated = true;
                    MoverPut.Instance.LoadingGun();
                }

                if (UnityEngine.Input.GetAxis("RightTrigger") == 0 && rightTriggerActivated)  {
                    Mover.Instance.CancelMover();
                    rightTriggerActivated = false;
                }
            }

            if (BananaMan.Instance.activeItemThrowableCategory == ItemThrowableCategory.PLATEFORM) {
                if (UnityEngine.Input.GetKeyDown(KeyCode.Mouse0)) {
                    leftClickActivated = true;
                    SlotSwitch.Instance.ValidatePlateform();
                }
                
                if (UnityEngine.Input.GetAxis("RightTrigger") != 0 && !rightTriggerActivated) {
                    rightTriggerActivated = true;
                    SlotSwitch.Instance.ValidatePlateform();
                }

                if (UnityEngine.Input.GetAxis("RightTrigger") == 0 && rightTriggerActivated) {
                    rightTriggerActivated = false;
                }
            }
        }
    }
}
