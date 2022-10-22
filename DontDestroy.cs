using UnityEngine;

public class DontDestroy : MonoBehaviour {
    private void Awake() {
        GameObject[] globalContainers = GameObject.FindGameObjectsWithTag("Global Container");
        
        if (globalContainers.Length > 1) {
            Destroy(GameObject.FindWithTag("Global Container"));
        }
        
        
        DontDestroyOnLoad(GameObject.FindWithTag("Global Container"));
    }
}
