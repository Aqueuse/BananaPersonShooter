using System.Collections.Generic;
using InGame.Items.ItemsBehaviours;
using InGame.Items.ItemsBehaviours.BuildablesBehaviours;
using InGame.Items.ItemsProperties.Monkeys;
using InGame.Monkeys;
using InGame.Monkeys.Ancestors;
using UnityEngine;

namespace InGame.Items.ItemsData {
    public class WorldData : MonoBehaviour {
        public List<MonkeyMenBehaviour> monkeysMensInStation;
        
        public GameObject initialBuildablesOnWorld;
        public GameObject initialSpaceshipDebrisOnWorld;
        
        public List<PortalDestination> portals;

        public List<Monkey> monkeys;

        public List<MonkeyPropertiesScriptableObject> monkeysData;
        
        public int GetDroppedQuantityInWorld() {
            return FindObjectsByType<SpaceshipDebrisBehaviour>(FindObjectsSortMode.None).Length;
        }
    }
}