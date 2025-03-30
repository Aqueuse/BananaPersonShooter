using TMPro;
using UnityEngine;

namespace UI.InGame.CommandRoomControlPanels {
    public enum AssemblerMode {
        IDLE,
        BANANA_GUN
    }

    public class UIblueprinter : MonoBehaviour {
        [SerializeField] private TextMeshPro basicText;
        [SerializeField] private TextMeshPro bananaGunReparationText;
        [SerializeField] private TextMeshPro bananaGunPiecesQuantityText;

        public AssemblerMode assemblerMode = AssemblerMode.IDLE;
        
        public void SwitchToBananaGunReparationMode() {
            basicText.alpha = 0;
            bananaGunPiecesQuantityText.alpha = 1;
            bananaGunReparationText.alpha = 1;
        }

        public void SwitchToIdleMode() {
            basicText.alpha = 1;
            bananaGunPiecesQuantityText.alpha = 0;
            bananaGunReparationText.alpha = 0;
        }

        public void SetBananaGunPiecesQuantity(int newQuantity) {
            bananaGunPiecesQuantityText.text = newQuantity + "/4";
        }
    }
}
