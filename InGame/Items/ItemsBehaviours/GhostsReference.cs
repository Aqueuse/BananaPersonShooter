using UnityEngine;

namespace InGame.Items.ItemsBehaviours {
    public enum GhostState {
        UNBUILDABLE,
        NOT_ENOUGH_MATERIALS,
        VALID
    }

    public class GhostsReference : MonoBehaviour {
        [SerializeField] private GenericDictionary<BuildableType, GameObject> ghostsByBuildablesTypes;
        [SerializeField] private GenericDictionary<GhostState, Color> colorByGhostState;

        public GameObject GetGhostByBuildableType(BuildableType buildableType) {
            return ghostsByBuildablesTypes[buildableType];
        }

        public Color GetColorByGhostState(GhostState ghostState) {
            return colorByGhostState[ghostState];
        }
    }
}