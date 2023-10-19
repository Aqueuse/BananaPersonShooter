using Data.Bananas;
using Data.Regimes;
using Tags;
using UnityEngine;

namespace Interactions {
    public class Regime : MonoBehaviour {
        [SerializeField] private MeshRenderer babyBananierMeshRenderer;
        [SerializeField] private MeshRenderer youngBananierMeshRenderer;
        [SerializeField] private MeshRenderer matureBananierMeshRenderer;
        
        public RegimeDataScriptableObject regimeDataScriptableObject;
        
        public void GrabBananas() {
            gameObject.layer = 0;

            babyBananierMeshRenderer.enabled = true;
            youngBananierMeshRenderer.enabled = false;
            matureBananierMeshRenderer.enabled = false;
            
            Invoke(nameof(Grown_young_bananier), 60);
        }

        private void Grown_young_bananier() {
            babyBananierMeshRenderer.enabled = false;
            youngBananierMeshRenderer.enabled = true;
            matureBananierMeshRenderer.enabled = false;
            
            Invoke(nameof(Grown_mature_bananier), 60);
        }
        
        private void Grown_mature_bananier() {
            gameObject.layer = 7;
            
            babyBananierMeshRenderer.enabled = false;
            youngBananierMeshRenderer.enabled = false;
            matureBananierMeshRenderer.enabled = true;
        }
    }
}