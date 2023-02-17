namespace Game {
    public class MapsManager : MonoSingleton<MapsManager> {
        public GenericDictionary<string, Map> mapBySceneName;

        public Map currentMap;
    }
}