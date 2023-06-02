using Data.Buildables;
using UnityEngine;

namespace Building {
    public class Ghost : MonoBehaviour {
        public BuildableDataScriptableObject buildableDataScriptableObject;

        private MeshRenderer _meshRenderer;
        private Material _ghostMaterial;
        
        private Material[] _buildableMaterials;
        private GhostState _ghostState;

        private void Start() {
            _meshRenderer = GetComponent<MeshRenderer>();

            _buildableMaterials = new []{_ghostMaterial};
            _ghostMaterial = _meshRenderer.materials[0];

            _ghostState = GhostState.VALID;
        }
        
        public void SetGhostState(GhostState newGhostState) {
            _ghostState = newGhostState;
            _ghostMaterial.color = ObjectsReference.Instance.ghostsReference.GetColorByGhostState(newGhostState);
            
            _buildableMaterials = new []{_ghostMaterial};
            _meshRenderer.materials = _buildableMaterials;
        }

        public GhostState GetPlateformState() {
            return _ghostState;
        }
    }
}