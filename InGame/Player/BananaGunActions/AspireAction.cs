using InGame.Items;
using InGame.Items.ItemsBehaviours;
using InGame.Items.ItemsBehaviours.BuildablesBehaviours;
using InGame.Items.ItemsProperties.Dropped;
using Tags;
using UnityEngine;

namespace InGame.Player.BananaGunActions {

    public class AspireAction : MonoBehaviour {
        [SerializeField] private LayerMask GestionViewSelectableLayerMask;
        [SerializeField] private Camera mainCamera;

        public bool isAspiring;

        private Tag gameObjectTagClass;
        public GAME_OBJECT_TAG gameObjectTag;
        
        private Regime regimeClass;
        
        public GameObject targetedGameObject;
        
        private Ray ray;
        private RaycastHit raycastHit;
        
        private void Update() {
            if (Physics.Raycast(mainCamera.transform.position, mainCamera.transform.forward, out raycastHit, 100, layerMask: GestionViewSelectableLayerMask)) {
                Debug.Log(raycastHit.transform.gameObject);
                
                if (raycastHit.transform.gameObject.GetComponent<Tag>().itemScriptableObject.isAspirable) {
                    targetedGameObject = raycastHit.transform.gameObject;
                }
            }
            else {
                targetedGameObject = null;
                ObjectsReference.Instance.uInventoriesManager.GetCurrentUIHelper().ShowDefaultHelper();
            }
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
                        for (int i = 0; i < droppedRawMaterialIngredient.Value; i++) {
                            Instantiate(
                                droppedRawMaterialIngredient.Key.prefab,
                                spawnPosition,
                                Quaternion.identity,
                                ObjectsReference.Instance.gameSave.savablesItemsContainer
                            );
                        }
                    }
                    
                    Destroy(targetedGameObject);
                    targetedGameObject = null;

                    break;
                
                case GAME_OBJECT_TAG.BUILDABLE:
                    targetedGameObject.GetComponent<BuildableBehaviour>().ChangeToBlueprint();
                    targetedGameObject.GetComponent<BuildableBehaviour>().TryRetrieveOneRawMaterial();
                
                    ObjectsReference.Instance.audioManager.PlayEffect(SoundEffectType.TAKE_SOMETHING, 0);
                    break;
                
                case GAME_OBJECT_TAG.ITEM_STACK:
                    targetedGameObject.GetComponent<ItemStack>().ThrowOne();
                    break;
            }
            
            ObjectsReference.Instance.audioManager.PlayEffect(SoundEffectType.TAKE_SOMETHING, 0);

            ObjectsReference.Instance.bottomSlots.RefreshSlotsQuantities();
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