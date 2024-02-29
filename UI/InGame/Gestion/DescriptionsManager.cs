using System.Collections.Generic;
using InGame.Interactions;
using InGame.Items.ItemsProperties;
using InGame.Items.ItemsProperties.Monkeys;
using InGame.Monkeys.Ancestors;
using JetBrains.Annotations;
using UI.InGame.Gestion.blocks;
using UnityEngine;
using UnityEngine.Localization.Settings;

namespace UI.InGame.Gestion {
    public class DescriptionsManager : MonoBehaviour {
        private ItemScriptableObject scriptableObject;

        [SerializeField] private ItemDescriptionBlock _itemDescriptionBlock;
        [SerializeField] private ItemOneLineDescriptionBlock _itemOneLineDescriptionBlock;
        [SerializeField] private ItemPreviewBlock _itemPreviewBlock;
        [SerializeField] private OneItemSliderBlock _oneItemSliderBlock;
        [SerializeField] private ItemMultiSlidersBlock _multiSlidersBlock;
        [SerializeField] private ItemListBlock _itemListBlock;
        
        string GrowthStageLocalizedName;
        string nextBananaRequestLocalizedName;
        private string marketValue;
        [CanBeNull] private string compositionName;
        
        private List<int> itemsQuantities = new ();
        private List<string> itemsNames = new ();

        private void Start() {
            compositionName = LocalizationSettings.StringDatabase.GetTable("UI").GetEntry("composition").GetLocalizedString();
            marketValue = LocalizationSettings.StringDatabase.GetTable("UI").GetEntry("market_value").GetLocalizedString();
        }

        public void SetDescription(ItemScriptableObject itemScriptableObject, GameObject targetedGameObject = null) {
            switch (itemScriptableObject.itemCategory) {
                case ItemCategory.BUILDABLE:
                    _itemDescriptionBlock.gameObject.SetActive(true);
                    _itemPreviewBlock.gameObject.SetActive(true);
                    _oneItemSliderBlock.gameObject.SetActive(false);
                    _multiSlidersBlock.gameObject.SetActive(false);
                    _itemListBlock.gameObject.SetActive(true);
                    _itemOneLineDescriptionBlock.gameObject.SetActive(false);
                    
                    _itemDescriptionBlock.SetBlock(itemScriptableObject.GetName(), itemScriptableObject.GetDescription(), false);
                    _itemPreviewBlock.SetBlock(itemScriptableObject.itemSprite);
                    
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
                case ItemCategory.COMMAND_ROOM_PANEL:
                    _itemDescriptionBlock.gameObject.SetActive(true);
                    _itemPreviewBlock.gameObject.SetActive(false);
                    _oneItemSliderBlock.gameObject.SetActive(false);
                    _multiSlidersBlock.gameObject.SetActive(false);
                    _itemListBlock.gameObject.SetActive(false);
                    _itemOneLineDescriptionBlock.gameObject.SetActive(false);

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
                    _itemOneLineDescriptionBlock.gameObject.SetActive(false);

                    _itemDescriptionBlock.SetBlock(
                        itemScriptableObject.GetName(),
                        itemScriptableObject.GetDescription(), false);
                    break;
                case ItemCategory.BANANA or ItemCategory.RAW_MATERIAL or ItemCategory.INGREDIENT or ItemCategory.MANUFACTURED_ITEM:
                    _itemDescriptionBlock.gameObject.SetActive(true);
                    _itemPreviewBlock.gameObject.SetActive(true);
                    _oneItemSliderBlock.gameObject.SetActive(false);
                    _multiSlidersBlock.gameObject.SetActive(false);
                    _itemListBlock.gameObject.SetActive(false);
                    _itemOneLineDescriptionBlock.gameObject.SetActive(true);
                    
                    _itemDescriptionBlock.SetBlock(
                        itemScriptableObject.GetName(),
                        itemScriptableObject.GetDescription(), false);
                    _itemOneLineDescriptionBlock.SetBlock(marketValue, itemScriptableObject.bitKongValue+" BTK");
                    _itemPreviewBlock.SetBlock(itemScriptableObject.itemSprite);
                    break;
                
                case ItemCategory.CHIMPLOYEE or ItemCategory.MINI_CHIMP or ItemCategory.MERCHANT:
                    _itemDescriptionBlock.gameObject.SetActive(true);
                    _itemPreviewBlock.gameObject.SetActive(true);
                    _oneItemSliderBlock.gameObject.SetActive(false);
                    _multiSlidersBlock.gameObject.SetActive(false);
                    _itemListBlock.gameObject.SetActive(false);
                    _itemOneLineDescriptionBlock.gameObject.SetActive(false);
                    
                    _itemDescriptionBlock.SetBlock(
                        itemScriptableObject.GetName(),
                        itemScriptableObject.GetDescription(), false);
                    _itemPreviewBlock.SetBlock(itemScriptableObject.itemSprite);
                    break;
                
                case ItemCategory.MONKEY:
                    _itemDescriptionBlock.gameObject.SetActive(true);
                    _itemPreviewBlock.gameObject.SetActive(true);
                    _oneItemSliderBlock.gameObject.SetActive(true);
                    _multiSlidersBlock.gameObject.SetActive(false);
                    _itemListBlock.gameObject.SetActive(false);
                    _itemOneLineDescriptionBlock.gameObject.SetActive(false);

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
                        _itemOneLineDescriptionBlock.gameObject.SetActive(false);

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
                    _itemOneLineDescriptionBlock.gameObject.SetActive(false);

                    _itemDescriptionBlock.SetBlock(
                        itemScriptableObject.GetName(),
                        itemScriptableObject.GetDescription(), false);
//                    _multiSlidersBlock.SetBlock();
                    break;
            }
        }

        public void HideAllPanels() {
            _itemDescriptionBlock.gameObject.SetActive(false);
            _itemOneLineDescriptionBlock.gameObject.SetActive(false);
            _itemPreviewBlock.gameObject.SetActive(false);
            _oneItemSliderBlock.gameObject.SetActive(false);
            _multiSlidersBlock.gameObject.SetActive(false);
            _itemListBlock.gameObject.SetActive(false);
        }
    }
}
