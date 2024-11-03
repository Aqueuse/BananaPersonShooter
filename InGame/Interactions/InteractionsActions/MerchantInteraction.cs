using InGame.Monkeys;
using InGame.Monkeys.Merchimps;
using UnityEngine;

namespace InGame.Interactions.InteractionsActions {
    public class MerchantInteraction : Interaction {
        public override void Activate(GameObject interactedGameObject) {
            ObjectsReference.Instance.uiManager.SetActive(UICanvasGroupType.MERCHANT_INTERFACE, true);

            ObjectsReference.Instance.uiMerchant.merchimpBehaviour = interactedGameObject.GetComponent<MerchimpBehaviour>();
            ObjectsReference.Instance.uiMerchant.InitializeInventories(interactedGameObject.GetComponent<MonkeyMenBehaviour>().monkeyMenData);

            ObjectsReference.Instance.inputManager.SwitchContext(InputContext.UI);
        }
    }
}
