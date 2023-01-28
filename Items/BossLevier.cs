using Enums;
using Monkeys;
using UnityEngine;

public class BossLevier : MonoBehaviour {
    [SerializeField] private MonkeyType monkeyType;

    public void ActivateBoss() {
        MapManager.Instance.activeMonkeyType = monkeyType;
        MapManager.Instance.StartBossFight(monkeyType);
    }
}
