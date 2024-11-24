namespace InGame.Items.ItemsData.Characters {
    
    public class TouristData {
        public GenericDictionary<NeedType, int> touristNeeds;

        public RegionType RegionType;
        
        private string touristGuid;
        public string touristPosition;
        public string touristRotation;
        public CharacterType characterType;
        public TouristState touristState;
        public int notoriety;
        
        // APPARENCE
        public float textureRotation;
        public int prefabIndex;
        public string touristName;
    }
}