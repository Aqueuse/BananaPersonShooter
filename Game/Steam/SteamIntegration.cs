using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Steam {
    public enum SteamAchievement {
        STEAM_ACHIEVEMENT_BANAGUN_RECONSTRUCTED,
        STEAM_ACHIEVEMENT_JUNGLE_CLEANED,
        STEAM_ACHIEVEMENT_MONKEY_FEEDED
    }

    public class SteamIntegration : MonoBehaviour {
        // game steam id : 2236270
        public bool isGameOnSteam;

        private Dictionary<SteamAchievement, string> steamIdByAchievement;

        private void Start() {
            if (!isGameOnSteam) return;
            
            try {
                Steamworks.SteamClient.Init(2236270);
            }
            catch (Exception e) {
                isGameOnSteam = false;
            }

            steamIdByAchievement = new Dictionary<SteamAchievement, string> {
                {SteamAchievement.STEAM_ACHIEVEMENT_BANAGUN_RECONSTRUCTED, "STEAM_ACHIEVEMENT_BANAGUN_RECONSTRUCTED"}, 
                {SteamAchievement.STEAM_ACHIEVEMENT_MONKEY_FEEDED, "STEAM_ACHIEVEMENT_MONKEY_FEEDED"}, 
                {SteamAchievement.STEAM_ACHIEVEMENT_JUNGLE_CLEANED, "STEAM_ACHIEVEMENT_JUNGLE_CLEANED"} 
            };
        }

        private void Update() {
            Steamworks.SteamClient.RunCallbacks();
        }

        private bool IsAchievementUnlocked(SteamAchievement steamAchievement) {
            var achievement = new Steamworks.Data.Achievement(steamIdByAchievement[steamAchievement]);

            return achievement.State;
        }

        public void UnlockAchievement(SteamAchievement steamAchievement) {
            if (!isGameOnSteam) return;
            
            var achievement = new Steamworks.Data.Achievement(steamIdByAchievement[steamAchievement]);
            if (!IsAchievementUnlocked(steamAchievement)) achievement.Trigger();
        }

        private void OnApplicationQuit() {
            Steamworks.SteamClient.Shutdown();
        }
    }
}
