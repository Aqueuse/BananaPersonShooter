using InGame.MiniGames.SpaceTrafficControlMiniGame.projectiles;
using InGame.MiniGames.SpaceTrafficControlMiniGame.projectiles.projectilesBehaviours;
using UnityEngine;

namespace InGame.MiniGames.SpaceTrafficControlMiniGame.Spaceships {
    public class Cannon : MonoBehaviour {
        public Transform launcher;
        [SerializeField] private ProjectilesPool projectilesPool;
        
        private BananaType _projectileType;
        private Color _projectileColor;
        private Projectile _projectile;
        
        public void Shoot() {
            _projectile = projectilesPool.Get_projectile();

            _projectileType = ObjectsReference.Instance.bananaMan.activeBanana.bananaType;
            _projectileColor = ObjectsReference.Instance.meshReferenceScriptableObject
                .bananasPropertiesScriptableObjects[_projectileType].bananaColor;
            
            _projectile.SetColor(_projectileColor);
            
            var projectileTransform = _projectile.transform;
            projectileTransform.position = launcher.position;

            var cannonLauncherRotation = launcher.rotation;
            projectileTransform.rotation = cannonLauncherRotation;
            
            switch (_projectileType) {
                case BananaType.CAVENDISH:
                    if (_projectile.gameObject.GetComponent<CavendishBehaviour>() != null) {
                        Destroy(_projectile.gameObject.GetComponent<CavendishBehaviour>());
                    }
                    _projectile.gameObject.AddComponent<CavendishBehaviour>();
                    _projectile.GetComponent<CavendishBehaviour>().Pew();
                    break;
            }
        }
    }
}
