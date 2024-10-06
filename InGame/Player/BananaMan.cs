using InGame.Items.ItemsProperties.Bananas;
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

        public void SetActiveItem(BananasPropertiesScriptableObject bananasPropertiesScriptableObject) {
            bananaManData.activeBanana = bananasPropertiesScriptableObject;
            
            ObjectsReference.Instance.uiFlippers.SetBananaType(bananasPropertiesScriptableObject);
            ObjectsReference.Instance.uiFlippers.SetBananaQuantity(ObjectsReference.Instance.bananasInventory.GetQuantity(bananasPropertiesScriptableObject.bananaType));
        }

        public void SetActiveBuildable(BuildablePropertiesScriptableObject buildablePropertiesScriptableObject) {
            bananaManData.activeBuildable = buildablePropertiesScriptableObject;
            ObjectsReference.Instance.build.SetActiveBuildable(buildablePropertiesScriptableObject.buildableType);
            ObjectsReference.Instance.uiFlippers.SetBuildable(buildablePropertiesScriptableObject.blueprintSprite);
        }

        public void GainHealth() {
            if (bananaManData.bananasInventory[bananaManData.activeBanana.bananaType] > 0 && health < _maxHealth) {
                health += bananaManData.activeBanana.healthBonus;
                resistance += bananaManData.activeBanana.resistanceBonus;

                ObjectsReference.Instance.bananasInventory.RemoveQuantity(bananaManData.activeBanana.bananaType, 1);
                ObjectsReference.Instance.droppedInventory.AddQuantity(DroppedType.BANANA_PEEL, 1);
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