using Enums;
using UnityEngine;

namespace Building {
    public enum GhostState {
        UNBUILDABLE,
        UNVALID,
        VALID
    }

    public class GhostsReference : MonoBehaviour {
        [SerializeField] private GenericDictionary<BuildableType, GameObject> ghostsByBuildablesTypes;
        [SerializeField] private GenericDictionary<GhostState, Color> colorByGhostState;
        
        public GameObject GetGhostByBuildableType(BuildableType buildableType) {
            return ghostsByBuildablesTypes[buildableType];
        }

        public Color GetColorByGhostState(GhostState ghostState ) {
            return colorByGhostState[ghostState];
        }
    }
}
