using Monkeys;
using Player;
using UI.InGame;
using UnityEngine;
using UnityEngine.AI;

namespace Weapons {
    public class Weapon : MonoBehaviour {
        public BananasDataScriptableObject bananasDataScriptableObject;

        private void OnTriggerEnter(Collider other) {
            if (GameManager.Instance.isFigthing && other.gameObject.CompareTag("Boss")) {
                MonkeyManager.Instance.GetActiveBoss().GetComponent<Monkey>().AddSatiety(BananaMan.Instance.activeItem.damage);
                DestroyMe();
            }
        }

        private void OnCollisionEnter(Collision collision) {
            if (collision.gameObject.CompareTag("Player")) {
                BananaMan.Instance.resistance += 1;
                UIVitals.Instance.Set_Resistance(BananaMan.Instance.resistance);
                DestroyMe();
            }
            
            else {
                transform.gameObject.GetComponent<MeshRenderer>().enabled = false;
                transform.GetChild(0).GetComponent<MeshRenderer>().enabled = true;

                MonkeyManager.Instance.GetActiveBoss().GetComponent<NavMeshAgent>().SetDestination(transform.position);
            
                Invoke(nameof(DestroyMe), 10);
            }
        }

        private void DestroyMe() {
            Destroy(transform.gameObject);
        }
    }
}
