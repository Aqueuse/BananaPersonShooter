using Data;
using Data.Door;
using Data.Monkeys;
using Enums;
using Tags;
using UnityEngine;

namespace UI.InGame.Gestion {
    public class descriptionsManager : MonoBehaviour {
        public InventoryGestionPanel inventoryGestionPanel;

        public CommandRoomGestionPanel commandRoomGestionPanel;
        public DoorGestionPanel doorGestionPanel;
        public RegimeGestionPanel regimeGestionPanel;
        public DebrisGestionPanel debrisGestionPanel;
        public RuineGestionPanel ruineGestionPanel;
        public BuildableGestionPanel buildableGestionPanel;
        
        public MonkeyGestionPanel monkeyGestionPanel;
        public MiniChimpGestionPanel miniChimpGestionPanel;
        public ChimployeeGestionPanel chimployeeGestionPanel;
        public VisitorGestionPanel visitorGestionPanel;

        public GenericDictionary<GAME_OBJECT_TAG, CanvasGroup> canvasGroupByItemTag;

        private ItemScriptableObject scriptableObject;
        
        public void ShowPanel(GAME_OBJECT_TAG gameObjectTag) {
            HideAllPanels();
            canvasGroupByItemTag[gameObjectTag].alpha = 1;
        }

        public void HideAllPanels() {
            foreach (var canvasGroup in canvasGroupByItemTag) {
                if (canvasGroup.Value.alpha > 0) canvasGroup.Value.alpha = 0;
            }
        }

        public void SetDescription(ItemScriptableObject itemScriptableObject) {
            switch (itemScriptableObject.itemCategory) {
                case ItemCategory.COMMAND_ROOM_PANEL:
                    commandRoomGestionPanel.SetDescription();
                    break;
                case ItemCategory.REGIME:
                    regimeGestionPanel.SetDescription(itemScriptableObject);
                    break;
                case ItemCategory.DOOR:
                    doorGestionPanel.SetDescription((DoorDataScriptableObject)itemScriptableObject);
                    break;
                case ItemCategory.DEBRIS:
                    debrisGestionPanel.SetDescription(itemScriptableObject);
                    break;
                case ItemCategory.RUINE:
                    ruineGestionPanel.SetDescription(itemScriptableObject);
                    break;
                case ItemCategory.VISITOR:
                    visitorGestionPanel.SetDescription(itemScriptableObject);
                    break;
                case ItemCategory.MONKEY:
                    monkeyGestionPanel.SetDescription((MonkeyDataScriptableObject)itemScriptableObject);
                    break;
                case ItemCategory.MINI_CHIMP:
                    miniChimpGestionPanel.SetDescription(itemScriptableObject);
                    break;
                case ItemCategory.CHIMPLOYEE:
                    chimployeeGestionPanel.SetDescription(itemScriptableObject);
                    break;
                case ItemCategory.BUILDABLE:
                    buildableGestionPanel.SetDescription(itemScriptableObject);
                    break;
            }
        }
    }
}
