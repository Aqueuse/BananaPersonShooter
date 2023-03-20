using Game;
using Save.Templates;
using UnityEngine;

namespace Save {
    public class GameData : MonoSingleton<GameData> {
        [SerializeField] public GameObject[] debrisPrefab;
        [SerializeField] public GameObject plateformPrefab;

        public string currentSaveUuid;
        
        public BananaManSavedData bananaManSavedData = new BananaManSavedData();
        
        public Vector3 lastPositionOnMap;
        
        public GenericDictionary<string, MapSavedData> mapSavedDatasByMapName;

        private void Start() {
            mapSavedDatasByMapName = new GenericDictionary<string, MapSavedData>();
            
            foreach (var map in MapsManager.Instance.mapBySceneName) {
                mapSavedDatasByMapName.Add(map.Key, new MapSavedData());
            }
        }
    }
}
