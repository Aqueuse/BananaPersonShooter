using InGame.Items.ItemsProperties.Bananas;
using TMPro;
using UnityEngine;

namespace UI.InGame {
    public class UIFlippers : MonoBehaviour {
        [SerializeField] private Transform leftFlipperTransform;
        [SerializeField] private Transform middleFlipperTransform;
        [SerializeField] private Transform rightFlipperTransform;
        
        private Vector3 leftFlipperNormalPosition;
        private Vector3 middleFlipperNormalPosition;
        private Vector3 rightFlipperNormalPosition;

        private Vector3 leftFlipperUpPosition;
        private Vector3 middleFlipperUpPosition;
        private Vector3 rightFlipperUpPosition;
        
        [SerializeField] private SpriteRenderer bananaSpriteRenderer;
        [SerializeField] private TextMeshPro bananaQuantityText;
        [SerializeField] private SpriteRenderer buildableSpriteRenderer;

        [SerializeField] private Color availableBuildableColor;
        [SerializeField] private Color notEnoughMaterialBuildableColor;

        private void Start() {
            leftFlipperNormalPosition = leftFlipperTransform.localPosition;
            leftFlipperUpPosition = leftFlipperNormalPosition;
            leftFlipperUpPosition.z += 0.11f;

            middleFlipperNormalPosition = middleFlipperTransform.localPosition;
            middleFlipperUpPosition = middleFlipperNormalPosition;
            middleFlipperUpPosition.z += 0.11f;

            rightFlipperNormalPosition = rightFlipperTransform.localPosition;
            rightFlipperUpPosition = rightFlipperNormalPosition;
            rightFlipperUpPosition.z += 0.11f;

        }

        public void UpLeftFlipper() {
            leftFlipperTransform.localPosition = leftFlipperUpPosition;
            middleFlipperTransform.localPosition = middleFlipperNormalPosition;
            rightFlipperTransform.localPosition = rightFlipperNormalPosition;
        }
        
        public void UpMiddleFlipper() {
            leftFlipperTransform.localPosition = leftFlipperNormalPosition;
            middleFlipperTransform.localPosition = middleFlipperUpPosition;
            rightFlipperTransform.localPosition = rightFlipperNormalPosition;
        }

        public void UpRightFlipper() {
            leftFlipperTransform.localPosition = leftFlipperNormalPosition;
            middleFlipperTransform.localPosition = middleFlipperNormalPosition;
            rightFlipperTransform.localPosition = rightFlipperUpPosition;
        }
        
        public void SetBananaType(BananasPropertiesScriptableObject bananasPropertiesScriptableObject) {
            bananaSpriteRenderer.sprite = bananasPropertiesScriptableObject.GetSprite();
            bananaQuantityText.alpha = bananasPropertiesScriptableObject.bananaType == BananaType.EMPTY ? 0 : 1;
        }

        public void SetBananaQuantity(int bananaQuantity) {
            bananaQuantityText.text = bananaQuantity.ToString();
        }

        public void SetBuildable(Sprite sprite) {
            buildableSpriteRenderer.sprite = sprite;

            buildableSpriteRenderer.color =
                ObjectsReference.Instance.ghostsReference.GetGhostColorByAvailability(ObjectsReference.Instance.bananaMan
                    .activeBuildable);
        }

        public void SetBuildablePlacementAvailability(bool canBeBuild) {
            buildableSpriteRenderer.color = canBeBuild ? availableBuildableColor : notEnoughMaterialBuildableColor;
        }
    }
}
