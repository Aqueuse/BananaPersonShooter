using System;

namespace InGame.Items.ItemsData.Dropped {
    [Serializable]
    public class FoodData {
        public string droppedGuid;
        public string droppedPosition;
        public string droppedRotation;
        public FoodType foodType;
    }
}
