using Tags;
using UnityEngine;

namespace Game.BananaCannonMiniGame {
    public class SpaceshipDebris : MonoBehaviour {
        [SerializeField] private Spaceship spaceship;
        
        private void OnTriggerEnter(Collider other) {
            if (!spaceship.isExploded) return;
            
            if (TagsManager.Instance.HasTag(other.gameObject, GAME_OBJECT_TAG.CANNON_MINI_GAME_DOME)) {
                SpawnDebrisOnMap();
            }
            
            if (TagsManager.Instance.HasTag(other.gameObject, GAME_OBJECT_TAG.CANNON_MINI_GAME_LIMITS)) Destroy(gameObject);
        }
        
        private void SpawnDebrisOnMap() {
            if (spaceship.spaceshipType == CharacterType.VISITOR)
                BananaCannonMiniGameManager.Instance.AddVisitorDebris();
            
            if (spaceship.spaceshipType == CharacterType.PIRATE)
                BananaCannonMiniGameManager.Instance.AddPirateDebris();
            
            Destroy(gameObject);
        }
    }
}
