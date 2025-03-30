using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Settings {
    internal enum ControlScheme {
        KEYBOARD = 0,
        GAMEPAD = 1
    }
    public class InputRebind : MonoBehaviour {
        [SerializeField] private InputActionReference inputActionReference;

        public Transform rebindHelper;
        [SerializeField] private TextMeshProUGUI keyText;
        
        private readonly string[] ControlsExcluding = { "Mouse", "<Keyboard>/enter", "<Keyboard>/escape" };
        private readonly string[] CancelingThrough = { "Mouse", "<Keyboard>/enter", "<Keyboard>/escape" };

        [SerializeField] private ControlScheme controlScheme;
        public bool isComposite;

        // the composite part of the binding (consult the inputActionAsset for getting the correct indexs)
        [SerializeField] private int compositeIndex;

        private Button rebindButton;

        public InputActionRebindingExtensions.RebindingOperation current_rebind;

        // ui presentation :
        // a list for keyboards action to rebind
        // a list for gamepads action to rebind
        // each item in the list contain an instance of this class
        
        private void Start() {
            rebindButton = GetComponent<Button>();
        }

        public void Init() {
            keyText.text = GetLocalizedDisplayName();
        }

        public void Rebind() {
            ObjectsReference.Instance.uiSettings.CancelAllRebinds();
            
            rebindButton.enabled = false;
            rebindHelper.gameObject.SetActive(true);
            keyText.enabled = false;

            inputActionReference.action.Disable();
            
            // typically for this code to work, you want to set the
            // inputActionAsset with one entry for keyboard
            // and one for gamepad and ALWAYS follow the same order
            
            // same if you want to work with a composite and a non composite
            // for example : movement is a composite for keybord (WASD) and
            // a non composite for gamepad (leftstick)

            if (isComposite) {
                current_rebind = inputActionReference.action.PerformInteractiveRebinding();

                var binding = inputActionReference.action.bindings[compositeIndex];
                if(binding.isPartOfComposite) current_rebind.WithTargetBinding(compositeIndex);
            }

            else {
                current_rebind = inputActionReference.action.PerformInteractiveRebinding((int)controlScheme);
            }
            
            if (ControlsExcluding != null) {
                foreach (var rebind in ControlsExcluding)
                    current_rebind.WithControlsExcluding(rebind);
            }

            // if you setup for gamepad, you don't want to let players use gamepad input
            // in the keyboard panel
            current_rebind.WithControlsExcluding(controlScheme == ControlScheme.GAMEPAD ? "<Keyboard>" : "<Gamepad>");

            if (CancelingThrough != null) {
                foreach (var rebind in CancelingThrough)
                    current_rebind.WithCancelingThrough(rebind);
            }

            current_rebind.OnCancel(_ => {
                inputActionReference.action.Enable();
                current_rebind?.Cancel();
                
                rebindHelper.gameObject.SetActive(false);
                rebindButton.enabled = true;
                keyText.enabled = true;
            });

            current_rebind.OnComplete(rebindingOperation => {
                inputActionReference.action.Enable();

                rebindHelper.gameObject.SetActive(false);
                rebindButton.enabled = true;
                keyText.enabled = true;
                keyText.text = GetLocalizedDisplayName();
                
                ObjectsReference.Instance.gameSettings.SetKeysBinding();
                // equals to :
                // prefs.SetString("inputBindings", inputActionAsset.SaveBindingOverridesAsJson());
                // prefs.Save();
                
                current_rebind?.Dispose();
                current_rebind = null;
            });
            
            current_rebind.Start();
        }

        private string GetLocalizedDisplayName() {
            if (controlScheme == ControlScheme.GAMEPAD) {
                return inputActionReference.ToInputAction().GetBindingDisplayString(group:"Gamepad");
            }

            if (controlScheme == ControlScheme.KEYBOARD) {
                if (isComposite) {
                    var path = inputActionReference.ToInputAction().GetBindingDisplayString(compositeIndex);

                    return path;
                }
                
                Debug.Log(inputActionReference.ToInputAction().GetBindingDisplayString(group:"Keyboard"));

                return inputActionReference.ToInputAction().GetBindingDisplayString(group:"Keyboard");
            }

            return "";
        }
    }
}
