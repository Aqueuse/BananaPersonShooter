using InGame.Items.ItemsProperties.Buildables;
using UnityEngine;
using UnityEngine.UI;

namespace UI.InGame.Inventory {
    public class UIBlueprintSlot : MonoBehaviour {
        public BuildablePropertiesScriptableObject buildableScriptableObject;

        public void Activate() {
            if (ObjectsReference.Instance.gameManager.gameContext == GameContext.IN_MINICHIMP_VIEW) {
                ObjectsReference.Instance.gestionMode.ActivateGhostByScriptableObject(buildableScriptableObject);
            }

            else {
                ObjectsReference.Instance.bananaMan.SetActiveBuildable(buildableScriptableObject);
            }
            
            SetDescriptionAndName();
        }
         
        public void SetDescriptionAndName() {
            ObjectsReference.Instance.uiDescriptionsManager.SetDescription(buildableScriptableObject);
        }
        
        public void SetColor(Color color) {
            GetComponent<Image>().color = color;
        }
    }
}
