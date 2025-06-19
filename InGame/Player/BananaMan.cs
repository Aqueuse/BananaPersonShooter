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
        
        private void Start() {
            tpsPlayerAnimator = GetComponentInChildren<Animator>().GetComponent<TpsPlayerAnimator>();
            faceCanvasRenderer.SetMesh(facePlane.GetComponent<MeshFilter>().mesh);
        }
        
        // TODO : move to a foodBehaviour and change to foodType
        public void EatFood(BananaType bananaType) {
            if (ObjectsReference.Instance.BananaManBananasInventory.bananasInventory[bananaType] > 0 & health < _maxHealth) {
                var bananaData =
                    ObjectsReference.Instance.meshReferenceScriptableObject.bananasPropertiesScriptableObjects[bananaType];
                
                health += bananaData.healthBonus;

                //ObjectsReference.Instance.BananaManBananasInventory.RemoveQuantity(bananaManData.activeDroppableItem, 1);
                ObjectsReference.Instance.bananaManRawMaterialInventory.AddQuantity(
                    ObjectsReference.Instance.meshReferenceScriptableObject.
                        rawMaterialPropertiesScriptableObjects[RawMaterialType.BANANA_PEEL], 1);
                
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