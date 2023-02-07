using Enums;
using UnityEngine;

namespace UI.InGame.Statistics {
    public class UIStatisticsSlot : MonoBehaviour {
        [SerializeField] private MonkeyType monkeyType;
        
        public void OnClick() {
            UIStatistics.Instance.Refresh_Map_Statistics(monkeyType);
        }
    }
}
