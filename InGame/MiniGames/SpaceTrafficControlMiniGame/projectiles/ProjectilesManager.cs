using InGame.Items.ItemsProperties.Bananas;
using InGame.MiniGames.SpaceTrafficControlMiniGame.projectiles.projectilesBehaviours;
using TMPro;
using UnityEngine;

namespace InGame.MiniGames.SpaceTrafficControlMiniGame.projectiles {
    public class ProjectilesManager : MonoBehaviour {
        [SerializeField] private GenericDictionary<RegionType, Transform> cannonLauncherBySceneType;
        [SerializeField] private GenericDictionary<BananaType, BananasPropertiesScriptableObject> bananasDataScriptableObjectsByBananaType;
        [SerializeField] private TextMeshProUGUI bananaSelectorQuantityText;
        
        private ProjectilesPool _projectilesPool;

        private Projectile _projectile;

        public BananaType _projectileType;
        private Color _projectileColor;
        
        public void Init() {
            _projectilesPool = GetComponent<ProjectilesPool>();
            SetBananasQuantity();
            
            SwitchBanana(_projectileType);
        }

        public void AlertNoBanana() {
            bananaSelectorQuantityText.text = "no banana";
            bananaSelectorQuantityText.color = Color.red;
            
            Invoke(nameof(SetNormalBananaQuantityColor), 0.5f);
        }

        private void SetBananasQuantity() {
            bananaSelectorQuantityText.text = ObjectsReference.Instance.bananasInventory.GetQuantity(_projectileType).ToString();
            if (ObjectsReference.Instance.bananasInventory.GetQuantity(_projectileType) == 0) AlertNoBanana();
        }

        private void SetNormalBananaQuantityColor() {
            bananaSelectorQuantityText.color = Color.white;
        }

        private void SwitchBanana(BananaType bananaType) {
            _projectileType = bananaType;
            _projectileColor = bananasDataScriptableObjectsByBananaType[bananaType].bananaColor;
            SetBananasQuantity();
        }

        private static void SetBehaviour(Projectile projectile, BananaType bananaType) {
            switch (bananaType) {
                case BananaType.CAVENDISH:
                    if (projectile.gameObject.GetComponent<CavendishBehaviour>() != null) {
                        Destroy(projectile.gameObject.GetComponent<CavendishBehaviour>());
                    }
                    projectile.gameObject.AddComponent<CavendishBehaviour>();
                    projectile.GetComponent<CavendishBehaviour>().Pew();
                    break;
            }
        }

        public void Shoot(RegionType regionType) {
            _projectile = _projectilesPool.Get_projectile();
            _projectile.SetColor(_projectileColor);
            
            var projectileTransform = _projectile.transform;
            projectileTransform.position = cannonLauncherBySceneType[regionType].position;

            var cannonLauncherRotation = cannonLauncherBySceneType[regionType].rotation;
            projectileTransform.rotation = cannonLauncherRotation;
            
            SetBananasQuantity();
            
            SetBehaviour(_projectile, _projectileType);
        }
    }
}
