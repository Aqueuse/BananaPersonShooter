using InGame.Items.ItemsProperties.Buildables;
using UnityEngine;

namespace InGame.Items.ItemsBehaviours {
    public enum GhostState {
        UNBUILDABLE,
        NOT_ENOUGH_MATERIALS,
        VALID
    }
    
    public class GhostsReference : MonoBehaviour {
        [SerializeField] private GenericDictionary<BuildableType, GameObject> ghostsByBuildablesTypes;

        public GenericDictionary<GhostState, Color> colorByGhostState;
        
        public Color availableUIColor;
        public Color unavailableUIColor;

        public Color GetGhostColorByAvailability(BuildablePropertiesScriptableObject buildablePropertiesScriptableObject) {
            return !ObjectsReference.Instance.bananaManRawMaterialInventory.HasCraftingIngredients(buildablePropertiesScriptableObject)
                ? GetColorByGhostState(GhostState.NOT_ENOUGH_MATERIALS)
                : GetColorByGhostState(GhostState.VALID);
        }

        public Color GetUIColorByAvailability(bool isAvailable) {
            return isAvailable ? availableUIColor : unavailableUIColor;
        }
        
        public GameObject GetGhostByBuildableType(BuildableType buildableType) {
            return ghostsByBuildablesTypes[buildableType];
        }

        public Color GetColorByGhostState(GhostState ghostState) {
            return colorByGhostState[ghostState];
        }
    }
}