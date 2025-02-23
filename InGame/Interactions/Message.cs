using UnityEngine;

namespace InGame.Interactions {
    public class Message : MonoBehaviour {
        [SerializeField] private GameObject messageGameObject;

        public void ShowHideMessage() {
            messageGameObject.SetActive(!messageGameObject.activeInHierarchy);
        }
    }
}