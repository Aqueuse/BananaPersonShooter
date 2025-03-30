using InGame.Items.ItemsProperties;
using InGame.Items.ItemsProperties.Buildables;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.InGame {
    public class UIFlippers : MonoBehaviour {
        [SerializeField] private Image droppedImage;
        [SerializeField] private TextMeshProUGUI droppedQuantityText;

        [SerializeField] private Image buildableImage;
        [SerializeField] private Image buildableBackgroundImage;

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
        
        public void Init() {
            droppedImage.sprite = null;
            droppedQuantityText.text = "";
            RefreshActiveBuildableAvailability();
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

        public void SetDroppableItem(ItemScriptableObject itemScriptableObject) {
            ObjectsReference.Instance.bananaMan.bananaManData.activeDroppableItem = itemScriptableObject;
            
            SetDroppableSprite(itemScriptableObject.GetSprite());
            SetDroppableQuantity(itemScriptableObject);
        }
        
        public void SetDroppableSprite(Sprite droppedSprite) {
            droppedImage.enabled = true;
            droppedImage.sprite = droppedSprite;
            droppedImage.material.color = Color.white;
        }

        public void SetDroppableQuantity(ItemScriptableObject itemScriptableObject) {
            var quantity = ObjectsReference.Instance.bananaMan.bananaManData.inventoriesByDroppedType
                [itemScriptableObject.droppedType].GetQuantity(itemScriptableObject);
            
            droppedQuantityText.text = quantity.ToString();
        }
        
        public void RefreshActiveDroppableQuantity() {
            if (ObjectsReference.Instance.bananaMan.bananaManData.activeDroppableItem == null) return;
            
            var quantity = ObjectsReference.Instance.bananaMan.bananaManData.GetActiveDroppableItemQuantity();
            
            droppedQuantityText.text = quantity.ToString();
        }
        
        public void SetBuildablePlacementAvailability(bool canBeBuild, BuildablePropertiesScriptableObject buildablePropertieScriptableObject) {
            buildableImage.sprite = canBeBuild
                ? buildablePropertieScriptableObject.GetSprite()
                : buildablePropertieScriptableObject.blueprintSprite;

            buildableBackgroundImage.color = canBeBuild
                ? ObjectsReference.Instance.ghostsReference.availableUIColor
                : ObjectsReference.Instance.ghostsReference.unavailableUIColor;
        }

        public void RefreshActiveBuildableAvailability() {
            var activeBuildable = ObjectsReference.Instance.bananaMan
                .bananaManData.activeBuildable;

            var buildablePropertieScriptableObject =
                ObjectsReference.Instance.meshReferenceScriptableObject.buildablePropertiesScriptableObjects[
                    activeBuildable];
            
            SetBuildablePlacementAvailability(
                ObjectsReference.Instance.bananaManRawMaterialInventory
                    .HasCraftingIngredients(buildablePropertieScriptableObject), buildablePropertieScriptableObject);
        }
    }
}