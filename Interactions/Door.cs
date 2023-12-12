using ItemsProperties.Doors;
using UnityEngine;

namespace Interactions {
    public class Door : MonoBehaviour {
        public SceneType destinationMap;
        public SpawnPoint spawnPoint;
        public DoorPropertiesScriptableObject doorPropertiesScriptableObject;
    }
}
