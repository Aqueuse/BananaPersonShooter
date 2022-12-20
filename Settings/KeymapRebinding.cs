using Player;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Settings {
    public class KeymapRebinding : MonoBehaviour {
        public TextMeshProUGUI buttonBindingText;
        [SerializeField] private string actionName;

        [SerializeField] private string initialBinding;
    
        [SerializeField] private GameObject rebindButton;
        [SerializeField] private GameObject listeningForInputButton;

        private PlayerInput playerInput;
        private InputAction focusedInputAction;
        private InputActionAsset focusedInputActionAsset;
        private InputActionRebindingExtensions.RebindingOperation rebindOperation;
    
        private void Start() {
            playerInput = BananaMan.Instance.GetComponent<PlayerInput>();
            focusedInputAction = playerInput.actions.FindAction(actionName);

            if (PlayerPrefs.HasKey("keymap_binding_" + actionName)) {
                buttonBindingText.text = PlayerPrefs.GetString("keymap_binding_" + actionName);   
            }
        }
    
        public void ButtonPressedStartRebind() {
            StartRebindProcess();
        }

        private void StartRebindProcess() {
            focusedInputAction.Disable();
        
            rebindButton.SetActive(false);
            listeningForInputButton.SetActive(true);
            
            rebindOperation = focusedInputAction.PerformInteractiveRebinding()
                .WithControlsExcluding("<Mouse>/position")
                .WithControlsExcluding("<Mouse>/delta")
                .WithControlsExcluding("<Gamepad>/Start")
                .WithControlsExcluding("<Keyboard>/escape")
                .OnMatchWaitForAnother(0.1f)
                .OnComplete(operation => RebindCompleted(focusedInputAction.GetBindingDisplayString(focusedInputAction.GetBindingIndex())));

            rebindOperation.Start();
        }

        private void RebindCompleted(string bindingKey) {
            rebindOperation.Dispose();
            rebindOperation = null;

            rebindButton.SetActive(true);
            listeningForInputButton.SetActive(false);

            focusedInputAction.Enable();

            var rebinds = playerInput.actions.SaveBindingOverridesAsJson();
            PlayerPrefs.SetString("keymap_binding", rebinds);
            PlayerPrefs.SetString("keymap_binding_"+actionName, bindingKey);

            UpdateBindingDisplayUI(bindingKey);
        }

        private void UpdateBindingDisplayUI(string bindingKey) {
            buttonBindingText.SetText(bindingKey);
        }

        public void ResetBinding() {
            focusedInputAction.RemoveAllBindingOverrides();

            if (GameSettings.Instance.languageIndexSelected == 0 && initialBinding.Equals("A")) {
                buttonBindingText.SetText("Q");
            }

            else {
                buttonBindingText.SetText(initialBinding);
            }
            
            PlayerPrefs.SetString("keymap_binding_"+actionName, initialBinding);
        }
    }
}