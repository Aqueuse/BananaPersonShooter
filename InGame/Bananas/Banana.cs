using InGame.Items.ItemsProperties.Bananas;
using InGame.Monkeys.Chimpirates;
using Tags;
using UnityEngine;

namespace InGame.Bananas {
    public class Banana : MonoBehaviour {
        [SerializeField] private GameObject bananaSkin;
        [SerializeField] private MeshRenderer bananaMeshRenderer;
        
        [SerializeField] private LayerMask bananaSplashLayerMask;
        
        public BananasPropertiesScriptableObject bananasDataScriptableObject;

        private void Start() {
            Invoke(nameof(DestroyMe), 10);
        }

        private void OnCollisionEnter(Collision collision) {
            if (bananaSplashLayerMask == (bananaSplashLayerMask | 1 << collision.gameObject.layer)) {
                // trasnformation en peau de banane
                bananaMeshRenderer.enabled = false;
                bananaSkin.SetActive(true);

                Invoke(nameof(DestroyMe), 10);
                return;
            }
            
            Invoke(nameof(DestroyMe), 10);
        }

        private void OnTriggerEnter(Collider other) {
            if (TagsManager.Instance.HasTag(other.gameObject, GAME_OBJECT_TAG.PLAYER)) {
                ObjectsReference.Instance.rawMaterialsInventory.AddQuantity(RawMaterialType.BANANA_PEEL, 1);
                DestroyMe();
            }

            if (TagsManager.Instance.HasTag(other.gameObject, GAME_OBJECT_TAG.PIRATE)) {
                other.transform.GetComponent<PirateBehaviour>().Flee();
            }
        }

        private void DestroyMe() {
            Destroy(transform.gameObject);
        }
    }
}
