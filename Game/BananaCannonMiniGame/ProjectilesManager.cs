using Data.Bananas;
using Game.BananaCannonMiniGame.projectilesBehaviours;
using TMPro;
using UnityEngine;

namespace Game.BananaCannonMiniGame {
    public class ProjectilesManager : MonoBehaviour {
        [SerializeField] private Transform cannonLauncherTransform;
        [SerializeField] private GenericDictionary<ItemType, BananasDataScriptableObject> bananasDataScriptableObjectsByBananaType;
        [SerializeField] private TextMeshProUGUI bananaSelectorQuantityText;
        
        private ProjectilesPool _projectilesPool;

        private Projectile _projectile;

        public ItemType _projectileType;
        private Color _projectileColor;
        
        private void Start() {
            _projectilesPool = GetComponent<ProjectilesPool>();
            SetBananasQuantity();
            
            SwitchBanana(_projectileType);
        }

        public void AlertNoBanana() {
            bananaSelectorQuantityText.text = "0";
            bananaSelectorQuantityText.color = Color.red;
            
            Invoke(nameof(SetNormalBananaQuantityColor), 0.5f);
        }

        private void SetBananasQuantity() {
            bananaSelectorQuantityText.text = ObjectsReference.Instance.inventory.GetQuantity(_projectileType).ToString();
            if (ObjectsReference.Instance.inventory.GetQuantity(_projectileType) == 0) AlertNoBanana();
        }

        private void SetNormalBananaQuantityColor() {
            bananaSelectorQuantityText.color = Color.white;
        }

        private void SwitchBanana(ItemType bananaType) {
            _projectileType = bananaType;
            _projectileColor = bananasDataScriptableObjectsByBananaType[bananaType].bananaColor;
            SetBananasQuantity();
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
            
            SetBananasQuantity();
            
            SetBehaviour(_projectile, _projectileType);
        }
    }
}
