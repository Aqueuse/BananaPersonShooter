using Monkeys;
using Player;
using UI.InGame;
using UnityEngine;
using UnityEngine.AI;

namespace Bananas {
    public class Banana : MonoBehaviour {
        public BananasDataScriptableObject bananasDataScriptableObject;

        private void OnTriggerEnter(Collider other) {
            if (GameManager.Instance.isFigthing && other.gameObject.CompareTag("Boss")) {
                MonkeyManager.Instance.GetActiveBoss().GetComponent<Monkey>().AddSatiety(BananaMan.Instance.activeItem.damage);
                DestroyMe();
            }
        }

        private void OnCollisionEnter(Collision collision) {
            if (collision.gameObject.layer == LayerMask.NameToLayer("Terrain")) {  
                // trasnformation en peau de banane
                transform.gameObject.GetComponent<MeshRenderer>().enabled = false;
                transform.GetChild(0).GetComponent<MeshRenderer>().enabled = true;

                if (GameManager.Instance.isInCorolle) {
                    MonkeyManager.Instance.GetActiveBoss().GetComponent<NavMeshAgent>().SetDestination(transform.position);
                }
                
                Invoke(nameof(DestroyMe), 10);
            }

            else {
                if (!collision.gameObject.CompareTag("Player")) {
                    BananaMan.Instance.resistance += 1;
                    UIVitals.Instance.Set_Resistance(BananaMan.Instance.resistance);
                    DestroyMe();
                }
            }
        }

        private void DestroyMe() {
            Destroy(transform.gameObject);
        }
    }
}
