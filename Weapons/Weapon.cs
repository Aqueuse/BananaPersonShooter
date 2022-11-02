using Bosses;
using Player;
using UI.InGame;
using UnityEngine;

namespace Weapons {
    public class Weapon : MonoBehaviour {
        public BananasDataScriptableObject bananasDataScriptableObject;

        private void OnTriggerEnter(Collider other) {
            if (GameManager.Instance.isBossFigthing && other.gameObject.CompareTag("Boss")) {
                BossManager.Instance.GetGorillaBoss().GetComponent<Boss>().AddSatiety(BananaMan.Instance.activeBanana.damage);
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
            
                Invoke(nameof(DestroyMe), 10);
            }
        }

        private void DestroyMe() {
            Destroy(transform.gameObject);
        }
    }
}
