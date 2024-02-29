using InGame.MiniGames.SpaceTrafficControlMiniGame.Spaceships;
using Tags;
using UnityEngine;

namespace InGame.MiniGames.SpaceTrafficControlMiniGame.debris {
    public class SpaceshipDebris : MonoBehaviour {
        [SerializeField] private Spaceship2D spaceship2D;
        
        private void OnTriggerEnter(Collider other) {
            if (!spaceship2D.isExploded) return;
            
            if (TagsManager.Instance.HasTag(other.gameObject, GAME_OBJECT_TAG.CANNON_MINI_GAME_DOME)) {
                SpawnDebrisOnMap();
            }
            
            if (TagsManager.Instance.HasTag(other.gameObject, GAME_OBJECT_TAG.CANNON_MINI_GAME_LIMITS)) Destroy(gameObject);
        }
        
        private void SpawnDebrisOnMap() {
            if (spaceship2D.spaceshipType == CharacterType.VISITOR)
                ObjectsReference.Instance.spaceTrafficControlMiniGameManager.AddVisitorDebris();
            
            if (spaceship2D.spaceshipType == CharacterType.PIRATE)
                ObjectsReference.Instance.spaceTrafficControlMiniGameManager.AddPirateDebris();
            
            if (spaceship2D.spaceshipType == CharacterType.MERCHIMP)
                ObjectsReference.Instance.spaceTrafficControlMiniGameManager.AddMerchantDebris();
            
            Destroy(gameObject);
        }
    }
}
