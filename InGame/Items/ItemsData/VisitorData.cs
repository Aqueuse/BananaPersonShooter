namespace InGame.Items.ItemsData {
    
    public class VisitorData {
        public GenericDictionary<NeedType, int> visitorNeeds;

        public RegionType RegionType;
        
        private string visitorGuid;
        public string visitorPosition;
        public string visitorRotation;
        public CharacterType characterType;
        public VisitorState visitorState;
        public int notoriety;
        
        // APPARENCE
        public float textureRotation;
        public int prefabIndex;
        public string visitorName;
    }
}