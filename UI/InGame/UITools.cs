using UnityEngine;
using UnityEngine.UI;

namespace UI.InGame {
    public class UITools : MonoBehaviour {
        [SerializeField] private Image shootImage;
        [SerializeField] private Image takeImage;
        [SerializeField] private Image placeImage;

        private readonly Vector3 unzoomedScale = new(0.6f, 0.6f, 1);
        private readonly Vector3 zoomedScale = new(1, 1, 1);

        public void ZoomShootIcon() {
            shootImage.rectTransform.localScale = zoomedScale;
            takeImage.rectTransform.localScale = unzoomedScale;
            placeImage.rectTransform.localScale = unzoomedScale;
        }

        public void ZoomTakeIcon() {
            shootImage.rectTransform.localScale = unzoomedScale;
            takeImage.rectTransform.localScale = zoomedScale;
            placeImage.rectTransform.localScale = unzoomedScale;
        }

        public void ZoomPlaceIcon() {
            shootImage.rectTransform.localScale = unzoomedScale;
            takeImage.rectTransform.localScale = unzoomedScale;
            placeImage.rectTransform.localScale = zoomedScale;
        }

        public void UnzoomIcons() {
            shootImage.rectTransform.localScale = unzoomedScale;
            takeImage.rectTransform.localScale = unzoomedScale;
            placeImage.rectTransform.localScale = unzoomedScale;
        }
    }
}
