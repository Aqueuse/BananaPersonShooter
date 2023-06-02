using Building;
using Data.Bananas;
using Monkeys;
using UnityEngine;
using UnityEngine.AI;

namespace Bananas {
    public class Banana : MonoBehaviour {
        [SerializeField] private GameObject bananaSkin;
        [SerializeField] private MeshRenderer bananaMeshRenderer;

        public BananasDataScriptableObject bananasDataScriptableObject;

        private void OnCollisionEnter(Collision collision) {
            if (collision.gameObject.CompareTag("Boss")) {
                collision.gameObject.GetComponent<Monkey>().Feed(bananasDataScriptableObject.sasiety);
                DestroyMe();
                return;
            }

            if (
                collision.gameObject.layer == LayerMask.NameToLayer("Default") || 
                collision.gameObject.layer == LayerMask.NameToLayer("Aspirable") ||
                collision.gameObject.layer == LayerMask.NameToLayer("Items")) {  
                // trasnformation en peau de banane
                bananaMeshRenderer.enabled = false;
                bananaSkin.SetActive(true);

                if (ObjectsReference.Instance.mapsManager.currentMap.activeMonkeyType != MonkeyType.NONE) {
                    foreach (var monkey in MapItems.Instance.monkeys) {
                        monkey.GetComponent<NavMeshAgent>().SetDestination(transform.position);
                    }
                }

                Invoke(nameof(DestroyMe), 10);
                return;
            }

            if (collision.gameObject.CompareTag("Player")) {
                ObjectsReference.Instance.inventory.AddQuantity(ItemCategory.RAW_MATERIAL, ItemType.BANANA_PEEL, 1);
                DestroyMe();
                return;
            }

            Invoke(nameof(DestroyMe), 10);
        }

        private void DestroyMe() {
            Destroy(transform.gameObject);
        }
    }
}
