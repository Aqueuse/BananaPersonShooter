using System.Collections.Generic;
using InGame.Items.ItemsBehaviours;
using InGame.Items.ItemsBehaviours.BuildablesBehaviours;
using InGame.Items.ItemsProperties.Monkeys;
using InGame.Monkeys.Ancestors;
using UnityEngine;

namespace InGame.Items.ItemsData {
    public class WorldData : MonoBehaviour {
        public int piratesQuantity;
        public int visitorsQuantity;
        public int chimployeesQuantity;
        
        public GameObject initialBuildablesOnWorld;
        public GameObject initialDebrisOnWorld;
        
        public List<PortalDestination> portals;

        public List<Monkey> monkeys;

        public List<MonkeyPropertiesScriptableObject> monkeysData;
        
        public int GetDebrisQuantityInWorld() {
            return FindObjectsByType<DebrisBehaviour>(FindObjectsSortMode.None).Length;
        }
    }
}