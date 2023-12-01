using System.Collections.Generic;
using Gestion;
using Gestion.Buildables.Plateforms;
using Data.Maps;
using Tags;
using UnityEngine;

namespace Game {
    public class Map : MonoBehaviour {
        public MapDataScriptableObject mapDataScriptableObject;
        
        public bool isDiscovered;

        public List<CharacterType> debrisToSPawnByCharacterType;
        
        public int piratesDebrisToSpawn;
        public int visitorsDebrisToSpawn;
        
        public int maxDebrisQuantity;
        private int _actualDebrisQuantity;

        public GameObject initialAspirablesOnMap;

        public List<Vector3> itemsPositions;
        public List<Quaternion> itemsRotations;

        public List<ItemCategory> itemsCategories;
        public List<BuildableType> itemsBuildableTypes;
        public List<BananaType> itemBananaTypes;

        public List<Vector3> debrisPositions;
        public List<Quaternion> debrisRotations;
        public List<CharacterType> debrisTypes;
        
        public List<PortalDestination> portals;

        private void Start() {
            maxDebrisQuantity = 99;
            piratesDebrisToSpawn = 0;
        }
        
        public void RefreshItemsDataMap() {
            if (MapItems.Instance == null) return;

            itemsCategories = new List<ItemCategory>();
            itemsPositions = new List<Vector3>();
            itemsRotations = new List<Quaternion>();
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
                itemsBuildableTypes.Add(itemData.buildableType);

                itemBananaTypes.Add(itemData.buildableType == BuildableType.PLATEFORM
                    ? item.GetComponent<Plateform>().plateformType
                    : itemData.bananaType);
            }
        }

        public int GetDebrisQuantity() {
            return piratesDebrisToSpawn+TagsManager.Instance.GetAllGameObjectsWithTag(GAME_OBJECT_TAG.DEBRIS).Count;
        }
    }
}