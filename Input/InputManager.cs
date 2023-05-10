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
        public ShootGameActions shootGameActions;
        public BuildGameActions buildGameActions;

        public BananasDryerAction bananasDryerAction;
        
        void Start() {
            _gameActions = GetComponent<GameActions>();
            shootGameActions = GetComponent<ShootGameActions>();
            buildGameActions = GetComponent<BuildGameActions>();
            
            uiSchemaSwitcher = GetComponent<UISchemaSwitcher>();
        }

        public void SwitchContext(InputContext newInputContext) {
            if (newInputContext == InputContext.UI) {
                _gameActions.enabled = false;
                shootGameActions.enabled = false;
                buildGameActions.enabled = false;
                
                uiSchemaSwitcher.SwitchUISchema(uiSchemaContext);
                
                eventSystem.enabled = true;
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }
            else {
                _gameActions.enabled = true;
                uiSchemaSwitcher.DisableAllUISchemas();
                
                eventSystem.enabled = false;
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Confined;
                
                if (ObjectsReference.Instance.bananaMan.activeItemCategory == ItemCategory.BANANA) {
                    buildGameActions.enabled = false;
                    shootGameActions.enabled = true;
                }

                if (ObjectsReference.Instance.bananaMan.activeItemCategory == ItemCategory.BUILDABLE) {
                    shootGameActions.enabled = false;
                    buildGameActions.enabled = true;
                }

                // else {
                //     shootGameActions.enabled = false;
                //     buildGameActions.enabled = false;
                // }
            }
        }
    }
}
