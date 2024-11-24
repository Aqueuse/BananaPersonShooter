using System.Collections.Generic;
using Save.Helpers;

namespace InGame.Items.ItemsData.Characters {
    public class PirateData {
        public string associatedSpaceshipGuid;

        public string piratePosition;
        public string pirateRotation;
        
        public Dictionary<RawMaterialType, int> piratesInventory = new() {
            {RawMaterialType.ELECTRONIC, 0},
            {RawMaterialType.BANANA_PEEL, 0},
            {RawMaterialType.METAL, 0},
            {RawMaterialType.FABRIC, 0},
            {RawMaterialType.BATTERY, 0}
        };

        public PirateState pirateState = PirateState.GO_TO_TELEPORTER;
        public string destination = JsonHelper.FromVector3ToString(ObjectsReference.Instance.gameManager.spawnPointsBySpawnType[SpawnPoint.TP_HANGARS].position);
    }
}