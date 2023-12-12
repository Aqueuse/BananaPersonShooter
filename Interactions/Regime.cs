using ItemsProperties.Regimes;
using UnityEngine;

namespace Interactions {
    public class Regime : MonoBehaviour {
        [SerializeField] private MeshRenderer babyBananierMeshRenderer;
        [SerializeField] private MeshRenderer youngBananierMeshRenderer;
        [SerializeField] private MeshRenderer matureBananierMeshRenderer;

        public RegimeStade regimeStade;

        public RegimePropertiesScriptableObject regimeDataScriptableObject;

        public void GrabBananas() {
            regimeStade = RegimeStade.BABY;

            babyBananierMeshRenderer.enabled = true;
            youngBananierMeshRenderer.enabled = false;
            matureBananierMeshRenderer.enabled = false;

            Invoke(nameof(Grown_young_bananier), 60);
        }

        private void Grown_young_bananier() {
            regimeStade = RegimeStade.YOUNG;
            
            babyBananierMeshRenderer.enabled = false;
            youngBananierMeshRenderer.enabled = true;
            matureBananierMeshRenderer.enabled = false;

            Invoke(nameof(Grown_mature_bananier), 60);
        }
        
        private void Grown_mature_bananier() {
            regimeStade = RegimeStade.MATURE;
            
            babyBananierMeshRenderer.enabled = false;
            youngBananierMeshRenderer.enabled = false;
            matureBananierMeshRenderer.enabled = true;
        }
    }
}