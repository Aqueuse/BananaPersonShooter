using UI.InGame.Merchimps;

namespace InGame.Monkeys.Merchimps {
    public class MerchimpBehaviour : MonkeyMenBehaviour {
        public UIMerchantWaitTimer uiMerchantWaitTimer;
        
        private UIMerchant uiMerchant;
        
        public void StartToSell() {
            uiMerchant = ObjectsReference.Instance.uiMerchant;
            ObjectsReference.Instance.chimpManager.merchimpsManager.activeMerchimpBehaviour = this;
            
            uiMerchant.InitializeInventories(monkeyMenData);
            uiMerchant.RefreshMerchantInventories();
            uiMerchant.RefreshBitkongQuantities();
            uiMerchant.Switch_to_Sell_inventory();
        }

        public override void LoadFromSavedData() {
            if (!monkeyMenData.isInSpaceship) SetColors();
        }

        public override void GenerateSavedData() {
            monkeyMenSavedData.characterType = CharacterType.MERCHIMP;
            
            monkeyMenSavedData.ingredientsInventory = monkeyMenData.ingredientsInventory;
            monkeyMenSavedData.bananasInventory = monkeyMenData.bananasInventory;
            monkeyMenSavedData.manufacturedItemsInventory = monkeyMenData.manufacturedItemsInventory;
            monkeyMenSavedData.droppedInventory = monkeyMenData.droppedInventory;

            monkeyMenSavedData.bitKongQuantity = monkeyMenData.bitKongQuantity;
            
            monkeyMenSavedData.uid = monkeyMenData.uid;
            monkeyMenSavedData.name = monkeyMenData.monkeyMenName;
        }
    }
}