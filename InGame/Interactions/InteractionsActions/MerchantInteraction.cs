using InGame.Monkeys.Merchimps;
using UnityEngine;

namespace InGame.Interactions.InteractionsActions {
    public class MerchantInteraction : Interaction {
        public override void Activate(GameObject interactedGameObject) {
            ObjectsReference.Instance.cameraPlayer.Set0Sensibility();
            ObjectsReference.Instance.bananaGunActionsSwitch.gameObject.SetActive(false);
            ObjectsReference.Instance.gameManager.gameContext = GameContext.IN_GAME_UI_PANEL;
            
            ObjectsReference.Instance.uiManager.SetActive(UICanvasGroupType.MERCHANT_INTERFACE, true);

            ObjectsReference.Instance.uiMerchant.merchimpBehaviour =
                interactedGameObject.transform.parent.parent.GetComponent<MerchimpBehaviour>(); 
            
            ObjectsReference.Instance.uiMerchant.InitializeInventories();
            ObjectsReference.Instance.uiMerchant.RefreshBitkongQuantities();
            ObjectsReference.Instance.uiMerchant.Switch_to_Sell_inventory();
            
            ObjectsReference.Instance.inputManager.SwitchContext(InputContext.UI);
        }
    }
}