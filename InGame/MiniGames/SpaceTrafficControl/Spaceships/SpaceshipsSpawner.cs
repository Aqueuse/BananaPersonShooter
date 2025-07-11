using System.Collections.Generic;
using InGame.Items.ItemsBehaviours;
using InGame.MiniGames.MarketingCampaign;
using UnityEngine;
using Random = UnityEngine.Random;

namespace InGame.MiniGames.SpaceTrafficControl.Spaceships {
    public class SpaceshipsSpawner : MonoBehaviour {
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
            ObjectsReference.Instance.uiCommunicationPanel.HideCampaignCreationTools();

            spaceships = ShuffleSpaceships(adCampaign.piratesNumber + adCampaign.touristsNumber, adCampaign.merchimpsNumber);

            foreach (var spaceship in spaceships) {
                spacechipsQueue.Enqueue(spaceship);
            }

            InvokeRepeating(nameof(SpawnSpaceshipInSpace), 2, 2);
        }

        public void RemoveGuestInCampaignCreator() {
            if (spaceships.Count == 0) {
                ObjectsReference.Instance.uiCommunicationPanel.ShowCampaignCreatorTools();
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

            var characterType = spacechipsQueue.Dequeue();

            var spaceshipType = MeshReferenceScriptableObject.GetRandomSpaceshipType();

            var spaceshipBehaviour = SpawnSpaceship(spaceshipType, entryPoint, Quaternion.identity);
            
            spaceshipBehaviour.Init(arrivalPoint, characterType, spaceshipType);
            spaceshipBehaviour.OpenCommunications(characterType);
            
            ObjectsReference.Instance.spaceTrafficControlManager.spaceshipBehavioursByGuid.Add(spaceshipBehaviour.spaceshipData.spaceshipGuid, spaceshipBehaviour);
        }

        public SpaceshipBehaviour SpawnSpaceship(SpaceshipType spaceshipType, Vector3 position, Quaternion rotation) {
            var spaceship = Instantiate(
                ObjectsReference.Instance.meshReferenceScriptableObject.spaceshipPrefabBySpaceshipType[spaceshipType],
                position,
                rotation,
                ObjectsReference.Instance.gameSave.savablesItemsContainer);
            
            spaceship.transform.rotation = Quaternion.Euler((transform.position - entryPoint).normalized);

            return spaceship.GetComponent<SpaceshipBehaviour>();
        }

        private List<CharacterType> ShuffleSpaceships(int visitorsSpaceshipsQuantity, int merchantsSpaceshipsQuantity) {
            spaceships.Clear();
            
            for (var i = 0; i < visitorsSpaceshipsQuantity; i++) {
                spaceships.Add(CharacterType.VISITOR);
            }

            for (var i = 0; i < merchantsSpaceshipsQuantity; i++) {
                spaceships.Add(CharacterType.MERCHIMP);
            }

            var listCount = spaceships.Count;

            while (listCount > 1) {
                listCount--;
                var nextRandomIndex = systemRandom.Next(listCount + 1);
                (spaceships[nextRandomIndex], spaceships[listCount]) = (spaceships[listCount], spaceships[nextRandomIndex]);
            }

            return spaceships;
        }
    }
}
