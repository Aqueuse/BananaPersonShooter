using System.Collections.Generic;
using UnityEngine;

namespace Tags {
    public enum GAME_OBJECT_TAG {
        UNTAGGED,
        GLOBAL_CONTAINER,
        VEGETATION_MASK,
        BANANA,
        PLAYER,
        TELEPORTATION,
        BUILD_UNVALID,
        LASER,
        DOME,
        SPACESHIP,
        DOOR,
        BUILDABLE,
        DROPPED,
        REGIME,
        MONKEY,
        MINI_CHIMP,
        CHIMPLOYEE,
        TOURIST,
        COMMAND_ROOM_PANEL,
        RAW_MATERIAL,
        INGREDIENT,
        ACCESS_MANAGED,
        RETRIEVER_ROTATING_LOGO,
        PIRATE,
        MERCHANT,
        FOOD,
        MANUFACTURED_ITEM,
        CULTIVATOR,
        CHIMPMEN,
        WASTE,
        BLUEPRINT
    }

    public class TagsManager : MonoSingleton<TagsManager> {
        public GameObject GetFirstGameObjectWithTag(GAME_OBJECT_TAG myTag) {
            var gameObjects = FindObjectsOfType<Tag>();

            foreach (var objectTag in gameObjects) {
                if (objectTag.gameObjectTag == myTag) return objectTag.gameObject;
            }

            return null;
        }

        public static GameObject GetFirstGameObjectWithTagInGameObject(GameObject parent, GAME_OBJECT_TAG myTag) {
            var gameObjects = parent.gameObject.GetComponentsInChildren<Tag>();

            foreach (var objectTag in gameObjects) {
                if (objectTag.gameObjectTag == myTag) return objectTag.gameObject;
            }

            return null;
        }

        public static List<GameObject> GetAllGameObjectsWithTag(GAME_OBJECT_TAG myTag) {
            var gameObjects = FindObjectsOfType<Tag>();

            var gameObjectsWithTag = new List<GameObject>();
            
            foreach (var objectTag in gameObjects) {
                if (objectTag.gameObjectTag == myTag) gameObjectsWithTag.Add(objectTag.gameObject);
            }

            return gameObjectsWithTag;
        }
        
        public bool HasTag(GameObject myGameObject, GAME_OBJECT_TAG myTag) {
            if (myGameObject.GetComponent<Tag>() == null) {
                return false;
            }

            return myGameObject.GetComponent<Tag>().gameObjectTag == myTag;
        }
    }
}