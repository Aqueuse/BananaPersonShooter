using Game;
using UnityEngine;

namespace UI.InGame.Statistics {
    public class UIStatisticsSlot : MonoBehaviour {
        [SerializeField] private Map map;
        
        public void OnClick() {
            ObjectsReference.Instance.uiStatistics.Refresh_Map_Statistics(map.mapName);
        }
    }
}
