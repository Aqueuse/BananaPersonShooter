using Data.Bananas;
using Enums;
using Game.BananaCannonMiniGame.projectilesBehaviours;
using TMPro;
using UnityEngine;

namespace Game.BananaCannonMiniGame {
    public class ProjectilesManager : MonoBehaviour {
        [SerializeField] private Transform cannonLauncherTransform;
        [SerializeField] private GenericDictionary<BananaType, BananasDataScriptableObject> bananasDataScriptableObjectsByBananaType;
        [SerializeField] private TextMeshProUGUI bananaSelectorQuantityText;
        
        private ProjectilesPool _projectilesPool;

        private Projectile _projectile;

        public BananaType _projectileType;
        private Color _projectileColor;
        
        private void Start() {
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

        public void Shoot() {
            _projectile = _projectilesPool.Get_projectile();
            _projectile.SetColor(_projectileColor);
            
            var projectileTransform = _projectile.transform;
            projectileTransform.position = cannonLauncherTransform.position;

            var cannonLauncherRotation = cannonLauncherTransform.rotation;
            projectileTransform.rotation = cannonLauncherRotation;
            
            SetBananasQuantity();
            
            SetBehaviour(_projectile, _projectileType);
        }
    }
}
