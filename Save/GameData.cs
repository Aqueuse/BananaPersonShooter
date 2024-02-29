using System.Collections.Generic;
using InGame.Items.ItemsData;
using Save.Templates;
using UnityEngine;

namespace Save {
    public class GameData : MonoBehaviour {
        public string currentSaveUuid;
        
        public BananaManSavedData bananaManSaved = new();

        public float nextAdCampaignTimer;
        
        public Vector3 lastPositionOnMap;
        public Vector3 lastRotationOnMap;
        
        public Dictionary<string, Vector3> portalsTeleportPositionByName;
        
        public WorldData worldData;
    }
}
