using UnityEngine;

namespace InGame.Items.ItemsData {
    public class SpaceshipData : MonoBehaviour {
        public string spaceshipGuid;
        public string spaceshipName;
        public Color spaceshipUIcolor;
        public int communicationMessagePrefabIndex;

        public CharacterType characterType;
        public SpaceshipType spaceshipType;

        public int assignatedHangar;

        public int monkeyMenToSpawn;

        public Vector3 arrivalPosition;
        
        public const float lookAtRotation = 0.01f;
        public const float _propulsionSpeed = 100f;

        public TravelState travelState;
    }
}
