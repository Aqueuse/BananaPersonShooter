using Data;
using Interactions;
using Monkeys;
using Monkeys.Chimployees;
using Monkeys.MiniChimps;
using Monkeys.Visitors;
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
        
        public MonkeyGestionPanel monkeyGestionPanel;
        public MiniChimpGestionPanel miniChimpGestionPanel;
        public ChimployeeGestionPanel chimployeeGestionPanel;
        public VisitorGestionPanel visitorGestionPanel;

        public GenericDictionary<GAME_OBJECT_TAG, CanvasGroup> canvasGroupByItemTag;

        private MeshReferenceScriptableObject meshReferenceScriptableObject;
        private ItemScriptableObject scriptableObject;

        private void Start() {
            meshReferenceScriptableObject = ObjectsReference.Instance.scriptableObjectManager._meshReferenceScriptableObject;
        }

        public void ShowPanel(GAME_OBJECT_TAG gameObjectTag) {
            HideAllPanels();
            canvasGroupByItemTag[gameObjectTag].alpha = 1;
        }

        public void HideAllPanels() {
            foreach (var canvasGroup in canvasGroupByItemTag) {
                if (canvasGroup.Value.alpha > 0) canvasGroup.Value.alpha = 0;
            }
        }

        public void SetDescriptionByTag(GAME_OBJECT_TAG tag, GameObject itemGameObject) {
            switch (tag) {
                case GAME_OBJECT_TAG.COMMAND_ROOM_PANEL:
                    commandRoomGestionPanel.SetDescription();
                    break;
                case GAME_OBJECT_TAG.REGIME:
                    regimeGestionPanel.SetDescription(itemGameObject.GetComponent<Regime>().regimeDataScriptableObject);
                    break;
                case GAME_OBJECT_TAG.DOOR:
                    doorGestionPanel.SetDescription(itemGameObject.GetComponent<Door>().doorDataScriptableObject);
                    break;
                case GAME_OBJECT_TAG.DEBRIS:
                    scriptableObject = meshReferenceScriptableObject.gameObjectDataScriptableObjectsByTag[tag];
                    debrisGestionPanel.SetDescription(scriptableObject);
                    break;
                case GAME_OBJECT_TAG.RUINE:
                    scriptableObject = meshReferenceScriptableObject.gameObjectDataScriptableObjectsByTag[tag];
                    ruineGestionPanel.SetDescription(scriptableObject);
                    break;
                case GAME_OBJECT_TAG.VISITOR:
                    visitorGestionPanel.SetDescription(itemGameObject.GetComponent<Visitor>().visitorItemScriptableObject);
                    break;
                case GAME_OBJECT_TAG.MONKEY:
                    monkeyGestionPanel.SetDescription(itemGameObject.GetComponent<Monkey>().monkeyDataScriptableObject);
                    break;
                case GAME_OBJECT_TAG.MINI_CHIMP:
                    miniChimpGestionPanel.SetDescription(itemGameObject.GetComponent<MiniChimp>().miniChimpDataScriptableObject);
                    break;
                case GAME_OBJECT_TAG.CHIMPLOYEE:
                    chimployeeGestionPanel.SetDescription(itemGameObject.GetComponent<Chimployee>().chimployeeDataScriptableObject);
                    break;
            }
        }
    }
}
