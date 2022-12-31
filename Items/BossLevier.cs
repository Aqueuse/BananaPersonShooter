using Enums;
using Monkeys;
using UnityEngine;

public class BossLevier : MonoBehaviour {
    [SerializeField] private MonkeyType monkeyType;

    public void ActivateBoss() {
        MonkeyManager.Instance.activeMonkeyType = monkeyType;
        MonkeyManager.Instance.StartBossFight(monkeyType);
    }
}
