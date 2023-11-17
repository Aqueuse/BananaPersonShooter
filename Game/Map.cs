using System.Collections.Generic;
using Gestion;
using Gestion.Buildables.Plateforms;
using Data.Maps;
using Enums;
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
                if (item.GetComponent<Tag>().gameObjectTag != GAME_OBJECT_TAG.BUILDABLE) break;
                
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

        public void RecalculateCleanliness() {
            _actualDebrisQuantity = MapItems.Instance.aspirablesContainer.GetComponentsInChildren<MeshFilter>().Length;
            ObjectsReference.Instance.mapsManager.currentMap.cleanliness = 50-_actualDebrisQuantity /(float)maxDebrisQuantity*50;

        }
    }
}