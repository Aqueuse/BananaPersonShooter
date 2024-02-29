using TMPro;
using UnityEngine;

namespace UI.InGame.SpaceTrafficControlMiniGame {
    public class SpaceTrafficControlMiniGameUI : MonoBehaviour {
        [SerializeField] private TextMeshProUGUI bottomBananasQuantityText;
        [SerializeField] private GenericDictionary<RegionType, SpriteRenderer> spriteRenderersBySceneType;
        [SerializeField] private GenericDictionary<RegionType, TextMeshProUGUI> textsBySceneType;

        private Color activationColor;

        private void Start() {
            activationColor = ObjectsReference.Instance.uiManager.activationColor;
        }

        public void RefreshBananasQuantity(BananaType bananaType) {
            bottomBananasQuantityText.text = ObjectsReference.Instance.bananasInventory.GetQuantity(bananaType).ToString();
        }

        public void ActivateButton(RegionType regionType) {
            DesactivateButtons();
            SetActivatedButton(regionType);
        }

        public void DesactivateButtons() {
            foreach (var spriteRenderer in spriteRenderersBySceneType) {
                SetUnactivatedButton(spriteRenderer.Key);
            }
        }
        
        private void SetActivatedButton(RegionType regionType) {
            spriteRenderersBySceneType[regionType].color = activationColor;
            textsBySceneType[regionType].color = Color.black;
        }

        private void SetUnactivatedButton(RegionType regionType) {
            spriteRenderersBySceneType[regionType].color = Color.black;
            textsBySceneType[regionType].color = activationColor;
        }
    }
}
