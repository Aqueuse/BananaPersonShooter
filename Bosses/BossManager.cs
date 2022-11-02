using UI.InGame;
using UnityEngine;

namespace Bosses {
    public class BossManager : MonoSingleton<BossManager> {
        [SerializeField] private GameObject gorilleBossGameObject;
        public UIBoss associatedUI;
    
        public GameObject GetGorillaBoss() {
            return gorilleBossGameObject;
        }

        public void StartBossFight() {
            associatedUI.Show_Boss_Life_Slider();
        }

        public void EndBossFight(bool isWinned) {
            associatedUI.Hide_Boss_Life_Slider();
            if (isWinned) {
                Debug.Log("you win !");
            }
        }
    }
}
