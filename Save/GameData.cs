using Save.Templates;
using UnityEngine;

namespace Save {
    public class GameData : MonoBehaviour {
        public string currentSaveUuid;
        
        public BananaManSavedData bananaManSavedData = new();
        
        public Vector3 lastPositionOnMap;
        public Vector3 lastRotationOnMap;
        
        public GenericDictionary<string, MapSavedData> mapSavedDatasByMapName;

        private void Start() {
            mapSavedDatasByMapName = new GenericDictionary<string, MapSavedData>();
            
            foreach (var map in ObjectsReference.Instance.mapsManager.mapBySceneName) {
                mapSavedDatasByMapName.Add(map.Key, new MapSavedData());
            }
        }
    }
}
