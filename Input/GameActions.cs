using Building;
using Items;
using Player;
using UI;
using UI.InGame;
using UnityEngine;

namespace Input {
    public class GameActions : MonoSingleton<GameActions> {
        public Vector2 look;
        public Vector2 gamepadRightStick;
        
        public Vector2 move;

        public Vector2 scrollSlotsValue;
        
        private PlayerController _playerController;

        private bool _scrolledUp;
        private bool _scrolledDown;
        
        private bool _leftTriggerActivated;
        public bool rightTriggerActivated;

        public bool leftClickActivated;

        public bool isCameraInverted;

        private void Start() {
            look = new Vector2();
            move = new Vector2();

            scrollSlotsValue = new Vector2();

            _playerController = BananaMan.Instance.GetComponent<PlayerController>();

            isCameraInverted = false;
        }

        private void Update() {
            Look();
            Move();
            Jump();
            Run();
            Roll();
            Eat();
            
            Interact();
            FocusCamera();
            Show_Inventory();
            PauseGame();

            Scroll_Slots();
            SwitchToUpperSlot();
            SwitchToLowerSlot();
            
            Aspire();
            Shoot();
        }

        private void Look() {
            look.x = UnityEngine.Input.GetAxis("Mouse X") * 5;

            if (isCameraInverted) {
                look.y = -UnityEngine.Input.GetAxis("Mouse Y") * 5;
            }

            else {
                look.y = UnityEngine.Input.GetAxis("Mouse Y") * 5;
            }

            
            gamepadRightStick.x = UnityEngine.Input.GetAxis("Right Stick X") * 200;
            gamepadRightStick.y = UnityEngine.Input.GetAxis("Right Stick Y") * 200;
            
            if (gamepadRightStick != Vector2.zero) look = gamepadRightStick;
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
            if (UnityEngine.Input.GetKeyDown(KeyCode.Space) || UnityEngine.Input.GetKeyDown(KeyCode.Joystick1Button0)) {
                _playerController.PlayerJump();
            }
        }

        private void Run() {
            if (
                UnityEngine.Input.GetKeyDown(KeyCode.LeftShift) || 
                UnityEngine.Input.GetKeyDown(KeyCode.Joystick1Button4) ||
                UnityEngine.Input.GetKeyDown(KeyCode.Joystick1Button8)
                ) {
                _playerController.PlayerSprint();
            }

            if (UnityEngine.Input.GetKeyUp(KeyCode.LeftShift) || UnityEngine.Input.GetKeyUp(KeyCode.Joystick1Button4)) {
                _playerController.PlayerStopSprint();
            }
        }

        private void Roll() {
            if (UnityEngine.Input.GetKeyDown(KeyCode.LeftControl) || UnityEngine.Input.GetKeyDown(KeyCode.Joystick1Button1)) {
                _playerController.PlayerRoll();
            }
        }

        private void Eat() {
            if (UnityEngine.Input.GetKeyDown(KeyCode.R) || UnityEngine.Input.GetKeyDown(KeyCode.Joystick1Button3)) {
                BananaMan.Instance.GainHealth();
            }
        }

        private void Interact() {
            if (UnityEngine.Input.GetKeyDown(KeyCode.E) || UnityEngine.Input.GetKeyDown(KeyCode.Joystick1Button2)) {
                ItemsManager.Instance.Validate();
            }
        }

        private void Show_Inventory() {
            if (UnityEngine.Input.GetKeyDown(KeyCode.I) || UnityEngine.Input.GetKeyDown(KeyCode.JoystickButton6)) {
                UIManager.Instance.Show_Hide_inventory();
            }
        }

        private void FocusCamera() {
            if (UnityEngine.Input.GetKeyDown(KeyCode.F) || UnityEngine.Input.GetKeyDown(KeyCode.JoystickButton9)) {
                if (!_playerController.isFocusCamera) {
                    _playerController.FocusCamera();
                }
                else {
                    _playerController.FreeCamera();
                }
            }
        }

        private void PauseGame() {
            if (UnityEngine.Input.GetKeyDown(KeyCode.Escape) || UnityEngine.Input.GetKeyDown(KeyCode.JoystickButton7)) {
                UIManager.Instance.Show_game_menu();
            }
        }

        private void SwitchToUpperSlot() {
            if (UnityEngine.Input.GetAxis("DpadHorizontal") > 0 && !_scrolledUp) {
                UISlotsManager.Instance.Select_Upper_Slot();
                _scrolledUp = true;
            }

            if (UnityEngine.Input.GetAxis("DpadHorizontal") == 0) {
                _scrolledUp = false;
                _scrolledDown = false;
            }
        }
    
        private void SwitchToLowerSlot() {
            if (UnityEngine.Input.GetAxis("DpadHorizontal") < 0 && !_scrolledDown) {
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

        public void Aspire() {
            if (UnityEngine.Input.GetKeyDown(KeyCode.Mouse1) && BananaMan.Instance.hasMover) {
                MoverGet.Instance.StartToGet();
            }

            if (UnityEngine.Input.GetKeyUp(KeyCode.Mouse1) && BananaMan.Instance.hasMover) {
                MoverGet.Instance.CancelGet();
            }

            if (UnityEngine.Input.GetAxis("LeftTrigger") >= 0.9 && !_leftTriggerActivated && BananaMan.Instance.hasMover) {
                _leftTriggerActivated = true;
                MoverGet.Instance.StartToGet();
            }

            if (UnityEngine.Input.GetAxis("LeftTrigger") <= 0.1 && _leftTriggerActivated && BananaMan.Instance.hasMover)  {
                _leftTriggerActivated = false;
                MoverGet.Instance.CancelGet();
            }
        }

        public void Shoot() {
            if (UnityEngine.Input.GetKeyDown(KeyCode.Mouse0) && BananaMan.Instance.hasMover) {
                leftClickActivated = true;
                MoverPut.Instance.StartToPut();
            }

            if (UnityEngine.Input.GetKeyUp(KeyCode.Mouse0) && BananaMan.Instance.hasMover) {
                leftClickActivated = false;
                MoverPut.Instance.ValidatePut();
            }

            if (UnityEngine.Input.GetAxis("RightTrigger") >= 0.9 && !rightTriggerActivated && BananaMan.Instance.hasMover) {
                rightTriggerActivated = true;
                MoverPut.Instance.StartToPut();
            }

            if (UnityEngine.Input.GetAxis("RightTrigger") <= 0.1 && rightTriggerActivated && BananaMan.Instance.hasMover)  {
                rightTriggerActivated = false;
            }
        }
    }
}
