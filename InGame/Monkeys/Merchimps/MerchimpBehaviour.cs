using InGame.Inventories;
using InGame.Items.ItemsBehaviours;
using InGame.Items.ItemsData.Characters;
using InGame.Items.ItemsProperties;
using Save.Helpers;
using Save.Templates;
using UI.InGame.Merchimps;
using UnityEngine;

namespace InGame.Monkeys.Merchimps {
    public class MerchimpBehaviour : MonoBehaviour {
        [SerializeField] private SkinnedMeshRenderer meshRenderer;

        public UIMerchantWaitTimer uiMerchantWaitTimer;

        public MonkeyMenData monkeyMenData;
        private SpaceshipBehaviour associatedSpaceshipBehaviour;
        
        private static readonly int color00 = Shader.PropertyToID("_Color00");
        private static readonly int color01 = Shader.PropertyToID("_Color01");
        private static readonly int color02 = Shader.PropertyToID("_Color02");
        private static readonly int color10 = Shader.PropertyToID("_Color10");
        private static readonly int color11 = Shader.PropertyToID("_Color11");
        private static readonly int color12 = Shader.PropertyToID("_Color12");
        private static readonly int color20 = Shader.PropertyToID("_Color20");
        private static readonly int color21 = Shader.PropertyToID("_Color21");
        private static readonly int color22 = Shader.PropertyToID("_Color22");

        private Color[] colorPreset;
        
        [HideInInspector] public ItemScriptableObject activeItemScriptableObject;

        public IngredientsInventory ingredientsInventory;
        public ManufacturedItemsInventory manufacturedItemsInventory;
        public RawMaterialInventory rawMaterialInventory;
        
        private BananasInventory bananaManBananasInventory;
        private ManufacturedItemsInventory bananaManManufacturedItemsInventory;
        private IngredientsInventory bananaManIngredientsInventory;

        public void Start() {
            bananaManBananasInventory = ObjectsReference.Instance.BananaManBananasInventory;
            bananaManManufacturedItemsInventory = ObjectsReference.Instance.bananaManManufacturedItemsInventory;
            bananaManIngredientsInventory = ObjectsReference.Instance.bananaManIngredientsInventory;
            
            StartWaitingTimer();
        }

        public void Init(int propertiesIndex, SpaceshipBehaviour spaceshipBehaviour) {
            monkeyMenData.propertiesIndex = propertiesIndex;
            associatedSpaceshipBehaviour = spaceshipBehaviour;
            
            SetColors(propertiesIndex);
        }
        
        private int waitTimer;
        
        private void StartWaitingTimer() {
            uiMerchantWaitTimer.SetTimer(120);
            waitTimer = 120;
            InvokeRepeating(nameof(DecrementeTimer), 0, 1);
        }
        
        public void DecrementeTimer() {
            waitTimer--;
            if (waitTimer <= 0) {
                CancelInvoke(nameof(DecrementeTimer));
                associatedSpaceshipBehaviour.StopWaiting();
                gameObject.SetActive(false);
            }
            
            uiMerchantWaitTimer.SetTimer(waitTimer);
        }
        
        public int GetMerchantItemQuantity(ItemScriptableObject itemScriptableObject) {
            switch (itemScriptableObject.itemCategory) {
                case ItemCategory.INGREDIENT:
                    return monkeyMenData.ingredientsInventory[itemScriptableObject.ingredientsType];

                case ItemCategory.MANUFACTURED_ITEM:
                    return monkeyMenData.manufacturedItemsInventory[itemScriptableObject.manufacturedItemsType];

                case ItemCategory.DROPPED:
                    return monkeyMenData.rawMaterialsInventory[itemScriptableObject.rawMaterialType];
            }

            return 0;
        }

        public int GetBananaManItemQuantity(ItemScriptableObject itemScriptableObject) {
            switch (itemScriptableObject.itemCategory) {
                case ItemCategory.BANANA:
                    return bananaManBananasInventory.bananasInventory[itemScriptableObject.bananaType];

                case ItemCategory.MANUFACTURED_ITEM:
                    return bananaManManufacturedItemsInventory.manufacturedItemsInventory[itemScriptableObject.manufacturedItemsType];

                case ItemCategory.INGREDIENT:
                    return bananaManIngredientsInventory.ingredientsInventory[itemScriptableObject.ingredientsType];
            }

            return 0;
        }

        private void SetColors(int index) {
            var monkeyMenMaterial = meshRenderer.material;

            colorPreset = ObjectsReference.Instance.meshReferenceScriptableObject.merchimpsAppearanceScriptableObjects[index].colorSets;
            
            monkeyMenMaterial.SetColor(color00, colorPreset[0]);
            monkeyMenMaterial.SetColor(color01, colorPreset[1]);
            monkeyMenMaterial.SetColor(color02, colorPreset[2]);
            monkeyMenMaterial.SetColor(color10, colorPreset[3]);
            monkeyMenMaterial.SetColor(color11, colorPreset[4]);
            monkeyMenMaterial.SetColor(color12, colorPreset[5]);
            monkeyMenMaterial.SetColor(color20, colorPreset[6]);
            monkeyMenMaterial.SetColor(color21, colorPreset[7]);
            monkeyMenMaterial.SetColor(color22, colorPreset[8]);

            meshRenderer.material = monkeyMenMaterial;
        }
        
        public void LoadSavedData(MonkeyMenSavedData monkeyMenSavedData) {
            SetColors(monkeyMenSavedData.colorSetIndex);

            associatedSpaceshipBehaviour =
                ObjectsReference.Instance.spaceTrafficControlManager.spaceshipBehavioursByGuid[
                    monkeyMenData.spaceshipGuid];
        }
        
        public MonkeyMenSavedData GenerateSavedData() {
            return new MonkeyMenSavedData {
                characterType = monkeyMenData.characterType,
                ingredientsInventory = monkeyMenData.ingredientsInventory,
                manufacturedItemsInventory = monkeyMenData.manufacturedItemsInventory,
                rawMaterialsInventory = monkeyMenData.rawMaterialsInventory,
                bitKongQuantity = monkeyMenData.bitKongQuantity,
                uid = monkeyMenData.uid,
                name = monkeyMenData.monkeyMenName,
                prefabIndex = monkeyMenData.prefabIndex,
                colorSetIndex = monkeyMenData.propertiesIndex,
                destination = JsonHelper.FromVector3ToString(monkeyMenData.destination),
                spaceshipGuid = monkeyMenData.spaceshipGuid,
                position = JsonHelper.FromVector3ToString(transform.position),
                rotation = JsonHelper.FromQuaternionToString(transform.rotation),
                need = monkeyMenData.need,
                isSatisfied = monkeyMenData.isSatisfied
            };
        }
    }
}