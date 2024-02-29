using System.Collections.Generic;
using InGame.Items.ItemsBehaviours.BuildablesBehaviours;
using UnityEngine;

namespace InGame.Items.ItemsData {
    public class WorldData : MonoBehaviour {

        public List<CharacterType> debrisToSPawnByCharacterType;

        public int piratesDebrisToSpawn;
        public int visitorsDebrisToSpawn;
        public int merchantsDebrisToSpawn;
        
        public int piratesDebris;
        public int visitorsDebris;

        public int piratesQuantity;
        public int visitorsQuantity;
        public int chimployeesQuantity;
        
        private int _actualDebrisQuantity;
        public GameObject initialAspirablesOnWorld;
        
        public List<PortalDestination> portals;

        public GenericDictionary<BuildableType, List<string>> buildablesDataDictionaryByBuildableType;
        public GenericDictionary<CharacterType, List<string>> debrisDataDictionnaryByCharacterType;

        public GenericDictionary<string, Vector3> monkeysPositionByMonkeyId;
        public GenericDictionary<string, int> monkeysSasietyTimerByMonkeyId;
        
        public void AddBuildableToBuildableDictionnary(BuildableType buildableType, string buildableData) {
            if (buildablesDataDictionaryByBuildableType.ContainsKey(buildableType)) {
                buildablesDataDictionaryByBuildableType[buildableType].Add(buildableData);
            }
            else {
                buildablesDataDictionaryByBuildableType.Add(buildableType, new List<string>{ buildableData });
            }
        }

        public void AddDebrisToDebrisDictionnary(CharacterType debrisType, string debrisData) {
            if (debrisDataDictionnaryByCharacterType.ContainsKey(debrisType)) {
                debrisDataDictionnaryByCharacterType[debrisType].Add(debrisData);
            }
            else {
                debrisDataDictionnaryByCharacterType.Add(debrisType, new List<string>{ debrisData });
            }
        }
    }
}