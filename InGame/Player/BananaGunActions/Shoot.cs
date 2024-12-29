using InGame.Items;
using InGame.Items.ItemsBehaviours;
using InGame.Items.ItemsProperties.Dropped;
using Tags;
using UnityEngine;

namespace InGame.Player.BananaGunActions {
    public class Shoot : MonoBehaviour {
        [SerializeField] private Transform launchingPoint;
        public GameObject targetedGameObject;
        public bool isAspiring;
        
        [SerializeField] private LayerMask GestionViewSelectableLayerMask;

        [SerializeField] private Camera mainCamera;

        private Ray ray;
        private RaycastHit raycastHit;

        private GameObject droppable;

        private Tag gameObjectTagClass;
        private GAME_OBJECT_TAG gameObjectTag;

        private GenericDictionary<RawMaterialType, int> rawMaterialsWithQuantity;
        private Regime regimeClass;

        private void Update() {
            if (Physics.Raycast(mainCamera.transform.position, mainCamera.transform.forward, out raycastHit, 100, layerMask: GestionViewSelectableLayerMask)) {
                if (raycastHit.transform.gameObject.GetComponent<Tag>().itemScriptableObject.isAspirable) {
                    targetedGameObject = raycastHit.transform.gameObject;
                    ObjectsReference.Instance.uInventoriesManager.GetCurrentUIHelper().ShowRetrieveConfirmation();
                }
            }
            else {
                targetedGameObject = null;
                ObjectsReference.Instance.uInventoriesManager.GetCurrentUIHelper().ShowDefaultHelper();
            }
        }

        public void LoadingGun() {
            InvokeRepeating(nameof(Throw), 0.3f, 0.3f);
        }

        public void CancelThrow() {
            ObjectsReference.Instance.audioManager.StopAudioSource(AudioSourcesType.EFFECT);
            CancelInvoke(nameof(Throw));
        }

        public void Throw() {
            droppable = Instantiate(
                ObjectsReference.Instance.meshReferenceScriptableObject.GetActiveDroppablePrefab(), 
                launchingPoint.transform.position, 
                Quaternion.identity,
                ObjectsReference.Instance.gameSave.savablesItemsContainer);
            
            droppable.GetComponent<Rigidbody>().AddForce(launchingPoint.transform.forward * 10000, ForceMode.Force);
            
            ObjectsReference.Instance.bananaMan.bananaManData.RemoveActiveDroppableItemQuantity(1);

            ObjectsReference.Instance.audioManager.PlayEffect(SoundEffectType.THROW_BANANA, 0);
            ObjectsReference.Instance.uiFlippers.RefreshActiveDroppableQuantity();
        }

        public void LoadAspire() {
            InvokeRepeating(nameof(Aspire), 0.3f, 0.3f);
        }
        
        public void Aspire() {
            isAspiring = true;
            
            if (targetedGameObject == null) return;
            
            gameObjectTagClass = targetedGameObject.GetComponent<Tag>();
            gameObjectTag = gameObjectTagClass.gameObjectTag;
            
            switch (gameObjectTag) {
                case GAME_OBJECT_TAG.REGIME:
                    regimeClass = targetedGameObject.GetComponent<Regime>();
                    if (regimeClass.regimeStade != RegimeStade.MATURE) return;
                    
                    regimeClass.GrabBananas();

                    foreach (var monkey in ObjectsReference.Instance.worldData.monkeys) {
                        monkey.SearchForBananaManBananas();
                    }
                    break;
                
                case GAME_OBJECT_TAG.WASTE:
                    targetedGameObject.GetComponent<MeshCollider>().enabled = false;
                    
                    var _wastePropertiesScriptableObject = 
                        (WastePropertiesScriptableObject)gameObjectTagClass.itemScriptableObject;

                    var spawnPosition = targetedGameObject.transform.position;
                    
                    foreach (var droppedRawMaterialIngredient in _wastePropertiesScriptableObject.rawMaterialsWithQuantity) {
                        Instantiate(
                            droppedRawMaterialIngredient.Key.prefab,
                            spawnPosition,
                            Quaternion.identity,
                            ObjectsReference.Instance.gameSave.savablesItemsContainer
                        );
                    }
                    
                    Destroy(targetedGameObject);
                    targetedGameObject = null;

                    break;
                
                case GAME_OBJECT_TAG.ITEM_STACK:
                    targetedGameObject.GetComponent<ItemStack>().ThrowOne();
                    break;
            }
            
            ObjectsReference.Instance.uInventoriesManager.GetCurrentUIHelper().HideRetrieveConfirmation();
            ObjectsReference.Instance.audioManager.PlayEffect(SoundEffectType.TAKE_SOMETHING, 0);

            ObjectsReference.Instance.uiFlippers.RefreshActiveDroppableQuantity();
        }

        public void CancelAspire() {
            isAspiring = false;
            
            CancelInvoke(nameof(Aspire));
        }

        public bool IsTargetingMe(Transform myTransform) {
            if (targetedGameObject == null) return false;

            return targetedGameObject.transform == myTransform;
        }
    }
}