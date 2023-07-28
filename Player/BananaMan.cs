using Data.Bananas;
using Enums;
using UnityEngine;

namespace Player {
    public class BananaMan : MonoBehaviour {
        [SerializeField] private Material bodyMaterial;
        public TpsPlayerAnimator tpsPlayerAnimator;

		public BananasDataScriptableObject activeItem;
        public ItemType activeItemType = ItemType.EMPTY;
        public ItemCategory activeItemCategory = ItemCategory.EMPTY;
        public BuildableType activeBuildableType = BuildableType.EMPTY; 
        
        public bool isInWater;
        public bool isGrabingBananaGun;

        private const float _maxHealth = 100;
        public float health;
        public float resistance;
        private static readonly int CutoffHeight = Shader.PropertyToID("Cutoff_Height");

        public bool hasRepairedBananaGun;
        public bool tutorialFinished;
        
        private void Start() {
            tpsPlayerAnimator = GetComponentInChildren<Animator>().GetComponent<TpsPlayerAnimator>();
        }
        
        public void GainHealth() {
            if (ObjectsReference.Instance.inventory.bananaManInventory[activeItemType] > 0 && health < _maxHealth) {
                health += activeItem.healthBonus;
                resistance += activeItem.resistanceBonus;

                ObjectsReference.Instance.inventory.RemoveQuantity(activeItemCategory, activeItemType, 1);
                ObjectsReference.Instance.inventory.AddQuantity(ItemCategory.RAW_MATERIAL, ItemType.BANANA_PEEL, 1);
                SetBananaSkinHealth();
                ObjectsReference.Instance.audioManager.PlayEffect(EffectType.EAT_BANANA, 0);
            }
        }

        public void TakeDamage(float damageAccount) {
            health -= damageAccount;
            SetBananaSkinHealth();
            
            if (health <= 0) ObjectsReference.Instance.death.Die();
        }

        public void SetBananaSkinHealth() {
            bodyMaterial.SetFloat(CutoffHeight, (_maxHealth-health)/100);
        }

        public void SetActiveItemTypeAndCategory(ItemType itemType, ItemCategory itemCategory, BuildableType itemBuildableType) {
            activeItemType = itemType;
            activeItemCategory = itemCategory;
            activeBuildableType = itemBuildableType;
        }
    }
}