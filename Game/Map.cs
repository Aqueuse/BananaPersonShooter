using System.Collections.Generic;
using Gestion;
using Gestion.Buildables.Plateforms;
using Data.Maps;
using Enums;
using Game.Steam;
using Monkeys;
using Tags;
using UnityEngine;

namespace Game {
    public class Map : MonoBehaviour {
        public MapDataScriptableObject mapDataScriptableObject;
        
        public bool isDiscovered;

        public int debrisToSpawn;

        public float cleanliness;

        public int maxDebrisQuantity;
        private int _actualDebrisQuantity;

        public GameObject initialAspirablesOnMap;

        public List<Vector3> itemsPositions;
        public List<Quaternion> itemsRotations;

        public List<ItemCategory> itemsCategories;
        public List<int> itemsPrefabsIndex;
        public List<BuildableType> itemsBuildableTypes;
        public List<BananaType> itemBananaTypes;

        public List<PortalDestination> portals;

        private void Start() {
            maxDebrisQuantity = 99;
            debrisToSpawn = 0;
        }

        public void Init() {
            if (mapDataScriptableObject.monkeyDataScriptableObjectsByMonkeyId.Count > 0) {
                foreach (var monkey in MapItems.Instance.monkeys) {
                    RecalculateHappiness(monkey);
                }
            }
        }

        public void RecalculateHappiness(Monkey monkey) {
            _actualDebrisQuantity = MapItems.Instance.aspirablesContainer.GetComponentsInChildren<MeshFilter>().Length;

            cleanliness = 50-_actualDebrisQuantity /(float)maxDebrisQuantity*50;

            if (cleanliness >= 50 && ObjectsReference.Instance.steamIntegration.isGameOnSteam) {
                ObjectsReference.Instance.steamIntegration.UnlockAchievement(SteamAchievement.STEAM_ACHIEVEMENT_JUNGLE_CLEANED); 
            }

            monkey.monkeyDataScriptableObject.happiness = monkey.monkeyDataScriptableObject.sasiety + cleanliness;

            if (monkey.monkeyDataScriptableObject.happiness < 20 && monkey.monkeyState != MonkeyState.ANGRY) {
                monkey.monkeyState = MonkeyState.ANGRY;
                monkey.monkeySounds.PlayRoarsSounds();
            }

            if (monkey.monkeyDataScriptableObject.happiness is >= 20 and < 60 && monkey.monkeyState != MonkeyState.SAD) {
                monkey.monkeyState = MonkeyState.SAD;
                monkey.monkeySounds.PlayQuickMonkeySounds();
            }

            if (monkey.monkeyDataScriptableObject.happiness >= 60 && monkey.monkeyState != MonkeyState.HAPPY) {
                monkey.monkeyState = MonkeyState.HAPPY;
                monkey.monkeySounds.PlayQuickMonkeySounds();
            }
        }

        public void RefreshItemsDataMap() {
            if (MapItems.Instance == null) return;

            itemsCategories = new List<ItemCategory>();
            itemsPositions = new List<Vector3>();
            itemsRotations = new List<Quaternion>();
            itemsPrefabsIndex = new List<int>();
            itemsBuildableTypes = new List<BuildableType>();
            itemBananaTypes = new List<BananaType>();
            
            // get the debris list
            var itemsList = MapItems.Instance.GetAllItemsInAspirableContainer();
            
            foreach (var item in itemsList) {
                var itemData = item.GetComponent<Tag>().itemScriptableObject;

                itemsCategories.Add(itemData.itemCategory);
                itemsPositions.Add(item.transform.position);
                itemsRotations.Add(item.transform.rotation);
                itemsPrefabsIndex.Add(itemData.prefabIndex);
                itemsBuildableTypes.Add(itemData.buildableType);

                itemBananaTypes.Add(itemData.buildableType == BuildableType.PLATEFORM
                    ? item.GetComponent<Plateform>().plateformType
                    : itemData.bananaType);
            }
        }

        public int GetDebrisQuantity() {
            return debrisToSpawn+TagsManager.Instance.GetAllGameObjectsWithTag(GAME_OBJECT_TAG.DEBRIS).Count;
        }

        public void StartBossFight(MonkeyType monkeyType) {
            ObjectsReference.Instance.audioManager.PlayMusic(MusicType.FIGHT, 0);
        }
    }
}