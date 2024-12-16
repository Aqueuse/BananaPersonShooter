using InGame.Items.ItemsProperties;
using InGame.Items.ItemsProperties.Buildables;
using UnityEngine;

namespace InGame.Player {
    public class BananaMan : MonoBehaviour {
        [SerializeField] private SkinnedMeshRenderer bodyMeshRenderer;
        [SerializeField] private GameObject facePlane;
        [SerializeField] private CanvasRenderer faceCanvasRenderer;
        public TpsPlayerAnimator tpsPlayerAnimator;

        public BananaManData bananaManData;

        public bool isGrabingBananaGun;
        public BananaGunMode bananaGunMode;
        
        private const float _maxHealth = 100;
        public float health;
        public float resistance;
        private static readonly int CutoffHeight = Shader.PropertyToID("Cutoff_Height");

        public bool tutorialFinished;
        
        private void Start() {
            tpsPlayerAnimator = GetComponentInChildren<Animator>().GetComponent<TpsPlayerAnimator>();
            faceCanvasRenderer.SetMesh(facePlane.GetComponent<MeshFilter>().mesh);
        }
        
        public void SetActiveBuildable(BuildablePropertiesScriptableObject buildablePropertiesScriptableObject) {
            bananaManData.activeBuildable = buildablePropertiesScriptableObject.buildableType;
            
            ObjectsReference.Instance.build.SetActiveBuildable(buildablePropertiesScriptableObject.buildableType);
            ObjectsReference.Instance.uiFlippers.RefreshActiveBuildableAvailability();
        }

        public void SetActiveDropped(ItemScriptableObject itemScriptableObject) {
            bananaManData.activeDropped = itemScriptableObject.droppedType;
            bananaManData.activeBanana = itemScriptableObject.bananaType;
            bananaManData.activeIngredient = itemScriptableObject.ingredientsType;
            bananaManData.activeRawMaterial = itemScriptableObject.rawMaterialType;
            bananaManData.activeManufacturedItem = itemScriptableObject.manufacturedItemsType;
            
            ObjectsReference.Instance.uiFlippers.SetDroppableSprite(itemScriptableObject.GetSprite());
            ObjectsReference.Instance.inventoriesHelper.GetQuantity(itemScriptableObject);
        }

        public void GainHealth(BananaType bananaType) {
            if (ObjectsReference.Instance.BananaManBananasInventory.bananasInventory[bananaType] > 0 & health < _maxHealth) {
                var bananaData =
                    ObjectsReference.Instance.meshReferenceScriptableObject.bananasPropertiesScriptableObjects[bananaType];
                
                health += bananaData.healthBonus;

                ObjectsReference.Instance.BananaManBananasInventory.RemoveQuantity(bananaType, 1);
                ObjectsReference.Instance.bananaManRawMaterialInventory.AddQuantity(RawMaterialType.BANANA_PEEL, 1);
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