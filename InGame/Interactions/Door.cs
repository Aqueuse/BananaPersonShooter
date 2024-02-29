using InGame.Items.ItemsProperties.Doors;
using UnityEngine;

namespace InGame.Interactions {
    public class Door : MonoBehaviour {
        public RegionType destinationMap;
        public SpawnPoint spawnPoint;
        public DoorPropertiesScriptableObject doorPropertiesScriptableObject;
    }
}
