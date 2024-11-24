using InGame.Items.ItemsProperties.Buildables;
using UnityEngine;

namespace InGame.Items.ItemsBehaviours {
    public enum GhostState {
        UNBUILDABLE,
        NOT_ENOUGH_MATERIALS,
        VALID
    }
    
    public enum UIState {
        NOT_ENOUGH_MATERIALS,
        VALID
    }
    
    public class GhostsReference : MonoBehaviour {
        [SerializeField] private GenericDictionary<BuildableType, GameObject> ghostsByBuildablesTypes;

        public GenericDictionary<GhostState, Color> colorByGhostState;
        public GenericDictionary<UIState, Color> colorByUIState;

        public Color GetGhostColorByAvailability(BuildablePropertiesScriptableObject buildablePropertiesScriptableObject) {
            return !ObjectsReference.Instance.rawMaterialInventory.HasCraftingIngredients(buildablePropertiesScriptableObject)
                ? GetColorByGhostState(GhostState.NOT_ENOUGH_MATERIALS)
                : GetColorByGhostState(GhostState.VALID);
        }

        public Color GetUIColorByAvailability(BuildablePropertiesScriptableObject buildablePropertiesScriptableObject) {
            return !ObjectsReference.Instance.rawMaterialInventory.HasCraftingIngredients(buildablePropertiesScriptableObject)
                ? colorByUIState[UIState.NOT_ENOUGH_MATERIALS]
                : colorByUIState[UIState.VALID];
        }
        
        public GameObject GetGhostByBuildableType(BuildableType buildableType) {
            return ghostsByBuildablesTypes[buildableType];
        }

        public Color GetColorByGhostState(GhostState ghostState) {
            return colorByGhostState[ghostState];
        }
    }
}