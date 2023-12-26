using System.Collections.Generic;
using Game.Monkeys.Ancestors;
using ItemsProperties;
using ItemsProperties.Monkeys;
using Interactions;
using ItemsProperties.Doors;
using UI.InGame.Gestion.blocks;
using UnityEngine;
using UnityEngine.Localization.Settings;

namespace UI.InGame.Gestion {
    public class DescriptionsManager : MonoBehaviour {
        private ItemScriptableObject scriptableObject;

        [SerializeField] private ItemDescriptionBlock _itemDescriptionBlock;
        [SerializeField] private ItemPreviewBlock _itemPreviewBlock;
        [SerializeField] private OneItemSliderBlock _oneItemSliderBlock;
        [SerializeField] private ItemMultiSlidersBlock _multiSlidersBlock;
        [SerializeField] private ItemListBlock _itemListBlock;
        
        string GrowthStageLocalizedName;
        string nextBananaRequestLocalizedName;
        
        private List<int> itemsQuantities = new ();
        private List<string> itemsNames = new ();
        
        public void SetDescription(ItemScriptableObject itemScriptableObject, GameObject targetedGameObject = null) {
            switch (itemScriptableObject.itemCategory) {
                case ItemCategory.BUILDABLE:
                    _itemDescriptionBlock.gameObject.SetActive(true);
                    _itemPreviewBlock.gameObject.SetActive(true);
                    _oneItemSliderBlock.gameObject.SetActive(false);
                    _multiSlidersBlock.gameObject.SetActive(false);
                    _itemListBlock.gameObject.SetActive(true);
                    
                    _itemDescriptionBlock.SetBlock(itemScriptableObject.GetName(), itemScriptableObject.GetDescription(), false);
                    _itemPreviewBlock.SetBlock(itemScriptableObject.itemSprite);

                    var compositionName = LocalizationSettings.StringDatabase.GetTable("UI").GetEntry("composition").GetLocalizedString();
                    
                    itemsQuantities = new List<int>();
                    itemsNames = new List<string>();

                    var rawMaterialsWithQuantities = ObjectsReference.Instance.meshReferenceScriptableObject.buildablePropertiesScriptableObjects[itemScriptableObject.buildableType].rawMaterialsWithQuantity;

                    foreach (var rawMaterialsWithQuantity in rawMaterialsWithQuantities) {
                        itemsQuantities.Add(rawMaterialsWithQuantity.Value);
                        GrowthStageLocalizedName = LocalizationSettings.StringDatabase.GetTable("items").GetEntry(rawMaterialsWithQuantity.Key.ToString().ToLower()).GetLocalizedString();
                        itemsNames.Add(GrowthStageLocalizedName);
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
                        itemScriptableObject.GetDescription(), false);
                    break;
                case ItemCategory.COMMAND_ROOM_PANEL:
                    _itemDescriptionBlock.gameObject.SetActive(true);
                    _itemPreviewBlock.gameObject.SetActive(false);
                    _oneItemSliderBlock.gameObject.SetActive(false);
                    _multiSlidersBlock.gameObject.SetActive(false);
                    _itemListBlock.gameObject.SetActive(false);

                    _itemDescriptionBlock.SetBlock(
                        itemScriptableObject.GetName(),
                        itemScriptableObject.GetDescription(), false);
                    break;
                case ItemCategory.DEBRIS:
                    _itemDescriptionBlock.gameObject.SetActive(true);
                    _itemPreviewBlock.gameObject.SetActive(false);
                    _oneItemSliderBlock.gameObject.SetActive(false);
                    _multiSlidersBlock.gameObject.SetActive(false);
                    _itemListBlock.gameObject.SetActive(false);

                    _itemDescriptionBlock.SetBlock(
                        itemScriptableObject.GetName(),
                        itemScriptableObject.GetDescription(), false);
                    break;
                case ItemCategory.BANANA or ItemCategory.RAW_MATERIAL or ItemCategory.INGREDIENT:
                    _itemDescriptionBlock.gameObject.SetActive(true);
                    _itemPreviewBlock.gameObject.SetActive(true);
                    _oneItemSliderBlock.gameObject.SetActive(false);
                    _multiSlidersBlock.gameObject.SetActive(false);
                    _itemListBlock.gameObject.SetActive(false);
                    
                    _itemDescriptionBlock.SetBlock(
                        itemScriptableObject.GetName(),
                        itemScriptableObject.GetDescription(), false);
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
                        itemScriptableObject.GetDescription(), false);
                    _itemPreviewBlock.SetBlock(itemScriptableObject.itemSprite);

                    var doorProperties = (DoorPropertiesScriptableObject)itemScriptableObject;
                    
                    var mapData =
                        ObjectsReference.Instance.gameData.mapBySceneName[
                            doorProperties.associatedMapPropertiesScriptableObject.sceneName];

                    var visitors = 0; // TODO : get the true value

                    var pollutionName = LocalizationSettings.StringDatabase.GetTable("UI").GetEntry("pollution").GetLocalizedString();
                    var visitorsName = LocalizationSettings.StringDatabase.GetTable("UI").GetEntry("visitors").GetLocalizedString();

                    _multiSlidersBlock.SetBlock(
                        new [] {
                            (mapData.piratesDebris+mapData.visitorsDebris, pollutionName),
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
                        itemScriptableObject.GetDescription(), false);
                    break;
                case ItemCategory.MONKEY:
                    _itemDescriptionBlock.gameObject.SetActive(true);
                    _itemPreviewBlock.gameObject.SetActive(true);
                    _oneItemSliderBlock.gameObject.SetActive(true);
                    _multiSlidersBlock.gameObject.SetActive(false);
                    _itemListBlock.gameObject.SetActive(false);

                    if (targetedGameObject != null) {
                        nextBananaRequestLocalizedName = LocalizationSettings.StringDatabase.GetTable("UI").GetEntry("next_banana_request_in").GetLocalizedString();

                        var monkey = targetedGameObject.GetComponent<Monkey>();

                        _itemDescriptionBlock.SetBlock(
                            itemScriptableObject.GetName(),
                            itemScriptableObject.GetDescription(), 
                            true,
                            nextBananaRequestLocalizedName+monkey.sasietyTimer);
                        _itemPreviewBlock.SetBlock(itemScriptableObject.itemSprite);

                        var monkeyData = (MonkeyPropertiesScriptableObject)itemScriptableObject;
                        
                        _oneItemSliderBlock.SetBlock(
                            nextBananaRequestLocalizedName,
                            (int)monkeyData.sasietyTimer,
                            10);
                    }

                    break;
                case ItemCategory.REGIME:
                    if (targetedGameObject != null) {
                        var regime = targetedGameObject.GetComponent<Regime>();
                        
                        _itemDescriptionBlock.gameObject.SetActive(true);
                        _itemPreviewBlock.gameObject.SetActive(false);
                        _oneItemSliderBlock.gameObject.SetActive(false);
                        _multiSlidersBlock.gameObject.SetActive(false);
                        _itemListBlock.gameObject.SetActive(false);

                        GrowthStageLocalizedName = LocalizationSettings.StringDatabase.GetTable("UI").GetEntry("growth_stage").GetLocalizedString();

                        _itemDescriptionBlock.SetBlock(
                            itemScriptableObject.GetName(),
                            itemScriptableObject.GetDescription(),
                            true,
                            GrowthStageLocalizedName+" : "+regime.regimeStade.ToString().ToLower()
                        );
                    }
                    
                    break;
                case ItemCategory.VISITOR:
                    _itemDescriptionBlock.gameObject.SetActive(true);
                    _itemPreviewBlock.gameObject.SetActive(false);
                    _oneItemSliderBlock.gameObject.SetActive(false);
                    _multiSlidersBlock.gameObject.SetActive(false);
                    _itemListBlock.gameObject.SetActive(false);

                    _itemDescriptionBlock.SetBlock(
                        itemScriptableObject.GetName(),
                        itemScriptableObject.GetDescription(), false);
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
        }
    }
}
