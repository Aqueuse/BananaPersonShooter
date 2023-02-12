using System;
using System.Collections.Generic;
using Data;
using Enums;
using Save.Templates;
using UnityEngine;

namespace Save {
    public class GameData : MonoSingleton<GameData> {
        [SerializeField] public GameObject[] debrisPrefab;
        
        public BananaManSavedData bananaManSavedData = new BananaManSavedData();
        public MAP01SavedData map01SavedData = new MAP01SavedData();
        
        public Vector3[] debrisPosition;
        public Quaternion[] debrisRotation;
        public int[] debrisPrefabIndex;
        
        public Vector3 lastPositionOnMap;
        
        public GameObject debrisContainer;

        public GenericDictionary<String, MapDataScriptableObject> mapDatasBySceneNames;
    }
}
