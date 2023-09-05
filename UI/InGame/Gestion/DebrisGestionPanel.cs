using Data;
using TMPro;
using UnityEngine;

namespace UI.InGame.Gestion {
    public class DebrisGestionPanel : MonoBehaviour {
        [SerializeField] private TextMeshProUGUI descriptionText;

        public void SetDescription(ItemScriptableObject itemScriptableObject) {
            descriptionText.text = itemScriptableObject.GetDescription();
        }
    }
}
