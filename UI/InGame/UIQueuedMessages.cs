using UnityEngine;

namespace UI.InGame {
    public class UIQueuedMessages : MonoBehaviour {
        [SerializeField] private GameObject queuedMessagePrefab;
    
        public void AddMessage(string message) {
            var queuedMessage = Instantiate(queuedMessagePrefab, transform);
        
            queuedMessage.GetComponent<UIMessage>().SetMessage(message);
        }
    }
}
