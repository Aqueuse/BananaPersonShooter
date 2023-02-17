using Game;
using Save.Templates;
using UnityEngine;

namespace Save {
    public class GameData : MonoSingleton<GameData> {
        [SerializeField] public GameObject[] debrisPrefab;

        public string currentSaveUuid;
        
        public BananaManSavedData BananaManSavedData = new BananaManSavedData();
        
        public Vector3 lastPositionOnMap;
        
        public GameObject debrisContainer;

        public GenericDictionary<string, MAPSavedData> mapSavedDatasByMapName;

        private void Start() {
            mapSavedDatasByMapName = new GenericDictionary<string, MAPSavedData>();
            
            foreach (var map in MapsManager.Instance.mapBySceneName) {
                mapSavedDatasByMapName.Add(map.Key, new MAPSavedData());
            }
        }
    }
}
