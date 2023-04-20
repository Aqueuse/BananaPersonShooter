using UnityEngine;

namespace Game {
    public class MapsManager : MonoBehaviour {
        public GenericDictionary<string, Map> mapBySceneName;

        public Map currentMap;
    }
}