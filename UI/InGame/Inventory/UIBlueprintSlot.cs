using Data.Buildables;
using UnityEngine;
using UnityEngine.UI;

namespace UI.InGame.Inventory {
    public class UIBlueprintSlot : MonoBehaviour {
        public BuildableDataScriptableObject itemScriptableObject;
    
        public void SetDescriptionAndName() {
            ObjectsReference.Instance.descriptionsManager.SetDescription(itemScriptableObject);
        }

        public void SetColor(Color color) {
            GetComponent<Image>().color = color;
        }
    }
}
