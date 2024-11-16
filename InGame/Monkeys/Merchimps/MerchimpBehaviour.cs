using InGame.Items.ItemsProperties;
using UI.InGame.Merchimps;
using UnityEngine;

namespace InGame.Monkeys.Merchimps {
    public class MerchimpBehaviour : MonoBehaviour {
        public MonkeyMenBehaviour monkeyMenBehaviour;
        public UIMerchantWaitTimer uiMerchantWaitTimer;
        private UIMerchant uiMerchant;

        [HideInInspector] public ItemScriptableObject activeItemScriptableObject;

        public void Start() {
            monkeyMenBehaviour = GetComponent<MonkeyMenBehaviour>();
            uiMerchant = ObjectsReference.Instance.uiMerchant;
            
            uiMerchant.InitializeInventories(monkeyMenBehaviour.monkeyMenData);
            uiMerchant.RefreshMerchantInventories();
            uiMerchant.RefreshBitkongQuantities();
            uiMerchant.Switch_to_Sell_inventory();
            
            StartWaitingTimer();
        }
        
        private int waitTimer;
        
        private void StartWaitingTimer() {
            transform.position = monkeyMenBehaviour.associatedSpaceshipBehaviour.transform.position;
            
            uiMerchantWaitTimer.SetTimer(120);
            waitTimer = 120;
            InvokeRepeating(nameof(DecrementeTimer), 0, 1);
        }
        
        public void DecrementeTimer() {
            waitTimer--;
            if (waitTimer <= 0) {
                transform.position = monkeyMenBehaviour.associatedSpaceshipBehaviour.transform.position;
                CancelInvoke(nameof(DecrementeTimer));
                monkeyMenBehaviour.associatedSpaceshipBehaviour.StopWaiting();
            }
            
            uiMerchantWaitTimer.SetTimer(waitTimer);
        }
        
        public int GetMerchantItemQuantity(ItemScriptableObject itemScriptableObject) {
            switch (itemScriptableObject.itemCategory) {
                case ItemCategory.INGREDIENT:
                    return monkeyMenBehaviour.monkeyMenData.ingredientsInventory[itemScriptableObject.ingredientsType];

                case ItemCategory.MANUFACTURED_ITEM:
                    return monkeyMenBehaviour.monkeyMenData.manufacturedItemsInventory[itemScriptableObject.manufacturedItemsType];

                case ItemCategory.DROPPED:
                    return monkeyMenBehaviour.monkeyMenData.droppedInventory[itemScriptableObject.droppedType];
            }

            return 0;
        }

        public int GetBananaManItemQuantity(ItemScriptableObject itemScriptableObject) {
            switch (itemScriptableObject.itemCategory) {
                case ItemCategory.BANANA:
                    return ObjectsReference.Instance.bananaMan.bananaManData.bananasInventory[itemScriptableObject.bananaType];

                case ItemCategory.MANUFACTURED_ITEM:
                    return ObjectsReference.Instance.bananaMan.bananaManData.manufacturedItemsInventory[itemScriptableObject.manufacturedItemsType];

                case ItemCategory.INGREDIENT:
                    return ObjectsReference.Instance.bananaMan.bananaManData.ingredientsInventory[itemScriptableObject.ingredientsType];
            }

            return 0;
        }
    }
}