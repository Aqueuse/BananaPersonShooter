using System.Collections.Generic;
using Data;
using Data.Door;
using Data.Monkeys;
using Data.Regimes;
using Enums;
using Interactions;
using UI.InGame.Gestion.blocks;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;

namespace UI.InGame.Gestion {
    public class descriptionsManager : MonoBehaviour {
        private ItemScriptableObject scriptableObject;

        [SerializeField] private ItemDescriptionBlock _itemDescriptionBlock;
        [SerializeField] private ItemPreviewBlock _itemPreviewBlock;
        [SerializeField] private OneItemSliderBlock _oneItemSliderBlock;
        [SerializeField] private ItemMultiSlidersBlock _multiSlidersBlock;
        [SerializeField] private ItemListBlock _itemListBlock;
        
        string growthLocalizedName;
        string bananasLocalizedName;
        
        private List<int> itemsQuantities;
        private List<string> itemsNames;
        //private List<string> itemsIDs;

        private void Start() {
            itemsQuantities = new List<int>();
            itemsNames = new List<string>();
            //itemsIDs = new List<string>();
        }
        
        public void SetDescription(ItemScriptableObject itemScriptableObject, GameObject targetedGameObject = null) {
            GetComponent<Image>().enabled = true;

            switch (itemScriptableObject.itemCategory) {
                case ItemCategory.BUILDABLE:
                    _itemDescriptionBlock.gameObject.SetActive(true);
                    _itemPreviewBlock.gameObject.SetActive(true);
                    _oneItemSliderBlock.gameObject.SetActive(false);
                    _multiSlidersBlock.gameObject.SetActive(false);
                    _itemListBlock.gameObject.SetActive(true);
                    
                    _itemDescriptionBlock.SetBlock(itemScriptableObject.GetName(), itemScriptableObject.GetDescription());
                    _itemPreviewBlock.SetBlock(itemScriptableObject.itemSprite);

                    var compositionName = LocalizationSettings.StringDatabase.GetTable("UI").GetEntry("composition").GetLocalizedString();
                    
                    itemsQuantities = new List<int>();
                    itemsNames = new List<string>();
                    //List<string> itemsIDs = new List<string>();

                    var rawMaterialsWithQuantities =
                        ObjectsReference.Instance.scriptableObjectManager.GetBuildableCraftingIngredients(itemScriptableObject
                            .buildableType);
                    
                    foreach (var rawMaterialsWithQuantity in rawMaterialsWithQuantities) {
                        itemsQuantities.Add(rawMaterialsWithQuantity.Value);
                        growthLocalizedName = LocalizationSettings.StringDatabase.GetTable("items").GetEntry(rawMaterialsWithQuantity.Key.ToString().ToLower()).GetLocalizedString();
                        itemsNames.Add(growthLocalizedName);
                    }
                    
                    _itemListBlock.SetBlock(
                        compositionName,
                        itemsQuantities.ToArray(),
                        itemsNames.ToArray()
                    );
                    break;
                case ItemCategory.CHIMPLOYEE:
                    _itemDescriptionBlock.gameObject.SetActive(true);
                    _itemPreviewBlock.gameObject.SetActive(false);
                    _oneItemSliderBlock.gameObject.SetActive(false);
                    _multiSlidersBlock.gameObject.SetActive(false);
                    _itemListBlock.gameObject.SetActive(false);

                    _itemDescriptionBlock.SetBlock(
                        itemScriptableObject.GetName(),
                        itemScriptableObject.GetDescription());
                    break;
                case ItemCategory.COMMAND_ROOM_PANEL:
                    _itemDescriptionBlock.gameObject.SetActive(true);
                    _itemPreviewBlock.gameObject.SetActive(false);
                    _oneItemSliderBlock.gameObject.SetActive(false);
                    _multiSlidersBlock.gameObject.SetActive(false);
                    _itemListBlock.gameObject.SetActive(false);

                    _itemDescriptionBlock.SetBlock(
                        itemScriptableObject.GetName(),
                        itemScriptableObject.GetDescription());
                    break;
                case ItemCategory.DEBRIS:
                    _itemDescriptionBlock.gameObject.SetActive(true);
                    _itemPreviewBlock.gameObject.SetActive(false);
                    _oneItemSliderBlock.gameObject.SetActive(false);
                    _multiSlidersBlock.gameObject.SetActive(false);
                    _itemListBlock.gameObject.SetActive(false);

                    _itemDescriptionBlock.SetBlock(
                        itemScriptableObject.GetName(),
                        itemScriptableObject.GetDescription());
                    break;
                case ItemCategory.BANANA or ItemCategory.RAW_MATERIAL or ItemCategory.INGREDIENT:
                    _itemDescriptionBlock.gameObject.SetActive(true);
                    _itemPreviewBlock.gameObject.SetActive(true);
                    _oneItemSliderBlock.gameObject.SetActive(false);
                    _multiSlidersBlock.gameObject.SetActive(false);
                    _itemListBlock.gameObject.SetActive(false);
                    
                    _itemDescriptionBlock.SetBlock(
                        itemScriptableObject.GetName(),
                        itemScriptableObject.GetDescription());
                    _itemPreviewBlock.SetBlock(itemScriptableObject.itemSprite);
                    break;
                case ItemCategory.DOOR:
                    _itemDescriptionBlock.gameObject.SetActive(true);
                    _itemPreviewBlock.gameObject.SetActive(true);
                    _oneItemSliderBlock.gameObject.SetActive(false);
                    _multiSlidersBlock.gameObject.SetActive(true);
                    _itemListBlock.gameObject.SetActive(false);

                    _itemDescriptionBlock.SetBlock(
                        itemScriptableObject.GetName(),
                        itemScriptableObject.GetDescription());
                    _itemPreviewBlock.SetBlock(itemScriptableObject.itemSprite);

                    var doorData = (DoorDataScriptableObject)itemScriptableObject;
                    var mapData = doorData.associatedMapDataScriptableObject;

                    var cleanliness = ObjectsReference.Instance.mapsManager.mapBySceneName[mapData.sceneName].cleanliness;
                    var visitors = 0; // TODO : get the true value

                    var cleanlinessName = LocalizationSettings.StringDatabase.GetTable("UI").GetEntry("cleanliness").GetLocalizedString();
                    var visitorsName = LocalizationSettings.StringDatabase.GetTable("UI").GetEntry("visitors").GetLocalizedString();
                    
                    _multiSlidersBlock.SetBlock(
                        new [] {
                            ((int)cleanliness, cleanlinessName),
                            (visitors, visitorsName)
                        },
                        100);
                    break;
                case ItemCategory.MINI_CHIMP:
                    _itemDescriptionBlock.gameObject.SetActive(true);
                    _itemPreviewBlock.gameObject.SetActive(false);
                    _oneItemSliderBlock.gameObject.SetActive(false);
                    _multiSlidersBlock.gameObject.SetActive(false);
                    _itemListBlock.gameObject.SetActive(false);

                    _itemDescriptionBlock.SetBlock(
                        itemScriptableObject.GetName(),
                        itemScriptableObject.GetDescription());
                    break;
                case ItemCategory.MONKEY:
                    _itemDescriptionBlock.gameObject.SetActive(true);
                    _itemPreviewBlock.gameObject.SetActive(true);
                    _oneItemSliderBlock.gameObject.SetActive(true);
                    _multiSlidersBlock.gameObject.SetActive(false);
                    _itemListBlock.gameObject.SetActive(false);

                    _itemDescriptionBlock.SetBlock(
                        itemScriptableObject.GetName(),
                        itemScriptableObject.GetDescription());
                    _itemPreviewBlock.SetBlock(itemScriptableObject.itemSprite);

                    var monkeyData = (MonkeyDataScriptableObject)itemScriptableObject;
                    
                    bananasLocalizedName = LocalizationSettings.StringDatabase.GetTable("UI").GetEntry("bananas").GetLocalizedString();
                    
                    _oneItemSliderBlock.SetBlock(
                        bananasLocalizedName,
                        (int)monkeyData.sasiety,
                        10);
                    break;
                case ItemCategory.REGIME:
                    _itemDescriptionBlock.gameObject.SetActive(true);
                    _itemPreviewBlock.gameObject.SetActive(false);
                    _oneItemSliderBlock.gameObject.SetActive(true);
                    _multiSlidersBlock.gameObject.SetActive(false);
                    _itemListBlock.gameObject.SetActive(false);

                    growthLocalizedName = LocalizationSettings.StringDatabase.GetTable("UI").GetEntry("growth").GetLocalizedString();

                    _itemDescriptionBlock.SetBlock(
                        itemScriptableObject.GetName(),
                        itemScriptableObject.GetDescription());
                    
                    if (targetedGameObject != null)
                        _oneItemSliderBlock.SetBlock(
                            growthLocalizedName,
                            (int)targetedGameObject.GetComponent<Regime>().regimeStade,
                            2);
                    break;
                case ItemCategory.VISITOR:
                    _itemDescriptionBlock.gameObject.SetActive(true);
                    _itemPreviewBlock.gameObject.SetActive(false);
                    _oneItemSliderBlock.gameObject.SetActive(false);
                    _multiSlidersBlock.gameObject.SetActive(false);
                    _itemListBlock.gameObject.SetActive(false);

                    _itemDescriptionBlock.SetBlock(
                        itemScriptableObject.GetName(),
                        itemScriptableObject.GetDescription());
//                    _multiSlidersBlock.SetBlock();
                    break;
            }
        }

        public void HideAllPanels() {
            _itemDescriptionBlock.gameObject.SetActive(false);
            _itemPreviewBlock.gameObject.SetActive(false);
            _oneItemSliderBlock.gameObject.SetActive(false);
            _multiSlidersBlock.gameObject.SetActive(false);
            _itemListBlock.gameObject.SetActive(false);
            GetComponent<Image>().enabled = false;
        }
    }
}
