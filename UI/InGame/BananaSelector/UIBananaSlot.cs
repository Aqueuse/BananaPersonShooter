using InGame.Items.ItemsProperties.Bananas;
using TMPro;
using UnityEngine;

namespace UI.InGame.BananaSelector {
    public class UIBananaSlot : MonoBehaviour {
        [SerializeField] private TextMeshProUGUI bananaQuantity;
        public BananasPropertiesScriptableObject bananasPropertiesScriptableObject;

        public void SetQuantity(int newQuantity) {
            bananaQuantity.text = newQuantity.ToString();
        }
    
        public void SetActive() {
            ObjectsReference.Instance.bananaMan.SetActiveItem(bananasPropertiesScriptableObject);
        }
    }
}
