using UnityEngine;

namespace UI.InGame.Statistics {
    public class UIStatisticsSlot : MonoBehaviour {
        [SerializeField] private string mapName;
        
        public void OnClick() {
            UIStatistics.Instance.Refresh_Map_Statistics(mapName);
        }
    }
}
