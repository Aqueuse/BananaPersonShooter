using System;
using System.Collections.Generic;
using InGame.Items.ItemsData;
using InGame.SpaceTrafficControl;
using Newtonsoft.Json;
using Save.Templates;

namespace InGame.Items.ItemsBehaviours.SpaceshipsBehaviours {
    public class PirateSpaceshipBehaviour : SpaceshipBehaviour {
        public List<PirateData> piratesData = new List<PirateData>();
        
        public override void Init() {
            if (spaceshipSavedData.travelState != TravelState.WAIT_IN_STATION) {
                // setup space traffic control mini game
                if (spaceshipSavedData.distance > 1000) {
                    ObjectsReference.Instance.spaceTrafficControlMiniGameManager.spaceshipsSpawner.ShowDistantDot(spaceshipSavedData.spaceshipName);
                }

                else {
                    ObjectsReference.Instance.spaceTrafficControlMiniGameManager.spaceshipsSpawner.Show2DSpaceship(spaceshipSavedData);
                }
            }

            else {
                // spawn 3D spaceship
                Spawn3DSpacehip();
            }
            
            // start incrementing timer
        }

        public override void GenerateSaveData() {
            if(string.IsNullOrEmpty(spaceshipGuid)) {
                spaceshipGuid = Guid.NewGuid().ToString();
            }

            SpaceshipSavedData pirateSpaceshipData = new SpaceshipSavedData {
                spaceshipGuid = spaceshipGuid,
                pirateDatas = piratesData
            };

            savedData = JsonConvert.SerializeObject(pirateSpaceshipData);
        }

        public override void Spawn3DSpacehip() {
            Hangars.Instance.Spawn3DSpaceshipInHangar(spaceshipSavedData.hangarNumber, ObjectsReference.Instance.meshReferenceScriptableObject.pirateSpaceshipPrefab);
        }
    }
}
