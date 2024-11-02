using System.Collections.Generic;
using InGame.Items.ItemsBehaviours;
using InGame.MiniGames.MarketingCampaignMiniGame;
using UnityEngine;
using Random = UnityEngine.Random;

namespace InGame.MiniGames.SpaceTrafficControlMiniGame.Spaceships {
    public class SpaceshipsSpawner : MonoBehaviour {
        [SerializeField] private Transform spaceshipsContainer;

        private AdCampaign adCampaign;

        private Vector3 spaceshipPosition;

        private System.Random systemRandom;
        private List<CharacterType> spaceships;

        private Queue<CharacterType> spacechipsQueue;
        
        private GameObject _spaceshipInstance;
        private SpaceshipBehaviour spaceshipBehaviourInstance;

        private Vector3 spaceshipSpawnerPosition;

        private Vector3 entryPoint;
        private Vector3 arrivalPoint;

        private void Start() {
            systemRandom = new System.Random();
            spaceships = new List<CharacterType>();
            spacechipsQueue = new Queue<CharacterType>();
            
            adCampaign = ObjectsReference.Instance.adMarketingCampaignManager.adCampaign;

        }
        
        public void SpawnSpaceshipsWithAdCampaign() {
            ObjectsReference.Instance.uiMarketingPanel.SwitchToCurrentCampaign();
            ObjectsReference.Instance.commandRoomControlPanelsManager.UnfocusPanel();
            
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
            if (spacechipsQueue.Count == 0) {
                CancelInvoke(nameof(SpawnSpaceshipInSpace));
                return;
            }
            
            var randomPositionInCircle = Random.insideUnitCircle.normalized * 6000;

            entryPoint = new Vector3(spaceshipPosition.x + randomPositionInCircle.x, 2631f, spaceshipPosition.z +randomPositionInCircle.y);
            arrivalPoint = (spaceshipSpawnerPosition - entryPoint).normalized * 4000;
            arrivalPoint.y = 2631f;

            var spaceshipType = spacechipsQueue.Dequeue();
            var spaceship = Instantiate(
                ObjectsReference.Instance.meshReferenceScriptableObject.spaceshipByCharacterType[spaceshipType],
                entryPoint,
                Quaternion.identity,
                null);
            
            spaceship.transform.rotation = Quaternion.Euler((transform.position - entryPoint).normalized); 

            spaceshipBehaviourInstance = spaceship.GetComponent<SpaceshipBehaviour>();
            
            spaceshipBehaviourInstance.GenerateSpaceshipData();
            spaceshipBehaviourInstance.arrivalPosition = arrivalPoint;
            
            ObjectsReference.Instance.spaceTrafficControlManager.spaceshipBehavioursByGuid.Add(spaceshipBehaviourInstance.spaceshipGuid, spaceship.GetComponent<SpaceshipBehaviour>());

            spaceshipBehaviourInstance.OpenCommunications();
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
