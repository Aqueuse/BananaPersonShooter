using Data.Bananas;
using UnityEngine;

namespace Items {
    public class Regime : MonoBehaviour {
        [SerializeField] private GameObject babyBananier;
        [SerializeField] private GameObject youngBananier;
        [SerializeField] private GameObject matureBananier;
        
        public BananasDataScriptableObject bananasDataScriptableObject;
        
        public void GrabBananas() {
            gameObject.tag = "Untagged";
        
            babyBananier.SetActive(true);
            youngBananier.SetActive(false);
            matureBananier.SetActive(false);
            
            Invoke(nameof(Grown_young_bananier), 60);
        }

        private void Grown_young_bananier() {
            babyBananier.SetActive(false);
            youngBananier.SetActive(true);
            matureBananier.SetActive(false);
            
            Invoke(nameof(Grown_mature_bananier), 60);
        }
        
        private void Grown_mature_bananier() {
            gameObject.tag = "Regime";
            
            babyBananier.SetActive(false);
            youngBananier.SetActive(false);
            matureBananier.SetActive(true);
        }
    }
}