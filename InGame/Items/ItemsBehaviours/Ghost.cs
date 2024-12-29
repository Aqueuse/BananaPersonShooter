using InGame.Items.ItemsProperties.Buildables;
using UnityEngine;

namespace InGame.Items.ItemsBehaviours {
    public class Ghost : MonoBehaviour {
        public BuildablePropertiesScriptableObject buildablePropertiesScriptableObject;

        private MeshRenderer _meshRenderer;
        private Material _ghostMaterial;
        
        private Material[] _buildableMaterials;
        private GhostState _ghostState = GhostState.NOT_ENOUGH_MATERIALS;

        private void Start() {
            _meshRenderer = GetComponent<MeshRenderer>();

            _buildableMaterials = new []{_ghostMaterial};
            _ghostMaterial = _meshRenderer.materials[0];
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
        
        private void OnTriggerExit(Collider other) {
            if (!ObjectsReference.Instance.bananaManRawMaterialInventory.HasCraftingIngredients(buildablePropertiesScriptableObject)) {
                SetGhostState(GhostState.NOT_ENOUGH_MATERIALS);
                return;
            }

            SetGhostState(GhostState.VALID);
        }
    }
}