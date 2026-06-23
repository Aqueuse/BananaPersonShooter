using System.Collections.Generic;
using UnityEngine;

namespace UI.InGame.BuildingMaterialMissingPanel {
    public class UIMissingMaterialBuildingPanel : MonoBehaviour {
        [SerializeField] private List<UIMissingMaterialSlot> materialSlots;

        public void ShowMissingMaterials(List<(Sprite, int)> rawMaterialsWithQuantity) {
            foreach (var uiMissingMaterialSlot in materialSlots) {
                uiMissingMaterialSlot.gameObject.SetActive(false);
            }
            
            for (var i = 0; i < rawMaterialsWithQuantity.Count; i++) {
                materialSlots[i].gameObject.SetActive(true);
                materialSlots[i].SetMaterialSlot(rawMaterialsWithQuantity[i].Item1, "X "+rawMaterialsWithQuantity[i].Item2);
            }
        }
    }
}