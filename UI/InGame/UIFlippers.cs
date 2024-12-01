using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.InGame {
    public class UIFlippers : MonoBehaviour {
        [SerializeField] private Image droppedImage;
        [SerializeField] private TextMeshProUGUI droppedQuantityText;
        [SerializeField] private Image buildableImage;

        [SerializeField] private Color availableBuildableColor;
        [SerializeField] private Color notEnoughMaterialBuildableColor;
        
        [SerializeField] private RectTransform leftFlipperTransform;
        [SerializeField] private RectTransform middleFlipperTransform;
        [SerializeField] private RectTransform rightFlipperTransform;

        private Vector3 leftFlipperNormalPosition;
        private Vector3 middleFlipperNormalPosition;
        private Vector3 rightFlipperNormalPosition;

        private Vector3 leftFlipperUpPosition;
        private Vector3 middleFlipperUpPosition;
        private Vector3 rightFlipperUpPosition;

        private void Start() {
            leftFlipperNormalPosition = leftFlipperTransform.localPosition;
            leftFlipperUpPosition = leftFlipperNormalPosition;
            leftFlipperUpPosition.y += 55f;

            middleFlipperNormalPosition = middleFlipperTransform.localPosition;
            middleFlipperUpPosition = middleFlipperNormalPosition;
            middleFlipperUpPosition.y += 55f;

            rightFlipperNormalPosition = rightFlipperTransform.localPosition;
            rightFlipperUpPosition = rightFlipperNormalPosition;
            rightFlipperUpPosition.y += 55f;
        }

        public void SetDroppableSprite(Sprite droppedSprite) {
            droppedImage.sprite = droppedSprite;
        }

        public void SetDroppableQuantity(string quantity) {
            droppedQuantityText.text = quantity;
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
        
        public void SetBuildableImage(Sprite sprite) {
            buildableImage.sprite = sprite;

            buildableImage.color =
                ObjectsReference.Instance.ghostsReference
                    .GetGhostColorByAvailability(
                        ObjectsReference.Instance.meshReferenceScriptableObject
                            .buildablePropertiesScriptableObjects[
                                ObjectsReference.Instance.bananaMan
                                    .bananaManData.activeBuildable]
                    );
        }

        public void SetBuildablePlacementAvailability(bool canBeBuild) {
            buildableImage.color = canBeBuild
                ? availableBuildableColor
                : notEnoughMaterialBuildableColor;
        }

        public void RefreshActiveBuildableAvailability() {
            var activeBuildable = ObjectsReference.Instance.bananaMan
                .bananaManData.activeBuildable;

            SetBuildablePlacementAvailability(
                ObjectsReference.Instance.bananaManRawMaterialInventory
                    .HasCraftingIngredients(activeBuildable));
        }
    }
}