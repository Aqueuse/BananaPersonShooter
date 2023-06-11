using Enums;
using Input.interactables;
using Input.UIActions;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Input {
    public class InputManager : MonoBehaviour {
        [SerializeField] private EventSystem eventSystem;
        public UISchemaSwitcher uiSchemaSwitcher;
        public UISchemaSwitchType uiSchemaContext;
        
        private GameActions _gameActions;

        public BananasDryerAction bananasDryerAction;
        
        private void Start() {
            _gameActions = GetComponent<GameActions>();
            
            uiSchemaSwitcher = GetComponent<UISchemaSwitcher>();
        }

        public void SwitchContext(InputContext newInputContext) {
            if (newInputContext == InputContext.UI) {
                _gameActions.enabled = false;
                
                uiSchemaSwitcher.SwitchUISchema(uiSchemaContext);

                eventSystem.enabled = true;
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;

                ObjectsReference.Instance.playerController.canMove = false;
            }
            else {
                _gameActions.enabled = true;
                uiSchemaSwitcher.DisableAllUISchemas();
                
                eventSystem.enabled = false;
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Confined;
                
                ObjectsReference.Instance.playerController.canMove = true;
            }
        }
    }
}
