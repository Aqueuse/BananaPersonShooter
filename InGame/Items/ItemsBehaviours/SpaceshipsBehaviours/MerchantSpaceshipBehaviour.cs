using System;
using InGame.Items.ItemsData;
using InGame.Monkeys.Merchimps;
using InGame.SpaceTrafficControl;
using Newtonsoft.Json;
using Save.Templates;

namespace InGame.Items.ItemsBehaviours.SpaceshipsBehaviours {
    public class MerchantSpaceshipBehaviour : SpaceshipBehaviour {
        public MerchantData merchantData;
        public Merchimp merchimp;
    
        public override void Init() {
            merchimp.merchantCharacterPropertiesScriptableObject = ObjectsReference.Instance.meshReferenceScriptableObject.merchantsScriptableObjectByMerchantType[merchimp.merchantType];
            merchimp.Init();
        }

        public override void GenerateSaveData() {
            if(string.IsNullOrEmpty(spaceshipGuid)) {
                spaceshipGuid = Guid.NewGuid().ToString();
            }

            SpaceshipSavedData merchantSpaceshipSavedData = new SpaceshipSavedData {
                spaceshipGuid = spaceshipGuid,
                merchantData = merchantData
            };
            
            // add merchimp data

            savedData = JsonConvert.SerializeObject(merchantSpaceshipSavedData); 
        }
        
        public override void Spawn3DSpacehip() {
            Hangars.Instance.Spawn3DSpaceshipInHangar(spaceshipSavedData.hangarNumber, ObjectsReference.Instance.meshReferenceScriptableObject.merchantSpaceshipPrefab);
        }
    }
}
