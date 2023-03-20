using Enums;
using Input.UIActions;

namespace Input {
    public class InputManager : MonoSingleton<InputManager> {
        private GameActions _gameActions;
        private UISchemaSwitcher _uiSchemaSwitcher;

        public UISchemaSwitchType uiSchemaContext;

        void Start() {
            _gameActions = GetComponent<GameActions>();
            _uiSchemaSwitcher = GetComponent<UISchemaSwitcher>();
        }

        public void SwitchContext(InputContext newInputContext) {
            if (newInputContext == InputContext.UI) {
                _gameActions.enabled = false;
                
                _uiSchemaSwitcher.SwitchUISchema(uiSchemaContext);
            }
            else {
                _gameActions.enabled = true;
                _uiSchemaSwitcher.DisableAllUISchemas();
            }
        }
    }
}
