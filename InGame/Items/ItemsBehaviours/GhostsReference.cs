using UnityEngine;

namespace InGame.Items.ItemsBehaviours {
    public class GhostsReference : MonoBehaviour {
        [SerializeField] private GenericDictionary<BuildableType, GameObject> ghostsByBuildablesTypes;
        
        public GameObject GetGhostByBuildableType(BuildableType buildableType) {
            return ghostsByBuildablesTypes[buildableType];
        }
    }
}