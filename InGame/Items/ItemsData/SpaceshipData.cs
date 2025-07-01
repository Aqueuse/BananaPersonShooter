using System.Collections.Generic;
using InGame.Items.ItemsData.Characters;
using InGame.Monkeys.Chimpvisitors;
using UnityEngine;

namespace InGame.Items.ItemsData {
    public class SpaceshipData : MonoBehaviour {
        public string spaceshipGuid;
        public string spaceshipName;
        public Color spaceshipUIcolor;
        public int communicationMessagePrefabIndex;

        public CharacterType characterType;
        public SpaceshipType spaceshipType;
        
        public List<MonkeyMenData> monkeyMenDatas;

        public GroupTravelState groupTravelState;
        public SpawnPoint[] guichetsMapsToVisit;
        public Vector3[] mapPointInterests;

        public int assignatedHangar;

        public Vector3 arrivalPosition;
        
        public const float lookAtRotation = 0.01f;
        public const float _propulsionSpeed = 100f;

        public TravelState travelState;
    }
}
