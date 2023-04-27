using Data.Bananas;
using Enums;
using UI.InGame;
using UnityEngine;

namespace Items {
    public class Regime : MonoBehaviour {
        [SerializeField] private GenericDictionary<BananierState, GameObject> bananiersPrefabByState;

        public BananasDataScriptableObject bananasDataScriptableObject;
        public BananierState bananierState;
        
        public void GrabBananas() {
            gameObject.tag = "Untagged";
            
            foreach (var bananier in bananiersPrefabByState) {
                bananier.Value.SetActive(bananier.Key == BananierState.BABY);
            }

            bananierState = BananierState.BABY;

            Invoke(nameof(Grown_young_bananier), 60);
        }

        private void Grown_young_bananier() {
            foreach (var bananier in bananiersPrefabByState) {
                bananier.Value.SetActive(bananier.Key == BananierState.YOUNG);
            }

            bananierState = BananierState.YOUNG;

            Invoke(nameof(Grown_mature_bananier), 60);
        }
        
        private void Grown_mature_bananier() {
            gameObject.tag = "Aspirables";
            
            foreach (var bananier in bananiersPrefabByState) {
                bananier.Value.SetActive(bananier.Key == BananierState.MATURE);
            }
            
            bananierState = BananierState.MATURE;
        }
    }
}