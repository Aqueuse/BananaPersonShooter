using System.Collections.Generic;
using Tags;
using UnityEngine;

public class DontDestroy : MonoBehaviour {
    private List<GameObject> globalContainers;

    private void Awake() {
        globalContainers = TagsManager.Instance.GetAllGameObjectsWithTag(GAME_OBJECT_TAG.GLOBAL_CONTAINER);
        
        if (globalContainers.Count > 1) {
            Destroy(TagsManager.Instance.GetFirstGameObjectWithTag(GAME_OBJECT_TAG.GLOBAL_CONTAINER));
        }
        
        DontDestroyOnLoad( TagsManager.Instance.GetFirstGameObjectWithTag(GAME_OBJECT_TAG.GLOBAL_CONTAINER));
    }
}
