using System.Collections.Generic;
using Gestion.BuildablesBehaviours;
using ItemsProperties.Maps;
using UnityEngine;

namespace Game {
    public class MapData : MonoBehaviour {
        public MapPropertiesScriptableObject mapPropertiesScriptableObject;
        
        public bool isDiscovered;

        public List<CharacterType> debrisToSPawnByCharacterType;

        public int piratesDebrisToSpawn;
        public int visitorsDebrisToSpawn;

        private int _actualDebrisQuantity;

        public GameObject initialAspirablesOnMap;
        
        public List<PortalDestination> portals;

        public GenericDictionary<BuildableType, List<string>> buildablesDataInMapDictionaryByBuildableType;
        public GenericDictionary<CharacterType, List<string>> debrisDataInMapDictionnaryByCharacterType;

        public void AddBuildableToBuildableDictionnary(BuildableType buildableType, string buildableData) {
            if (buildablesDataInMapDictionaryByBuildableType.ContainsKey(buildableType)) {
                buildablesDataInMapDictionaryByBuildableType[buildableType].Add(buildableData);
            }
            else {
                buildablesDataInMapDictionaryByBuildableType.Add(buildableType, new List<string>{ buildableData });
            }
        }

        public void AddDebrisToDebrisDictionnary(CharacterType debrisType, string debrisData) {
            if (debrisDataInMapDictionnaryByCharacterType.ContainsKey(debrisType)) {
                debrisDataInMapDictionnaryByCharacterType[debrisType].Add(debrisData);
            }
            else {
                debrisDataInMapDictionnaryByCharacterType.Add(debrisType, new List<string>{ debrisData });
            }
        }
    }
}