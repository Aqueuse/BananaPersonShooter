using Enums;
using TMPro;
using UnityEngine;

namespace UI.InGame.Blueprints {
    public class UIBlueprints : MonoBehaviour {
        [SerializeField] private GenericDictionary<BuildableType, GameObject> blueprintsSlots;
    
        public TextMeshProUGUI itemName;
        public TextMeshProUGUI itemDescription;
        
        public void SetVisible(BuildableType buildableType) {
            blueprintsSlots[buildableType].SetActive(true);
        }
        
        public void HideAllBlueprints() {
            foreach (var blueprintSlot in blueprintsSlots) {
                blueprintSlot.Value.SetActive(false);
            }
        }
    }
}
