using Data.Bananas;
using Game.BananaCannonMiniGame.projectilesBehaviours;
using UnityEngine;

namespace Game.BananaCannonMiniGame {
    public class ProjectilesManager : MonoBehaviour {
        [SerializeField] private Transform cannonLauncherTransform;
        [SerializeField] private GenericDictionary<ItemType, BananasDataScriptableObject> bananasDataScriptableObjectsByBananaType;

        private ProjectilesPool _projectilesPool;

        private Projectile _projectile;

        private ItemType _projectileType;
        private Color _projectileColor;
        
        private void Start() {
            _projectilesPool = GetComponent<ProjectilesPool>();
            
            SwitchBanana(ItemType.CAVENDISH);
        }

        private void SwitchBanana(ItemType bananaType) {
            _projectileType = bananaType;
            _projectileColor = bananasDataScriptableObjectsByBananaType[bananaType].bananaMaterial.color;
        }

        private static void SetBehaviour(Projectile projectile, ItemType bananaType) {
            switch (bananaType) {
                case ItemType.CAVENDISH:
                    if (projectile.gameObject.GetComponent<CavendishBehaviour>() != null) {
                        Destroy(projectile.gameObject.GetComponent<CavendishBehaviour>());
                    }
                    projectile.gameObject.AddComponent<CavendishBehaviour>();
                    projectile.GetComponent<CavendishBehaviour>().Pew();
                    break;
            }
        }

        public void Shoot() {
            _projectile = _projectilesPool.Get_projectile();
            _projectile.SetColor(_projectileColor);
            
            var projectileTransform = _projectile.transform;
            projectileTransform.position = cannonLauncherTransform.position;

            var cannonLauncherRotation = cannonLauncherTransform.rotation;
            projectileTransform.rotation = cannonLauncherRotation;
            
            SetBehaviour(_projectile, _projectileType);
        }
    }
}
