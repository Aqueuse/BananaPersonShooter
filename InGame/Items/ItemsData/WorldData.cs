using System.Collections.Generic;
using InGame.Items.ItemsBehaviours;
using InGame.Items.ItemsProperties.Monkeys;
using InGame.Monkeys.Ancestors;
using InGame.Monkeys.Chimpvisitors;
using UnityEngine;

namespace InGame.Items.ItemsData {
    public class WorldData : MonoBehaviour {
        public List<VisitorBehaviour> monkeysMensInStation;
        
        public GameObject initialSpaceshipDebrisOnWorld;
        
        public List<Monkey> monkeys;

        public List<MonkeyPropertiesScriptableObject> monkeysData;

        public int stationLightSetting;
        public int lastVisitorGroup;
        public int lastMerchimpGroup;
        
        public int GetDroppedQuantityInWorld() {
            return FindObjectsByType<SpaceshipDebrisBehaviour>(FindObjectsSortMode.None).Length;
        }
    }
}