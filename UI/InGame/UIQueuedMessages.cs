using UnityEngine;

namespace UI.InGame {
    public class UIQueuedMessages : MonoSingleton<UIQueuedMessages> {
        [SerializeField] private GameObject queuedMessagePrefab;
    
        public void AddMessage(string message) {
            var queuedMessage = Instantiate(queuedMessagePrefab, transform);
        
            queuedMessage.GetComponent<UIMessage>().SetMessage(message);
        }
    }
}
