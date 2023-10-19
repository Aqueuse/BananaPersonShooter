using Data.Door;
using TMPro;
using UnityEngine;

namespace UI.InGame.Gestion {
    public class DoorGestionPanel : MonoBehaviour {
        [SerializeField] private TextMeshProUGUI itemName;
        [SerializeField] private TextMeshProUGUI itemDescription;

        [SerializeField] private TextMeshProUGUI monkeysQuantityText;
        [SerializeField] private TextMeshProUGUI visitorsQuantityText;
        
        public void SetDescription(DoorDataScriptableObject doorDataScriptableObject) {
            itemName.text = doorDataScriptableObject.GetName();
            itemDescription.text = doorDataScriptableObject.GetDescription();

            monkeysQuantityText.text = doorDataScriptableObject.associatedMapDataScriptableObject.monkeyDataScriptableObjectsByMonkeyId.Count.ToString();
        }
    }
}
