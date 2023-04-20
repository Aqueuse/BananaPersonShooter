using Data.Bananas;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI {
    public class UIBananapediaEntry : MonoBehaviour {
        [SerializeField] private BananasDataScriptableObject bananasDataScriptableObject;
        [SerializeField] private Image image;
        [SerializeField] private TextMeshProUGUI descriptionText;
        [SerializeField] private TextMeshProUGUI effectsText;

        public void OnClick() {
            descriptionText.text = bananasDataScriptableObject.itemDescription[ObjectsReference.Instance.gameSettings.languageIndexSelected];
            effectsText.text = bananasDataScriptableObject.effects[ObjectsReference.Instance.gameSettings.languageIndexSelected];
            image.sprite = bananasDataScriptableObject.itemSprite;
        }
    }
}
