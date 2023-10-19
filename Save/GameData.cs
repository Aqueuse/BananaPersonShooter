using System.Collections.Generic;
using Save.Templates;
using UnityEngine;

namespace Save {
    public class GameData : MonoBehaviour {
        public string currentSaveUuid;
        
        public BananaManSavedData bananaManSavedData = new();
        
        public Vector3 lastPositionOnMap;
        public Vector3 lastRotationOnMap;
        
        public GenericDictionary<string, MapSavedData> mapSavedDatasByMapName;

        public Dictionary<string, Vector3> portalsTeleportPositionByName;

        private void Start() {
            mapSavedDatasByMapName = new GenericDictionary<string, MapSavedData>();
            
            foreach (var map in ObjectsReference.Instance.mapsManager.mapBySceneName) {
                var mapData = new MapSavedData();

                mapData.monkeysSasietyByMonkeyId = new Dictionary<string, float>();
                
                mapSavedDatasByMapName.Add(map.Key, mapData);
            }
        }
    }
}
