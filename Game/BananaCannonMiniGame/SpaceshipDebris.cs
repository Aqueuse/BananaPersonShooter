using UnityEngine;

namespace Game.BananaCannonMiniGame {
    public class SpaceshipDebris : MonoBehaviour {
        [SerializeField] private Spaceship spaceship;
        
        private void OnTriggerEnter(Collider other) {
            if (!spaceship.isExploded) return;
            
            if (other.CompareTag("banana_cannon_game_dome")) {
                SpawnDebrisOnMap();
            }
            
            if (other.CompareTag("miniGameLimits")) Destroy(gameObject);
        }
        
        private void SpawnDebrisOnMap() {
            BananaCannonMiniGameManager.Instance.SpawnDebrisOnMap(transform.position);
            Destroy(gameObject);
        }
    }
}
