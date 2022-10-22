using UnityEngine;

namespace Player {
    class BananaMan : MonoSingleton<BananaMan> {
        private BananasDataScriptableObject _activeBanana;
        public BananaType activeWeapon = BananaType.EMPTY_HAND;

        private TpsPlayerAnimator _tpsPlayerAnimator;

        public bool isArmed;
        public bool isShooting;

        public int health;
        public int resistance;
        
        private void Start() {
            _tpsPlayerAnimator = GetComponentInChildren<Animator>().GetComponent<TpsPlayerAnimator>();
        }

        public void Switch_weapon(BananaType bananaType) {
            activeWeapon = bananaType;

            if (activeWeapon == BananaType.EMPTY_HAND) {
                isArmed = false;
                _tpsPlayerAnimator.SwitchToUnarmedLayer();
            }
            else {
                isArmed = true;
                _tpsPlayerAnimator.SwitchToArmedLayer();
            }

            WeaponsManager.Instance.SetActiveWeapon(bananaType);
        }
    }
}