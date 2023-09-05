using Data;
using TMPro;
using UnityEngine;

namespace UI.InGame.Gestion {
    public class RuineGestionPanel : MonoBehaviour {
        [SerializeField] private TextMeshProUGUI descriptionText;

        public void SetDescription(ItemScriptableObject itemScriptableObject) {
            descriptionText.text = itemScriptableObject.GetDescription();
        }
    }
}
