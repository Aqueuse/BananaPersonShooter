using InGame.Monkeys.Merchimps;
using UnityEngine;

namespace InGame.Interactions.InteractionsActions {
    public class MerchantInteraction : Interaction {
        public override void Activate(GameObject interactedGameObject) {
            ObjectsReference.Instance.chimpManager.merchimpsManager.activeMerchimp = interactedGameObject.GetComponentInParent<Merchimp>();
            ObjectsReference.Instance.chimpManager.merchimpsManager.activeMerchimp.Init();

            ObjectsReference.Instance.uiManager.SetActive(UICanvasGroupType.MERCHANT_INTERFACE, true);
        
            ObjectsReference.Instance.inputManager.SwitchContext(InputContext.UI);
        }
    }
}
