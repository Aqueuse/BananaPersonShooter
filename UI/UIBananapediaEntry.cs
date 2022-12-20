using Settings;
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
            descriptionText.text = bananasDataScriptableObject.bananaDescription[GameSettings.Instance.languageIndexSelected];
            effectsText.text = bananasDataScriptableObject.bananaEffects[GameSettings.Instance.languageIndexSelected];
            image.sprite = bananasDataScriptableObject.sprite;
        }
    }
}
