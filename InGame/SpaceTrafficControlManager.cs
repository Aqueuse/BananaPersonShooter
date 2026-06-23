using System.Collections.Generic;
using InGame.Items.ItemsBehaviours;
using UnityEngine;
using UnityEngine.Splines;
using Random = UnityEngine.Random;

namespace InGame.SpaceTrafficControl {
    public class SpaceTrafficControlManager : MonoBehaviour {
        public GenericDictionary<string, SpaceshipBehaviour> spaceshipBehavioursByGuid;
        public GenericDictionary<int, bool> hangarAvailabilityByHangarNumber;

        public List<SplineContainer> spacePaths;
        public List<SplineContainer> elevatorPaths;
        
        private int nextRandomIndex;
        private System.Random systemRandom;

        private char letter1;
        private char letter2;
        private char letter3;
        private string number;

        public SpaceshipBehaviour selectedSpaceship;
        
        private void Start() {
            systemRandom = new System.Random();
        }

        public void AssignSpaceshipToHangar(int hangarNumber) {
            hangarAvailabilityByHangarNumber[hangarNumber] = false;
            ObjectsReference.Instance.uiCommunicationPanel.RefreshHangarAvailability();
            ObjectsReference.Instance.uiCommunicationPanel.RefreshCommunicationQuantityButton();
        }

        public void AssignToHangarFromCommunications(int hangarNumber) {
            // index are shifted in the UI because we start to count at 1 instead of 0
            ObjectsReference.Instance.spaceTrafficControlManager.AssignSpaceshipToHangar(hangarNumber);
            ObjectsReference.Instance.uiCommunicationPanel.CloseCommunications(selectedSpaceship);
            selectedSpaceship.TravelOnSpacePath(hangarNumber);
        }

        public void RefuseSpaceship() {
            ObjectsReference.Instance.uiCommunicationPanel.CloseCommunications(selectedSpaceship);
        }

        public void FreeHangar(int hangarNumber) {
            hangarAvailabilityByHangarNumber[hangarNumber] = true;
            
            ObjectsReference.Instance.uiCommunicationPanel.RefreshHangarAvailability();
            ObjectsReference.Instance.uiCommunicationPanel.RefreshCommunicationQuantityButton();
        }
        
        public string GetUniqueSpaceshipName() {
            nextRandomIndex = systemRandom.Next(26);
            letter1 = (char)('A'+nextRandomIndex);
            
            nextRandomIndex = systemRandom.Next(26);
            letter2 = (char)('A'+nextRandomIndex);

            nextRandomIndex = systemRandom.Next(26);
            letter3 = (char)('A'+nextRandomIndex);

            number = $"{systemRandom.Next(999):D3}";
            
            return string.Concat(letter1, letter2, letter3)+"-"+number;
        }
        
        public static Color GetRandomColor() {
            return Random.ColorHSV();
        }
    }
}
