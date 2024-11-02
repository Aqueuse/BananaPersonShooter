using InGame.Items.ItemsProperties.Bananas;
using InGame.Monkeys.Chimpirates;
using Tags;
using UnityEngine;

namespace InGame.Bananas {
    public class Banana : MonoBehaviour {
        [SerializeField] private MeshRenderer bananaMeshRenderer;
        [SerializeField] private GameObject bananaSkin;

        public BananasPropertiesScriptableObject bananasDataScriptableObject;

        private void OnCollisionEnter (Collision collision) {
            if (TagsManager.Instance.HasTag(collision.gameObject, GAME_OBJECT_TAG.PIRATE)) {
                collision.transform.GetComponent<PirateBehaviour>().Flee();
            }

            if (TagsManager.Instance.HasTag(collision.gameObject, GAME_OBJECT_TAG.PLAYER)) {
                ObjectsReference.Instance.droppedInventory.AddQuantity(DroppedType.BANANA_PEEL, 1);
                DestroyMe();
            }

            else {
                if (!bananaSkin.activeInHierarchy) ConvertToBananaSkin();
            }
        }
        
        private void ConvertToBananaSkin() {
            bananaMeshRenderer.enabled = false;
            bananaSkin.SetActive(true);
            
            Invoke(nameof(DestroyMe), 10);
        }

        public void DestroyMe() {
            Destroy(gameObject);
        }
    }
}
