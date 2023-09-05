using Enums;
using UnityEngine;

namespace Building.Buildables {
    public class DissolveMe : MonoBehaviour {
        private float dissolved;
        
        private MeshRenderer[] _meshRenderers;
        private Material _buildableInstanciatedMaterial;

        private static readonly int AlphaProperty = Shader.PropertyToID("alpha");
        
        private float _step;
        private float _dissolve;
        private bool _isDissolving;
        
        private ItemCategory _itemCategory;
        private BananaType bananaType;
        private BuildableType _buildableType;
        
        private void Start() {
            _meshRenderers = GetComponents<MeshRenderer>();
            
            _buildableInstanciatedMaterial = _meshRenderers[0].materials[0];
            _dissolve = 1.5f;
        }

        private void Update() {
            if (!_isDissolving) return;

            _dissolve -= Time.deltaTime;

            _buildableInstanciatedMaterial.SetFloat(AlphaProperty, _dissolve);
            foreach (var meshRenderer in _meshRenderers) {
                meshRenderer.materials[0] = _buildableInstanciatedMaterial;
            }

            if (_dissolve < dissolved) {
            }
        }

        public void Dissolve(ItemCategory itemCategory, BuildableType buildableType, BananaType bananaType) {
            _itemCategory = itemCategory;
            _buildableType = buildableType;
            this.bananaType = bananaType;
            _isDissolving = true;
        }
    }
}