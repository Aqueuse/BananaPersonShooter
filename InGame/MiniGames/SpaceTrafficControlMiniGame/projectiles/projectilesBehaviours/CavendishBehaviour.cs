using UnityEngine;

namespace InGame.MiniGames.SpaceTrafficControlMiniGame.projectiles.projectilesBehaviours {
    public class CavendishBehaviour : MonoBehaviour {
        public Vector3 projectileScale;
        private Transform _projectileTransform;

        float _bulletCounter = 3f;

        private void Awake() {
            projectileScale = new Vector3(1, 1, 0);
            _projectileTransform = GetComponent<Transform>();
        }
        
        private void Update() {
            projectileScale = transform.localScale;
            projectileScale.z += 0.01f;
            _projectileTransform.localScale = projectileScale;
            
            _bulletCounter -= Time.deltaTime;
            
            if (_bulletCounter < 0) {
                GetComponent<Projectile>().Destroy();
            }
        }

        public void Pew() {
            projectileScale = transform.localScale;
            projectileScale.z = 0;
            _projectileTransform.localScale = projectileScale;
        }
    }
}
