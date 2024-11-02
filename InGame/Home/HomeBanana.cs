using UnityEngine;

namespace InGame.Home {
    public class HomeBanana : MonoBehaviour {
        [SerializeField] private GameObject bananaSkin;
        [SerializeField] private MeshRenderer bananaMeshRenderer;
        
        [SerializeField] private LayerMask bananaSplashLayerMask;

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
        
        private void DestroyMe() {
            Destroy(gameObject);
        }
    }
}
