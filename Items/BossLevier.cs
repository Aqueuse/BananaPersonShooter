using Bosses;
using Enums;
using UnityEngine;

public class BossLevier : MonoBehaviour {
    [SerializeField] private BossType bossType;

    public void ActivateBoss() {
        BossManager.Instance.activeBossType = bossType;
        BossManager.Instance.StartBossFight(bossType);
    }
}
