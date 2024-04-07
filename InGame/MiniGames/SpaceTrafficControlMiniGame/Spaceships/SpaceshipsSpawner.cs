using System.Collections.Generic;
using InGame.Items.ItemsBehaviours.SpaceshipsBehaviours;
using UnityEngine;
using Random = UnityEngine.Random;

namespace InGame.MiniGames.SpaceTrafficControlMiniGame.Spaceships {
    public class SpaceshipsSpawner : MonoBehaviour {
        [SerializeField] private Transform spaceshipsContainer;

        public float spaceshipPropulsionSpeed = 100f;
        private Vector3 spaceshipPosition;

        private System.Random systemRandom;
        private List<CharacterType> spaceships;

        private Queue<CharacterType> spacechipsQueue;
        
        private GameObject _spaceshipInstance;
        private SpaceshipBehaviour spaceshipBehaviourInstance;

        private Vector3 spaceshipSpawnerPosition;

        private void Start() {
            systemRandom = new System.Random();
            spaceships = new List<CharacterType>();
            spacechipsQueue = new Queue<CharacterType>();
        }
        
        public void spawnSpaceshipsWithAdCampaign() {
            ObjectsReference.Instance.uiMarketingPanel.SwitchToCurrentCampaign();
            ObjectsReference.Instance.commandRoomControlPanelsManager.UnfocusPanel();
            var adCampaign = ObjectsReference.Instance.adMarketingCampaignManager.currentAdCampaign;
            
            spaceships = ShuffleSpaceships(adCampaign.piratesNumber, adCampaign.touristsNumber, adCampaign.merchimpsNumber);
            
            foreach (var spaceship in spaceships) {
                spacechipsQueue.Enqueue(spaceship);
            }
            
            InvokeRepeating(nameof(SpawnSpaceshipInSpace), 2, 2);
        }
        
        public void RemoveGuest() {
            if (spaceships.Count == 0) {
                ObjectsReference.Instance.uiMarketingPanel.SwitchToCampaignCreator();
            }

            else {
                spaceships.RemoveAt(0);
            }
        }

        public void SpawnSpaceshipInSpace() {
            spaceshipSpawnerPosition = transform.position;

            var spaceshipType = spacechipsQueue.Dequeue();
            
            var spaceship = Instantiate(
                ObjectsReference.Instance.meshReferenceScriptableObject.spaceshipByCharacterType[spaceshipType],
                spaceshipSpawnerPosition,
                Quaternion.identity,
                spaceshipsContainer);
            
            spaceshipPosition = spaceship.transform.position;
            
            var randomPositionInCircle = Random.insideUnitCircle.normalized * 6000;
            var randomEntryPoint = new Vector3(
                spaceshipPosition.x + randomPositionInCircle.x, 
                spaceshipPosition.y, 
                spaceshipPosition.z +randomPositionInCircle.y
            );

            spaceship.transform.position = randomEntryPoint;
            spaceship.transform.rotation = Quaternion.Euler((spaceshipSpawnerPosition - randomEntryPoint).normalized); 
            
            spaceshipBehaviourInstance = spaceship.GetComponent<SpaceshipBehaviour>();
            spaceshipBehaviourInstance.characterType = spaceshipType;
            
            spaceshipBehaviourInstance.OpenCommunications();
            
            ObjectsReference.Instance.spaceTrafficControlManager.spaceshipBehavioursByGuid.Add(spaceshipBehaviourInstance.spaceshipGuid, spaceship.GetComponent<PirateSpaceshipBehaviour>());
            spaceshipBehaviourInstance.travelState = TravelState.FREE_FLIGHT;

            spaceshipBehaviourInstance.InitiatePropulsion((spaceshipSpawnerPosition - randomEntryPoint).normalized * 4000);
            
            if (spacechipsQueue.Count == 0) CancelInvoke(nameof(SpawnSpaceshipInSpace));
        }
        
        private List<CharacterType> ShuffleSpaceships(int pirateSpaceshipsQuantity, int visitorsSpaceshipsQuantity, int merchantsSpaceshipsQuantity) {
            spaceships.Clear();

            for (int i = 0; i < pirateSpaceshipsQuantity; i++) {
                spaceships.Add(CharacterType.PIRATE);
            }

            for (int i = 0; i < visitorsSpaceshipsQuantity; i++) {
                spaceships.Add(CharacterType.TOURIST);
            }

            for (int i = 0; i < merchantsSpaceshipsQuantity; i++) {
                spaceships.Add(CharacterType.MERCHIMP);
            }

            int listCount = spaceships.Count;

            while (listCount > 1) {
                listCount--;
                int nextRandomIndex = systemRandom.Next(listCount + 1);
                (spaceships[nextRandomIndex], spaceships[listCount]) = (spaceships[listCount], spaceships[nextRandomIndex]);
            }

            return spaceships;
        }
    }
}
