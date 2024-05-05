using InGame.Items.ItemsProperties;
using UnityEngine;

namespace UI.InGame.Merchimps {
    public class UIMerchantSlot : MonoBehaviour {
        private ItemScriptableObject itemScriptableObject;
        
        public void SetDescriptionAndName() {
            ObjectsReference.Instance.uiDescriptionsManager.SetDescription(itemScriptableObject);
        }
    }
}
