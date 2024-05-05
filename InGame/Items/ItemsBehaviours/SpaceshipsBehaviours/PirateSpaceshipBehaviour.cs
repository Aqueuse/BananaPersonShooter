using System;
using System.Collections.Generic;
using InGame.Items.ItemsData;
using InGame.Monkeys.Chimpirates;
using Newtonsoft.Json;
using Save.Helpers;
using Save.Templates;
using UnityEngine;
using Random = UnityEngine.Random;

namespace InGame.Items.ItemsBehaviours.SpaceshipsBehaviours {
    public class PirateSpaceshipBehaviour : SpaceshipBehaviour {
        private List<GameObject> pirates;
        private List<PirateData> piratesDatas;
        
        [SerializeField] private Transform pirateAccessPoint;

        private GameObject pirateInstance;
        private GameObject piratePrefab;
        private PirateBehaviour pirateBehaviour;

        private int pirateNumber;

        public override void Init() {
            piratesDatas = spaceshipSavedData.pirateDatas;

            if (spaceshipSavedData.travelState == TravelState.WAIT_IN_STATION) {
                ObjectsReference.Instance.spaceTrafficControlManager.AssignSpaceshipToHangar(assignatedHangar);

                if (piratesDatas == null) {
                    SpawnPirates();
                }

                else {
                    LoadPirates();
                }
            }
        }

        private void LoadPirates() {
            piratePrefab = ObjectsReference.Instance.meshReferenceScriptableObject.piratePrefab;

            pirateNumber = piratesDatas.Count;
            
            if (pirateNumber > 0) {
                pirateNumber -= 1;
                
                Invoke(nameof(SpawnPirate), Random.Range(6, 13));
            }

            foreach (var pirateData in piratesDatas) {
                pirateInstance = Instantiate(piratePrefab, pirateAccessPoint.position, Quaternion.identity);
                
                pirateBehaviour = pirateInstance.GetComponent<PirateBehaviour>();
                pirateBehaviour.pirateSavedData = pirateData;

                pirateBehaviour.pirateState = pirateData.pirateState;
                pirateBehaviour.destination = JsonHelper.FromStringToVector3(pirateData.destination);
                pirateBehaviour.pirateInventory = pirateData.piratesInventory;
                pirateBehaviour.transform.position = JsonHelper.FromStringToVector3(pirateData.piratePosition);
                pirateBehaviour.transform.rotation = JsonHelper.FromStringToQuaternion(pirateData.pirateRotation);
                
                pirates.Add(pirateInstance);
            }
        }
        
        public void SpawnPirates() {
            piratePrefab = ObjectsReference.Instance.meshReferenceScriptableObject.piratePrefab;
            pirates = new List<GameObject>();

            pirateNumber = Random.Range(6, 13);

            if (pirateNumber > 0) {
                pirateNumber -= 1;
                
                Invoke(nameof(SpawnPirate), Random.Range(6, 13));
            }
            
            if (pirateNumber <= 0) CancelInvoke(nameof(SpawnPirate));
        }

        private void SpawnPirate() {
            pirateInstance = Instantiate(piratePrefab, pirateAccessPoint.position, Quaternion.identity);
            pirates.Add(pirateInstance);
        }
        
        public override void GenerateSaveData() {
            if(string.IsNullOrEmpty(spaceshipGuid)) {
                spaceshipGuid = Guid.NewGuid().ToString();
            }
            
            foreach (var pirate in pirates) {
                piratesDatas = new List<PirateData>();
                
                pirate.GetComponent<PirateBehaviour>().GenerateSavedData();
                piratesDatas.Add(pirate.GetComponent<PirateBehaviour>().pirateSavedData);
            }

            spaceshipSavedData = new SpaceshipSavedData {
                spaceshipName = spaceshipName,
                spaceshipGuid = spaceshipGuid,
                communicationMessagePrefabIndex = communicationMessagePrefabIndex,
                uiColor = JsonHelper.FromColorToString(spaceshipUIcolor),
                characterType = characterType,
                pirateDatas = piratesDatas,
                spaceshipPosition = JsonHelper.FromVector3ToString(transform.position),
                spaceshipRotation = JsonHelper.FromQuaternionToString(transform.rotation),
                travelState = travelState,
                arrivalPoint = JsonHelper.FromVector3ToString(arrivalPosition),
                hangarNumber = assignatedHangar
            };

            savedData = JsonConvert.SerializeObject(spaceshipSavedData);
        }
    }
}
