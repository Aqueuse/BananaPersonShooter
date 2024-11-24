using System;

namespace InGame.Items.ItemsData.Dropped {
    [Serializable]
    public class IngredientData {
        public string droppedGuid;
        public string droppedPosition;
        public string droppedRotation;
        public IngredientsType IngredientsType;
    }
}
