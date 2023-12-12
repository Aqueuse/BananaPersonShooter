using ItemsProperties.Buildables.VisitorsBuildable;
using Tags;
using UnityEngine;

namespace Gestion {
    public class Ghost : MonoBehaviour {
        public BuildablePropertiesScriptableObject buildablePropertiesScriptableObject;

        private MeshRenderer _meshRenderer;
        private Material _ghostMaterial;
        
        private Material[] _buildableMaterials;
        private GhostState _ghostState;

        private void Start() {
            _meshRenderer = GetComponent<MeshRenderer>();

            _buildableMaterials = new []{_ghostMaterial};
            _ghostMaterial = _meshRenderer.materials[0];

            _ghostState = GhostState.UNBUILDABLE;
        }
        
        public void SetGhostState(GhostState newGhostState) {
            _ghostState = newGhostState;
            _ghostMaterial.color = ObjectsReference.Instance.ghostsReference.GetColorByGhostState(newGhostState);
            
            _buildableMaterials = new []{_ghostMaterial};
            _meshRenderer.materials = _buildableMaterials;
        }

        public GhostState GetGhostState() {
            return _ghostState;
        }

        private void OnTriggerEnter(Collider other) {
            if (TagsManager.Instance.HasTag(other.gameObject, GAME_OBJECT_TAG.BUILD_UNVALID)) {
                SetGhostState(GhostState.UNBUILDABLE);
            }
        }

        private void OnTriggerExit(Collider other) {
            if (!ObjectsReference.Instance.rawMaterialsInventory.HasCraftingIngredients(buildablePropertiesScriptableObject)) {
                SetGhostState(GhostState.NOT_ENOUGH_MATERIALS);
                return;
            }

            SetGhostState(GhostState.VALID);
        }
    }
}