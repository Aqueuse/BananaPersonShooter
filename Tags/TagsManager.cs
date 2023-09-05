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
        MOVER_UNVALID,
        CANNON_MINI_GAME_PROJECTILE,
        CANNON_MINI_GAME_LIMITS,
        CANNON_MINI_GAME_DOME,
        DOOR,
        BUILDABLE,
        DEBRIS,
        REGIME,
        RUINE,
        MONKEY,
        MINI_CHIMP,
        CHIMPLOYEE,
        VISITOR,
        COMMAND_ROOM_PANEL,
        RAW_MATERIAL,
        INGREDIENT
    }

    public class TagsManager : MonoSingleton<TagsManager> {
        public GameObject GetFirstGameObjectWithTag(GAME_OBJECT_TAG myTag) {
            var gameObjects = FindObjectsOfType<Tag>();

            foreach (var objectTag in gameObjects) {
                if (objectTag.gameObjectTag == myTag) return objectTag.gameObject;
            }

            return null;
        }

        public List<GameObject> GetAllGameObjectsWithTag(GAME_OBJECT_TAG myTag) {
            var gameObjects = FindObjectsOfType<Tag>();

            var gameObjectsWithTag = new List<GameObject>();
            
            foreach (var objectTag in gameObjects) {
                if (objectTag.gameObjectTag == myTag) gameObjectsWithTag.Add(objectTag.gameObject);
            }

            return gameObjectsWithTag;
        }

        public GAME_OBJECT_TAG GetTag(GameObject myGameObject) {
            if (myGameObject.GetComponent<Tag>() != null) return myGameObject.GetComponent<Tag>().gameObjectTag;
            return GAME_OBJECT_TAG.UNTAGGED;
        }

        public void SetTag(GameObject myGameObject, GAME_OBJECT_TAG myTag) {
            if (myGameObject.GetComponent<Tag>() != null) myGameObject.GetComponent<Tag>().gameObjectTag = myTag;
        }

        public bool HasTag(GameObject myGameObject, GAME_OBJECT_TAG myTag) {
            if (myGameObject.GetComponent<Tag>() == null) {
                Debug.Log("GameObject has no Tag script component");
                return false;
            }

            return myGameObject.GetComponent<Tag>().gameObjectTag == myTag;
        }
    }
}