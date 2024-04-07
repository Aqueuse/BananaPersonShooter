using InGame.Items.ItemsProperties.Bananas;
using InGame.Items.ItemsProperties.Characters;
using UnityEngine;

namespace InGame.Player {
    public class BananaMan : MonoBehaviour {
        [SerializeField] private SkinnedMeshRenderer bodyMeshRenderer;
        [SerializeField] private GameObject facePlane;
        [SerializeField] private CanvasRenderer faceCanvasRenderer;
        public TpsPlayerAnimator tpsPlayerAnimator;

		public BananasPropertiesScriptableObject activeItem;

        public InventoryScriptableObject inventories;
        
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

        public void SetActiveItem(BananasPropertiesScriptableObject bananasPropertiesScriptableObject) {
            activeItem = bananasPropertiesScriptableObject;
            ObjectsReference.Instance.uiTools.SetBananaType(bananasPropertiesScriptableObject.GetSprite());
        }
        
        public void GainHealth() {
            if (ObjectsReference.Instance.bananaMan.inventories.bananasInventory[activeItem.bananaType] > 0 && health < _maxHealth) {
                health += activeItem.healthBonus;
                resistance += activeItem.resistanceBonus;

                ObjectsReference.Instance.bananasInventory.RemoveQuantity(activeItem.bananaType, 1);
                ObjectsReference.Instance.rawMaterialsInventory.AddQuantity(RawMaterialType.BANANA_PEEL, 1);
                SetBananaSkinHealth();
                ObjectsReference.Instance.audioManager.PlayEffect(SoundEffectType.EAT_BANANA, 0);
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
    }
}