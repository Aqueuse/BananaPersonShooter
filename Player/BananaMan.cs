using Data.Bananas;
using Enums;
using UnityEngine;

namespace Player {
    public class BananaMan : MonoBehaviour {
        [SerializeField] private SkinnedMeshRenderer bodyMeshRenderer;
        [SerializeField] private GameObject facePlane;
        [SerializeField] private CanvasRenderer faceCanvasRenderer;
        public TpsPlayerAnimator tpsPlayerAnimator;
        
		public BananasDataScriptableObject activeItem;
        public BananaType activeBananaType = BananaType.EMPTY;
        public ItemCategory activeItemCategory = ItemCategory.EMPTY;
        public BuildableType activeBuildableType = BuildableType.EMPTY; 

        public bool isInWater;
        public bool isGrabingBananaGun;

        public bool canTakeFallDamage;

        private const float _maxHealth = 100;
        public float health;
        public float resistance;
        private static readonly int CutoffHeight = Shader.PropertyToID("Cutoff_Height");

        public bool tutorialFinished;
        
        private void Start() {
            tpsPlayerAnimator = GetComponentInChildren<Animator>().GetComponent<TpsPlayerAnimator>();
            faceCanvasRenderer.SetMesh(facePlane.GetComponent<MeshFilter>().mesh);
        }
        
        public void GainHealth() {
            if (ObjectsReference.Instance.bananasInventory.bananasInventory[activeBananaType] > 0 && health < _maxHealth) {
                health += activeItem.healthBonus;
                resistance += activeItem.resistanceBonus;

                ObjectsReference.Instance.bananasInventory.RemoveQuantity(activeBananaType, 1);
                ObjectsReference.Instance.rawMaterialsInventory.AddQuantity(RawMaterialType.BANANA_PEEL, 1);
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
            bodyMeshRenderer.materials[0].SetFloat(CutoffHeight, (_maxHealth-health)/100);
        }

        public void SetActiveItemTypeAndCategory(BananaType bananaType, ItemCategory itemCategory, BuildableType itemBuildableType) {
            activeBananaType = bananaType;
            activeItemCategory = itemCategory;
            activeBuildableType = itemBuildableType;
        }
    }
}