using InGame.Monkeys.Merchimps;
using UnityEngine;

namespace InGame.Interactions.InteractionsActions {
    public class MerchantInteraction : Interaction {
        public override void Activate(GameObject interactedGameObject) {
            ObjectsReference.Instance.cameraPlayer.Set0Sensibility();
            ObjectsReference.Instance.bananaGunActionsSwitch.gameObject.SetActive(false);
            ObjectsReference.Instance.gameManager.gameContext = GameContext.MERCHANT;
            
            ObjectsReference.Instance.uiMerchant.ShowMerchant(interactedGameObject.transform.parent.parent.GetComponent<MerchimpBehaviour>());
            
            ObjectsReference.Instance.inputManager.SwitchContext(InputContext.MERCHANT);
        }
    }
    
}