using InGame.Items.ItemsProperties.Characters;
using UI.InGame.Merchimps;
using UnityEngine;

namespace InGame.Monkeys.Merchimps {
    public class Merchimp : MonoBehaviour {
        public InventoryScriptableObject merchantPropertiesScriptableObject;
        public CharacterPropertiesScriptableObject merchantCharacterPropertiesScriptableObject;
        public UIMerchantWaitTimer uiMerchantWaitTimer;
        
        public MerchantType merchantType;
        
        private static readonly int color00 = Shader.PropertyToID("_Color00");
        private static readonly int color01 = Shader.PropertyToID("_Color01");
        private static readonly int color02 = Shader.PropertyToID("_Color02");
        private static readonly int color10 = Shader.PropertyToID("_Color10");
        private static readonly int color11 = Shader.PropertyToID("_Color11");
        private static readonly int color12 = Shader.PropertyToID("_Color12");
        private static readonly int color20 = Shader.PropertyToID("_Color20");
        private static readonly int color21 = Shader.PropertyToID("_Color21");
        private static readonly int color22 = Shader.PropertyToID("_Color22");

        [SerializeField] private SkinnedMeshRenderer meshRenderer;
        private UIMerchant uiMerchant;
        
        public void Init() {
            uiMerchant = ObjectsReference.Instance.uiMerchant;
            ObjectsReference.Instance.chimpManager.merchimpsManager.activeMerchimp = this;
            
            var merchantMaterial = meshRenderer.material;
            
            merchantMaterial.SetColor(color00, merchantCharacterPropertiesScriptableObject.clothColors[0]);
            merchantMaterial.SetColor(color01, merchantCharacterPropertiesScriptableObject.clothColors[1]);
            merchantMaterial.SetColor(color02, merchantCharacterPropertiesScriptableObject.clothColors[2]);
            merchantMaterial.SetColor(color10, merchantCharacterPropertiesScriptableObject.clothColors[3]);
            merchantMaterial.SetColor(color11, merchantCharacterPropertiesScriptableObject.clothColors[4]);
            merchantMaterial.SetColor(color12, merchantCharacterPropertiesScriptableObject.clothColors[5]);
            merchantMaterial.SetColor(color20, merchantCharacterPropertiesScriptableObject.clothColors[6]);
            merchantMaterial.SetColor(color21, merchantCharacterPropertiesScriptableObject.clothColors[7]);
            merchantMaterial.SetColor(color22, merchantCharacterPropertiesScriptableObject.clothColors[8]);

            meshRenderer.material = merchantMaterial;

            uiMerchant.InitializeInventories(merchantPropertiesScriptableObject);
            uiMerchant.RefreshMerchantInventories();
            uiMerchant.RefreshBitkongQuantities();
            uiMerchant.Switch_to_Sell_inventory();
        }
    }
}