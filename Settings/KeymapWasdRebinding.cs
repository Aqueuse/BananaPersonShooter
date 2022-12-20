using Player;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Settings {
    public class KeymapWasdRebinding : MonoBehaviour {
        public TextMeshProUGUI buttonBindingText;
        [SerializeField] private string actionName;
        [SerializeField] private string actionNameComposite;

        [SerializeField] private GameObject rebindButton;
        [SerializeField] private GameObject listeningForInputButton;

        private PlayerInput playerInput;
        private InputAction focusedInputAction;
        private InputActionAsset focusedInputActionAsset;
        private InputActionRebindingExtensions.RebindingOperation rebindOperation;
    
        
        private void Start() {
            playerInput = BananaMan.Instance.GetComponent<PlayerInput>();
            focusedInputAction = playerInput.actions.FindAction(actionName);
            
            if (PlayerPrefs.HasKey("keymap_binding_" + actionNameComposite)) {
                buttonBindingText.text = PlayerPrefs.GetString("keymap_binding_" + actionNameComposite);   
            }
        }

        public void ButtonPressedWasdStartRebind() {
            StartRebindWasdProcess();
        }

        private void StartRebindWasdProcess() {
            focusedInputAction.Disable();

            rebindButton.SetActive(false);
            listeningForInputButton.SetActive(true);

            var wasd = focusedInputAction.ChangeBinding("WASD");
            var part = wasd.NextPartBinding(actionNameComposite);

            rebindOperation = focusedInputAction.PerformInteractiveRebinding()
                .WithTargetBinding(part.bindingIndex)
                .WithControlsExcluding("<Mouse>/position")
                .WithControlsExcluding("<Mouse>/delta")
                .WithControlsExcluding("<Gamepad>/Start")
                .WithControlsExcluding("<Keyboard>/escape")
                .OnMatchWaitForAnother(0.1f)
                .OnComplete(operation => RebindCompleted(focusedInputAction.GetBindingDisplayString(part.bindingIndex)));

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
            PlayerPrefs.SetString("keymap_binding_"+actionNameComposite, bindingKey);
           
            UpdateBindingDisplayUI(bindingKey);
        }


        private void UpdateBindingDisplayUI(string bindingKey) {
            buttonBindingText.SetText(bindingKey);
        }
   
        public void ResetBinding() {
            focusedInputAction.RemoveAllBindingOverrides();

            var wasd = focusedInputAction.ChangeBinding("WASD");
            var part = wasd.NextPartBinding(actionNameComposite);
            var key = focusedInputAction.GetBindingDisplayString(part.bindingIndex);
            buttonBindingText.SetText(key);
            PlayerPrefs.SetString("keymap_binding_"+actionNameComposite, key);
        }
    }
}