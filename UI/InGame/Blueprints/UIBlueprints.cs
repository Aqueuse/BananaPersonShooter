using UnityEngine;

namespace UI.InGame.Blueprints {
    public class UIBlueprints : MonoBehaviour {
        [SerializeField] private GenericDictionary<BuildableType, GameObject> blueprintsSlots;
    
        public UIBlueprintSlot[] GetActivatedBlueprints() {
            return transform.GetComponentsInChildren<UIBlueprintSlot>();
        }

        public void SetVisible(BuildableType buildableType) {
            blueprintsSlots[buildableType].SetActive(true);
        }
    }
}
