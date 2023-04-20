using Building;
using Data.Bananas;
using Enums;
using Monkeys;
using UnityEngine;
using UnityEngine.AI;

namespace Bananas {
    public class Banana : MonoBehaviour {
        [SerializeField] private GameObject bananaSkin;
        public BananasDataScriptableObject bananasDataScriptableObject;

        private void OnCollisionEnter(Collision collision) {
            if (collision.gameObject.CompareTag("Boss")) {
                collision.gameObject.GetComponent<Monkey>().Feed(bananasDataScriptableObject.sasiety);
                DestroyMe();
            }

            else {
                if (collision.gameObject.layer == LayerMask.NameToLayer("Terrain")) {  
                    // trasnformation en peau de banane
                    transform.gameObject.GetComponent<MeshRenderer>().enabled = false;
                    bananaSkin.SetActive(true);

                    if (ObjectsReference.Instance.mapsManager.currentMap.activeMonkeyType != MonkeyType.NONE) {
                        foreach (var monkey in MapItems.Instance.monkeys) {
                            monkey.GetComponent<NavMeshAgent>().SetDestination(transform.position);
                        }
                    }

                    Invoke(nameof(DestroyMe), 10);
                }

                else {
                    if (collision.gameObject.CompareTag("Player")) {
                        ObjectsReference.Instance.inventory.AddQuantity(ItemCategory.RAW_MATERIAL, ItemType.BANANA_SKIN, 1);
                        DestroyMe();
                    }

                    else {
                        Invoke(nameof(DestroyMe), 10);
                    }
                }
            }
        }

        private void DestroyMe() {
            Destroy(transform.gameObject);
        }
    }
}
