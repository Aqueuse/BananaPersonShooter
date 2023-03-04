using Enums;
using Game;
using Monkeys;
using Player;
using UnityEngine;
using UnityEngine.AI;

namespace Bananas {
    public class Banana : MonoBehaviour {
        public BananasDataScriptableObject bananasDataScriptableObject;

        private void OnTriggerEnter(Collider other) {
            if (other.gameObject.CompareTag("Boss")) {
                MapsManager.Instance.currentMap.GetActiveMonkey().GetComponent<Monkey>().Feed(BananaMan.Instance.activeItem.sasiety);
                DestroyMe();
            }
        }

        private void OnCollisionEnter(Collision collision) {
            if (collision.gameObject.layer == LayerMask.NameToLayer("Terrain")) {  
                // trasnformation en peau de banane
                transform.gameObject.GetComponent<MeshRenderer>().enabled = false;
                transform.GetChild(0).GetComponent<MeshRenderer>().enabled = true;

                if (MapsManager.Instance.currentMap.activeMonkeyType != MonkeyType.NONE) {
                    MapsManager.Instance.currentMap.GetActiveMonkey().GetComponent<NavMeshAgent>().SetDestination(transform.position);
                }
                
                Invoke(nameof(DestroyMe), 10);
            }

            else {
                if (!collision.gameObject.CompareTag("Player")) {
                    BananaMan.Instance.resistance += 1;
                    DestroyMe();
                }
            }
        }

        private void DestroyMe() {
            Destroy(transform.gameObject);
        }
    }
}
