using System;
using InGame.Items.ItemsData;
using InGame.Monkeys.Merchimps;
using Save.Helpers;
using Save.Templates;
using UI.InGame.Merchimps;
using UnityEngine;

namespace InGame.Items.ItemsBehaviours.SpaceshipsBehaviours {
    public class MerchantSpaceshipBehaviour : SpaceshipBehaviour {
        [SerializeField] private Transform merchantInTransform;
        [SerializeField] private Transform merchantOutTransform;
        [SerializeField] private UIMerchantWaitTimer uiMerchantWaitTimer;

        private MerchantData merchantData;
        public MerchimpBehaviour merchimpBehaviour;

        private int waitTimer;

        public override void Init() {
            merchantData = spaceshipSavedData.merchantData;

            if (spaceshipSavedData.travelState == TravelState.WAIT_IN_STATION) {
                //merchimp.merchantMerchantPropertiesScriptableObject = ObjectsReference.Instance.meshReferenceScriptableObject.merchantsScriptableObjectByMerchantType[merchimp.merchantType];

                ObjectsReference.Instance.spaceTrafficControlManager.AssignSpaceshipToHangar(assignatedHangar);
                StartWaitingTimer();
                
                merchimpBehaviour.StartToSell();
            }
        }

        public void StartWaitingTimer() {
            merchimpBehaviour.transform.position = merchantOutTransform.position;
            
            uiMerchantWaitTimer.SetTimer(120);
            waitTimer = 120;
            InvokeRepeating(nameof(DecrementeTimer), 0, 1);
        }

        private void StopWaiting() {
            merchimpBehaviour.transform.position = merchantInTransform.position;
            CancelInvoke(nameof(DecrementeTimer));
            
            ObjectsReference.Instance.spaceTrafficControlManager.FreeHangar(assignatedHangar);
            
            travelState = TravelState.TRAVEL_BACK_ON_ELEVATOR;
            TravelBackOnElevator();
        }

        public void DecrementeTimer() {
            waitTimer--;
            if (waitTimer <= 0) StopWaiting();
            uiMerchantWaitTimer.SetTimer(waitTimer);
        }

        public override void GenerateSaveData() {
            if(string.IsNullOrEmpty(spaceshipGuid)) {
                spaceshipGuid = Guid.NewGuid().ToString();
            }

            SpaceshipSavedData merchantSpaceshipSavedData = new SpaceshipSavedData {
                spaceshipName = spaceshipName,
                spaceshipGuid = spaceshipGuid,
                communicationMessagePrefabIndex = communicationMessagePrefabIndex,
                uiColor = JsonHelper.FromColorToString(spaceshipUIcolor),
                merchantData = merchantData,
                characterType = characterType,
                spaceshipPosition = JsonHelper.FromVector3ToString(transform.position),
                spaceshipRotation = JsonHelper.FromQuaternionToString(transform.rotation),
                travelState = travelState,
                arrivalPoint = JsonHelper.FromVector3ToString(arrivalPosition),
                hangarNumber = assignatedHangar
            };
            
            savedData = merchantSpaceshipSavedData; 
        }
    }
}
