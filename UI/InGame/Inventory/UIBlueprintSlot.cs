using ItemsProperties.Buildables;
using ItemsProperties.Buildables.VisitorsBuildable;
using UnityEngine;
using UnityEngine.UI;

namespace UI.InGame.Inventory {
    public class UIBlueprintSlot : MonoBehaviour {
        public BuildablePropertiesScriptableObject itemScriptableObject;
    
        public void SetDescriptionAndName() {
            ObjectsReference.Instance.descriptionsManager.SetDescription(itemScriptableObject);
        }

        public void SetColor(Color color) {
            GetComponent<Image>().color = color;
        }
    }
}
