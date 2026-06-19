using System.Collections.Generic;
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
    }
}