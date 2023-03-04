using Game;
using TMPro;
using UI.InGame.Statistics;
using UnityEngine;
using UnityEngine.UI;

namespace UI.InGame {
    public class Uihud : MonoSingleton<Uihud> {
        [SerializeField] private CanvasGroup inventoryCanvasGroup;
        [SerializeField] private CanvasGroup statisticsCanvasGroup;

        [SerializeField] private GameObject inventoryButton;
        [SerializeField] private GameObject statisticsButton;
    
        [SerializeField] Color activatedColor;
        [SerializeField] Color unactivatedColor;

        public void Switch_To_Inventory() {
            UIManager.Instance.Set_active(statisticsCanvasGroup, false);
            UIManager.Instance.Set_active(inventoryCanvasGroup, true);

            inventoryButton.GetComponent<Image>().color = activatedColor;
            inventoryButton.GetComponentInChildren<TextMeshProUGUI>().color = Color.black;
            
            statisticsButton.GetComponent<Image>().color = unactivatedColor;
            statisticsButton.GetComponentInChildren<TextMeshProUGUI>().color = activatedColor;
        }

        public void Switch_To_Statistics() {
            UIStatistics.Instance.Refresh_Map_Statistics("MAP01");
            
            UIManager.Instance.Set_active(inventoryCanvasGroup, false);
            UIManager.Instance.Set_active(statisticsCanvasGroup, true);

            statisticsButton.GetComponent<Image>().color = activatedColor;
            statisticsButton.GetComponentInChildren<TextMeshProUGUI>().color = Color.black;
            
            inventoryButton.GetComponent<Image>().color = unactivatedColor;
            inventoryButton.GetComponentInChildren<TextMeshProUGUI>().color = activatedColor;
        }
    }
}
