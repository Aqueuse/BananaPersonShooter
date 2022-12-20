using Cameras;
using Player;
using UnityEngine;

namespace Bosses.BossEffects {
    public class AttackWaves : MonoBehaviour {
        [SerializeField] private float speed;

        private Vector3 _initialScale;
        private Vector3 _finalScale;

        private float _scaleTime;

        private void Start() {
            _initialScale = transform.localScale;
            _finalScale = new Vector3(_initialScale.x * 150,_initialScale.y, _initialScale.z * 150);
            if (Camera.main != null) Camera.main.GetComponentInParent<CameraShake>().Shake(0.5f, 0.1f);

            Invoke(nameof(DestroyMe), 2f);
        }
        
        private void Update() {
            transform.localScale = Vector3.Lerp(_initialScale, _finalScale, _scaleTime);
            _scaleTime += Time.deltaTime * speed;
        }

        private void DestroyMe() {
            Destroy(gameObject);
        }

        private void OnTriggerEnter(Collider other) {
            if (other.CompareTag("Player")) {
                BananaMan.Instance.GetComponent<PlayerController>().PlayerRagdollAgainstCollider(GetComponent<MeshCollider>(), 100);
            }
        }
    }
}

