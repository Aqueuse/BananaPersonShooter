using Enums;
using UnityEngine;

namespace Items {
    public class Regime : MonoBehaviour {
        [SerializeField] private GenericDictionary<BananierState, GameObject> bananiersPrefabByState;

        public BananasDataScriptableObject bananasDataScriptableObject;
        
        public int activeBananierState;

        private void Start() {
            activeBananierState = 2;
        }

        public void GrabBananas() {
            gameObject.layer = LayerMask.NameToLayer("Terrain");
            
            foreach (var bananier in bananiersPrefabByState) {
                bananier.Value.SetActive(bananier.Key == BananierState.BABY);
            }

            activeBananierState = 0;

            Invoke(nameof(Grown_young_bananier), 60);
        }

        private void Grown_young_bananier() {
            foreach (var bananier in bananiersPrefabByState) {
                bananier.Value.SetActive(bananier.Key == BananierState.YOUNG);
            }

            activeBananierState = 1;
            
            Invoke(nameof(Grown_mature_bananier), 60);
        }
        
        private void Grown_mature_bananier() {
            gameObject.layer = LayerMask.NameToLayer("Items");

            foreach (var bananier in bananiersPrefabByState) {
                bananier.Value.SetActive(bananier.Key == BananierState.MATURE);
            }

            activeBananierState = 2;
        }
    }
}