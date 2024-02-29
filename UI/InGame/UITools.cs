using DG.Tweening;
using TMPro;
using UnityEngine;

namespace UI.InGame {
    public class UITools : MonoBehaviour {
        [SerializeField] private Transform leftFlipperTransform;
        [SerializeField] private Transform middleFlipperTransform;
        [SerializeField] private Transform rightFlipperTransform;
        
        [SerializeField] private Ease customEase;
        
        [SerializeField] private TextMeshPro bananaQuantityText;
        [SerializeField] private SpriteRenderer plateformSpriteRenderer;

        [SerializeField] private Color availableBuildPlateformColor;
        [SerializeField] private Color notEnoughMaterialPlateformColor;
        
        private Vector3 initialFlipRotation;
        private Vector3 rotatedFlipRotation;

        private void Start() {
            initialFlipRotation = new Vector3(0, 0, 0);
            rotatedFlipRotation = new Vector3(0, 180, 0);
        }

        public void ZoomShootIcon() {
            //leftFlipperTransform.DOLocalRotate(rotatedFlipRotation, 0.3f).SetEase(customEase);
        }

        public void ZoomTakeIcon() {
            //middleFlipperTransform.DOLocalRotate(rotatedFlipRotation, 0.3f).SetEase(customEase);
        }

        public void ZoomPlaceIcon() {
            //rightFlipperTransform.DOLocalRotate(rotatedFlipRotation, 0.3f).SetEase(customEase);
        }
        
        public void UnzoomIcons() {
//            leftFlipperTransform.DOLocalRotate(initialFlipRotation, 0.3f).SetEase(customEase);
//            middleFlipperTransform.DOLocalRotate(initialFlipRotation, 0.3f).SetEase(customEase);
//            rightFlipperTransform.DOLocalRotate(initialFlipRotation, 0.3f).SetEase(customEase);
        }

        public void SetBananaQuantity(int bananaQuantity) {
            bananaQuantityText.text = bananaQuantity.ToString();
        }

        public void SetPlateformPlacementAvailability(bool canBeBuild) {
            plateformSpriteRenderer.color = canBeBuild ? availableBuildPlateformColor : notEnoughMaterialPlateformColor;
        }
    }
}
