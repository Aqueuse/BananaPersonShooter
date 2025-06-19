using InGame.Inventories;
using InGame.Items.ItemsBehaviours;
using InGame.Items.ItemsData.Characters;
using InGame.Items.ItemsProperties;
using UI.InGame.Merchimps;
using UnityEngine;

namespace InGame.Monkeys.Merchimps {
    public class MerchimpBehaviour : MonoBehaviour {
        public UIMerchantWaitTimer uiMerchantWaitTimer;
        private UIMerchant uiMerchant;

        public MonkeyMenData monkeyMenData;
        private SpaceshipBehaviour associatedSpaceshipBehaviour;
        
        [HideInInspector] public ItemScriptableObject activeItemScriptableObject;

        private BananasInventory bananaManBananasInventory;
        private ManufacturedItemsInventory bananaManManufacturedItemsInventory;
        private IngredientsInventory bananaManIngredientsInventory;

        public void Start() {
            uiMerchant = ObjectsReference.Instance.uiMerchant;

            bananaManBananasInventory = ObjectsReference.Instance.BananaManBananasInventory;
            bananaManManufacturedItemsInventory = ObjectsReference.Instance.bananaManManufacturedItemsInventory;
            bananaManIngredientsInventory = ObjectsReference.Instance.bananaManIngredientsInventory;
            
            uiMerchant.InitializeInventories(monkeyMenData);
            uiMerchant.RefreshMerchantInventories();
            uiMerchant.RefreshBitkongQuantities();
            uiMerchant.Switch_to_Sell_inventory();
            
            StartWaitingTimer();
        }
        
        private int waitTimer;
        
        private void StartWaitingTimer() {
            transform.position = associatedSpaceshipBehaviour.transform.position;
            
            uiMerchantWaitTimer.SetTimer(120);
            waitTimer = 120;
            InvokeRepeating(nameof(DecrementeTimer), 0, 1);
        }
        
        public void DecrementeTimer() {
            waitTimer--;
            if (waitTimer <= 0) {
                transform.position = associatedSpaceshipBehaviour.transform.position;
                CancelInvoke(nameof(DecrementeTimer));
                associatedSpaceshipBehaviour.StopWaiting();
            }
            
            uiMerchantWaitTimer.SetTimer(waitTimer);
        }
        
        public int GetMerchantItemQuantity(ItemScriptableObject itemScriptableObject) {
            switch (itemScriptableObject.itemCategory) {
                case ItemCategory.INGREDIENT:
                    return monkeyMenData.ingredientsInventory[itemScriptableObject.ingredientsType];

                case ItemCategory.MANUFACTURED_ITEM:
                    return monkeyMenData.manufacturedItemsInventory[itemScriptableObject.manufacturedItemsType];

                case ItemCategory.DROPPED:
                    return monkeyMenData.rawMaterialsInventory[itemScriptableObject.rawMaterialType];
            }

            return 0;
        }

        public int GetBananaManItemQuantity(ItemScriptableObject itemScriptableObject) {
            switch (itemScriptableObject.itemCategory) {
                case ItemCategory.BANANA:
                    return bananaManBananasInventory.bananasInventory[itemScriptableObject.bananaType];

                case ItemCategory.MANUFACTURED_ITEM:
                    return bananaManManufacturedItemsInventory.manufacturedItemsInventory[itemScriptableObject.manufacturedItemsType];

                case ItemCategory.INGREDIENT:
                    return bananaManIngredientsInventory.ingredientsInventory[itemScriptableObject.ingredientsType];
            }

            return 0;
        }
    }
}