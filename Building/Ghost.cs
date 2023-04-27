using Data.Buildables;
using UnityEngine;

namespace Building {
    public class Ghost : MonoBehaviour {
        public BuildableDataScriptableObject buildableDataScriptableObject;

        private MeshRenderer _meshRenderer;
        private Material ghostMaterial;

        
        private Material[] buildableMaterials;
        private GhostState ghostState;
        
        private void Start() {
            _meshRenderer = GetComponent<MeshRenderer>();

            buildableMaterials = new []{ghostMaterial};
            ghostMaterial = _meshRenderer.materials[0];

            ghostState = GhostState.VALID;
        }

        public void SetGhostState(GhostState newGhostState) {
            ghostState = newGhostState;
            ghostMaterial.color = ObjectsReference.Instance.ghostsReference.GetColorByGhostState(newGhostState);
            
            buildableMaterials = new []{ghostMaterial};
            _meshRenderer.materials = buildableMaterials;
        }

        public GhostState GetPlateformState() {
            return ghostState;
        }
    }
}