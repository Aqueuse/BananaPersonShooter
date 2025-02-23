using InGame.Items.ItemsProperties;
using TMPro;
using UnityEngine;

namespace InGame.Items {
    public class ItemStack : MonoBehaviour {
        public ItemScriptableObject itemScriptableObject;
        [SerializeField] TextMeshProUGUI quantityText;
        [SerializeField] private MeshRenderer itemMeshRenderer;

        private int quantity;

        public void AddOne() {
            itemMeshRenderer.enabled = true;
            quantity += 1;
            quantityText.text = quantity.ToString();
            quantityText.alpha = 1;
        }

        public void ThrowOne() {
            if (quantity == 0) return;

            Instantiate(
                itemScriptableObject.prefab,
                transform.position + Vector3.forward,
                Quaternion.identity,
                ObjectsReference.Instance.gameSave.savablesItemsContainer
            );
            
            quantity -= 1;
            quantityText.text = quantity.ToString();

            if (quantity <= 0) {
                itemMeshRenderer.enabled = false;
                itemMeshRenderer.gameObject.layer = 0;
                quantityText.alpha = 0;
            }
        }
    }
}
