using System.Collections.Generic;
using System.Linq;
using InGame.Items.ItemsProperties;
using UnityEngine;

namespace InGame.Items.ItemsBehaviours {
    public class Regime : MonoBehaviour {
        [SerializeField] private MeshRenderer babyBananierMeshRenderer;
        [SerializeField] private MeshRenderer youngBananierMeshRenderer;
        [SerializeField] private MeshRenderer matureBananierMeshRenderer;

        public ItemScriptableObject associatedBananaDataScriptableObject;

        public RegimeStade regimeStade;
        public int bananaQuantity = 55;

        [SerializeField] private List<GameObject> bananasStacks;
        
        public void GrabBananas() {
            if (regimeStade != RegimeStade.MATURE) return;
            
            if (ObjectsReference.Instance.shoot.IsTargetingMe(transform)) {
                ThrowBananas();
            }
        }

        private void ThrowBananas() {
            if (bananaQuantity > 0) {
                for (int i = 0; i < 5; i++) {
                    Instantiate(
                        associatedBananaDataScriptableObject.prefab,
                        transform.position,
                        transform.rotation,
                        ObjectsReference.Instance.gameSave.savablesItemsContainer
                    );
                    bananaQuantity -= 1;
                }

                foreach (var bananasStack in bananasStacks.Where(bananasStack => bananasStack.activeInHierarchy)) {
                    bananasStack.SetActive(false);
                    break;
                }
            }
            else {
                GrowBabyBananier();
            }
            
        }

        private void GrowBabyBananier() {
            regimeStade = RegimeStade.BABY;

            babyBananierMeshRenderer.enabled = true;
            youngBananierMeshRenderer.enabled = false;
            matureBananierMeshRenderer.enabled = false;

            Invoke(nameof(GrownYoungBananier), 60);
        }

        private void GrownYoungBananier() {
            regimeStade = RegimeStade.YOUNG;
            
            babyBananierMeshRenderer.enabled = false;
            youngBananierMeshRenderer.enabled = true;
            matureBananierMeshRenderer.enabled = false;

            Invoke(nameof(GrownMatureBananier), 60);
        }
        
        private void GrownMatureBananier() {
            regimeStade = RegimeStade.MATURE;
            
            foreach (var bananasStack in bananasStacks) {
                bananasStack.SetActive(true);
            }

            bananaQuantity = 55;
            
            babyBananierMeshRenderer.enabled = false;
            youngBananierMeshRenderer.enabled = false;
            matureBananierMeshRenderer.enabled = true;
        }
    }
}