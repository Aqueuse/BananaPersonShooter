using System.Collections.Generic;
using Game;
using Save.Templates;
using UnityEngine;

namespace Save {
    public class GameData : MonoBehaviour {
        public string currentSaveUuid;
        
        public BananaManSavedData bananaManSaved = new();
        
        public Vector3 lastPositionOnMap;
        public Vector3 lastRotationOnMap;
        
        public Dictionary<string, Vector3> portalsTeleportPositionByName;
        
        public GenericDictionary<SceneType, MapData> mapBySceneName;

        public MapData currentMapData;
    }
}
