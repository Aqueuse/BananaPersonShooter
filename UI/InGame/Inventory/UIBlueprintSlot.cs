using InGame.Items.ItemsProperties.Buildables;
using UnityEngine;
using UnityEngine.UI;

namespace UI.InGame.Inventory {
    public class UIBlueprintSlot : MonoBehaviour {
        public BuildablePropertiesScriptableObject itemScriptableObject;

        public void Activate() {
            if (!ObjectsReference.Instance.rawMaterialsInventory.HasCraftingIngredients(itemScriptableObject.buildableType)) return;

            ObjectsReference.Instance.gestionBuild.ActivateGhostByScriptableObject(itemScriptableObject);
            SetDescriptionAndName();
        }
        
        public void SetDescriptionAndName() {
            ObjectsReference.Instance.descriptionsManager.SetDescription(itemScriptableObject);
        }

        public void SetColor(Color color) {
            GetComponent<Image>().color = color;
        }
    }
}
