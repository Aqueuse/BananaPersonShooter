using UnityEngine;
using UnityEngine.UI;

namespace UI.InGame.Gestion.blocks {
    public class ItemPreviewBlock : MonoBehaviour {
        [SerializeField] private Image _image;

        public void SetBlock(Sprite sprite) {
            _image.sprite = sprite;
        }
    }
}
