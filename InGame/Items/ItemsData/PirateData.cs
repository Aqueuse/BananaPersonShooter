using System.Collections.Generic;
using Save.Helpers;

namespace InGame.Items.ItemsData {
    public class PirateData {
        public string associatedSpaceshipGuid;

        public string piratePosition;
        public string pirateRotation;
        
        public Dictionary<DroppedType, int> piratesInventory = new() {
            {DroppedType.ELECTRONIC, 0},
            {DroppedType.BANANA_PEEL, 0},
            {DroppedType.METAL, 0},
            {DroppedType.FABRIC, 0},
            {DroppedType.BATTERY, 0}
        };

        public PirateState pirateState = PirateState.GO_TO_SAS;
        public string destination = JsonHelper.FromVector3ToString(ObjectsReference.Instance.spaceTrafficControlManager.teleportUpTransform.position);
    }
}