using System;
using InGame.Items.ItemsData;
using InGame.Monkeys.Merchimps;
using Newtonsoft.Json;
using Save.Helpers;
using Save.Templates;

namespace InGame.Items.ItemsBehaviours.SpaceshipsBehaviours {
    public class MerchantSpaceshipBehaviour : SpaceshipBehaviour {
        private MerchantData merchantData;
        public Merchimp merchimp;
    
        public override void Init() {
            transform.position = JsonHelper.FromStringToVector3(spaceshipSavedData.spaceshipPosition);
            transform.rotation = JsonHelper.FromStringToQuaternion(spaceshipSavedData.spaceshipRotation);

            spaceshipName = spaceshipSavedData.spaceshipName;
            spaceshipGuid = spaceshipSavedData.spaceshipGuid;
            travelState = spaceshipSavedData.travelState;

            merchantData = spaceshipSavedData.merchantData;

            if (spaceshipSavedData.travelState != TravelState.WAIT_IN_STATION) {
                // TODO : start incrementing timer
                // spawn character etc
                
                merchimp.merchantCharacterPropertiesScriptableObject = ObjectsReference.Instance.meshReferenceScriptableObject.merchantsScriptableObjectByMerchantType[merchimp.merchantType];
                merchimp.Init();
            }
        }

        public override void GenerateSaveData() {
            if(string.IsNullOrEmpty(spaceshipGuid)) {
                spaceshipGuid = Guid.NewGuid().ToString();
            }

            SpaceshipSavedData merchantSpaceshipSavedData = new SpaceshipSavedData {
                spaceshipName = spaceshipName,
                spaceshipGuid = spaceshipGuid,
                communicationMessageprefabIndex = communicationMessagePrefabIndex,
                merchantData = merchantData,
                spaceshipPosition = JsonHelper.FromVector3ToString(transform.position),
                spaceshipRotation = JsonHelper.FromQuaternionToString(transform.rotation),
                travelState = travelState,
                timeToNextState = timeToNextState
            };
            
            savedData = JsonConvert.SerializeObject(merchantSpaceshipSavedData); 
        }
    }
}
