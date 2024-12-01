using UnityEngine;
using UnityEngine.UI;

namespace UI.InGame {
    public class UICrosshairs : MonoBehaviour {
        [SerializeField] private Image emptyHandCrosshairImage;
        [SerializeField] private Image bananaGunCrosshairImage;
        
        public void SetCrosshair(bool hasBananaGun) {
            if (hasBananaGun) {
                bananaGunCrosshairImage.enabled = true;
                emptyHandCrosshairImage.enabled = false;
            }
            else {
                bananaGunCrosshairImage.enabled = false;
                emptyHandCrosshairImage.enabled = true;
            }
        }

        private void HideCrosshairs() {
            bananaGunCrosshairImage.enabled = false;
            emptyHandCrosshairImage.enabled = false;
        }
    }
}
