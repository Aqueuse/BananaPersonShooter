using UnityEngine;

namespace Player {
    class BananaMan : MonoSingleton<BananaMan> {
        public BananasDataScriptableObject activeBanana;

        public BananaType activeBananaType = BananaType.EMPTY_HAND;
        public TpsPlayerAnimator tpsPlayerAnimator;
        
        [SerializeField] public Transform shootingHand;
        
        public bool isArmed;

        public float health;
        public float resistance;
        
        private void Start() {
            tpsPlayerAnimator = GetComponentInChildren<Animator>().GetComponent<TpsPlayerAnimator>();
        }
    }
}