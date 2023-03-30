using Enums;
using UnityEngine;

namespace Input.UIActions {
    public class UISchemaSwitcher : MonoSingleton<UISchemaSwitcher> {
        [SerializeField] private GenericDictionary<UISchemaSwitchType, UISchemaSwitch> uISchemaSwitches;
        
        public void SwitchUISchema(UISchemaSwitchType uiSchemaSwitchType) {
            InputManager.Instance.uiSchemaContext = uiSchemaSwitchType;
            
            DisableAllUISchemas();
            
            uISchemaSwitches[uiSchemaSwitchType].EnableSchema();
        }
        
        public void DisableAllUISchemas() {
            foreach (var uISchemaSwitch in uISchemaSwitches) {
                uISchemaSwitch.Value.DisableSchema();
            }
        }

        public UISchemaSwitch GetUISchema(UISchemaSwitchType uiSchemaSwitchType) {
            return uISchemaSwitches[uiSchemaSwitchType];
        }
    }
}
