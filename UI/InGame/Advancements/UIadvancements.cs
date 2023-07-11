using Enums;
using Game.CommandRoomPanelControls;
using UnityEngine;
using UnityEngine.UI;

namespace UI.InGame.Advancements {
    public class UIadvancements : MonoBehaviour {
        [SerializeField] private GenericDictionary<AdvancementState, GameObject> goalsByAdvancement;
        [SerializeField] private GameObject unlostAdvancementBanana;
        
        public void SetAdvancementBanana(AdvancementState advancementState) {
            if (GetComponentsInChildren<Image>().Length > 0) {
                if (GetComponentInChildren<UIadvancement>() != null) {
                    if (GetComponentInChildren<UIadvancement>().advancementState != advancementState) {
                        Destroy(GetComponentInChildren<UIadvancement>().gameObject);
                        Instantiate(goalsByAdvancement[advancementState], transform);
                    }                    
                }
                else {
                    Destroy(GetComponentInChildren<Image>().gameObject);
                    Instantiate(goalsByAdvancement[advancementState], transform);
                }
            }

            else {
                Instantiate(goalsByAdvancement[advancementState], transform);
            }
        }

        // you can aspire directly the chimployee without speaking to mini chimp before
        // if you do that, you will lake a step in the quests continuity,
        // so you will be returned to mini chimp to refresh your best advancement and find your way back to good quest
        public void SetSpeakToMiniChimpAdvancementBanana() {
            if (ObjectsReference.Instance.mapsManager.currentMap.mapName == "COMMAND_ROOM") CommandRoomControlPanelsManager.Instance.miniChimpCommandRoomMiniChimpDialogue.HideBubble();
            
            if (GetComponentsInChildren<Image>().Length > 0) Destroy(GetComponentInChildren<Image>().gameObject);
            Instantiate(unlostAdvancementBanana, transform);
        }

        public void SetBestAdvancement() {
            ObjectsReference.Instance.uIadvancements.SetAdvancementBanana(ObjectsReference.Instance.advancements.GetBestAdvancement());
        }

        public void FlushAdvancements() {
            if (GetComponentsInChildren<Image>().Length != 0) Destroy(GetComponentInChildren<Image>().gameObject);
        }
    }
}
