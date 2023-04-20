using UnityEngine;

public class DontDestroy : MonoBehaviour {
    private const string GlobalContainerTag = "Global Container";

    private void Awake() {
        GameObject[] globalContainers = GameObject.FindGameObjectsWithTag(GlobalContainerTag);

        if (globalContainers.Length > 1) {
            Destroy(GameObject.FindWithTag(GlobalContainerTag));
        }

        DontDestroyOnLoad(GameObject.FindWithTag(GlobalContainerTag));
    }
}
