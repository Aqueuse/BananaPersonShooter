using System.Collections.Generic;
using InGame.Items.ItemsBehaviours;
using UnityEngine;
using Random = UnityEngine.Random;

namespace InGame.SpaceTrafficControl {
    public class SpaceTrafficControlManager : MonoBehaviour {
        public GenericDictionary<string, SpaceshipBehaviour> spaceshipBehavioursByGuid;
        public GenericDictionary<int, bool> hangarAvailabilityByHangarNumber;
        
        public readonly List<Vector3[]> pathsToHangars = new() {
                new Vector3[] {new(3427.478f,2199.609f,-1796f), new(2361.705f,-20.18628f,-1868.172f), new(-750.5123f,-1298.092f,-2555.486f)},
                new Vector3[] {new(429.1329f,1886.956f,1293.362f), new(-354.4004f,-20.18628f,69.54077f), new(-1281.391f,-1298.092f,-1604.781f)},
                new Vector3[] {new (-3455.312f,1886.956f,1691.814f), new(-3058.228f,-20.18628f,520.2107f), new(-2359.748f,-1298.092f,-1466.763f)},
                new Vector3[] {new (-6593.394f,1886.956f,-415.9173f), new(-5093.8f,-20.18628f,-1318.29f), new(-3016.645f,-1298.092f,-2301.683f)},
                new Vector3[] {new (-7442.096f,1886.956f,-3788.963f), new(-5264.229f,-20.18628f,-3549.354f), new(-2825.072f,-1298.092f,-3059.063f)},
                new Vector3[] {new (-4804.958f,1886.956f,-7220.742f), new(-3369.886f,-20.18628f,-5677.006f), new(-2151.887f,-1298.092f,-3638.466f)},
                new Vector3[] {new (415.8911f,1886.956f,-7253.578f), new(-458.3247f,-20.18628f,-5726.918f), new(-1385.756f,-1298.092f,-3664.557f)},
                new Vector3[] {new (3313.797f,1886.956f,-4828.157f), new(1230.392f,-20.18628f,-4128.994f), new(-878.1141f,-1298.092f,-3127.486f)}
        };
        
        public readonly List<Vector3[]> elevatorsPaths = new() {
            new Vector3[] {new(-1605.884f,-1153.465f,-2702.111f), new(-1605.884f,147.19f,-2702.111f)},
            new Vector3[] {new(-1696.109f,-1196.047f,-2598.885f), new(-1696.109f,147.19f,-2598.885f)},
            new Vector3[] {new(-1832.673f,-1196.047f,-2593.439f), new(-1835.777f,147.19f,-2595.016f)},
            new Vector3[] {new(-1950.893f,-1196.047f,-2701.864f), new(-1950.893f,147.19f,-2701.864f)},
            new Vector3[] {new(-1952.195f,-1196.047f,-2837.615f), new(-1952.195f,147.19f,-2837.615f)},
            new Vector3[] {new(-1816.3f,-1196.047f,-2925.192f), new(-1817.307f,147.19f,-2925.696f)},
            new Vector3[] {new(-1696.243f,-1196.047f,-2921.79f), new(-1696.243f,147.19f,-2921.79f)},
            new Vector3[] {new(-1600.332f,-1196.047f,-2826.458f), new(-1600.332f,147.19f,-2826.458f)}
        };
        
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
            ObjectsReference.Instance.uiCommunicationPanel.RefreshCommunicationButton();
        }

        public void AssignToHangarFromCommunications(int hangarNumber) {
            // index are shifted in the UI because we start to count at 1 instead of 0
            ObjectsReference.Instance.spaceTrafficControlManager.AssignSpaceshipToHangar(hangarNumber);
            ObjectsReference.Instance.uiCommunicationPanel.CloseCommunications(selectedSpaceship);
            selectedSpaceship.GoToPath(hangarNumber);
        }

        public void FreeHangar(int hangarNumber) {
            hangarAvailabilityByHangarNumber[hangarNumber] = true;
            
            ObjectsReference.Instance.uiCommunicationPanel.RefreshHangarAvailability();
            ObjectsReference.Instance.uiCommunicationPanel.RefreshCommunicationButton();
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
