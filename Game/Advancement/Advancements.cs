using System.Collections.Generic;
using Enums;
using UnityEngine;

namespace Game.Advancement {
    public class Advancements : MonoBehaviour {
        private Dictionary<AdvancementState, BuildableType[]> _buildableUnlockedByAdvancementState;

        private void Start() {
            _buildableUnlockedByAdvancementState = new Dictionary<AdvancementState, BuildableType[]> {
                { AdvancementState.PUT_PLATFORM, new [] { BuildableType.PLATEFORM } },
            };
        }

        public AdvancementState GetBestAdvancement() {
            var playerAdvancements = ObjectsReference.Instance.gameData.bananaManSavedData.playerAdvancements;

            if (playerAdvancements.Contains(AdvancementState.FREE_TIME)) return AdvancementState.FREE_TIME;

            if (playerAdvancements.Contains(AdvancementState.CLEANED_MAP) &&
                !playerAdvancements.Contains(AdvancementState.FREE_TIME)) {
                return AdvancementState.CLEANED_MAP;
            }
            
            if (playerAdvancements.Contains(AdvancementState.USE_BANANA_CANNON) &&
                !playerAdvancements.Contains(AdvancementState.CLEANED_MAP)) {
                return AdvancementState.USE_BANANA_CANNON;
            }
            
            if (playerAdvancements.Contains(AdvancementState.FEED_MONKEY) &&
                !playerAdvancements.Contains(AdvancementState.USE_BANANA_CANNON)) {
                return AdvancementState.FEED_MONKEY;
            }
            
            if (playerAdvancements.Contains(AdvancementState.GRAB_BANANAS) &&
                !playerAdvancements.Contains(AdvancementState.FEED_MONKEY)) {
                return AdvancementState.GRAB_BANANAS;
            }
            
            if (playerAdvancements.Contains(AdvancementState.PUT_PLATFORM) &&
                !playerAdvancements.Contains(AdvancementState.GRAB_BANANAS)) {
                return AdvancementState.PUT_PLATFORM;
            }
            
            if (playerAdvancements.Contains(AdvancementState.ASPIRE_SOMETHING) &&
                !playerAdvancements.Contains(AdvancementState.PUT_PLATFORM)) {
                return AdvancementState.ASPIRE_SOMETHING;
            }

            if (playerAdvancements.Contains(AdvancementState.GET_BANANAGUN) &&
                !playerAdvancements.Contains(AdvancementState.ASPIRE_SOMETHING)) {
                return AdvancementState.GET_BANANAGUN;
            }

            return AdvancementState.NEW_GAME;
        }
        
        public void TryAddBlueprintByAdvancementState(AdvancementState advancementState) {
            foreach (var buildableType in _buildableUnlockedByAdvancementState[advancementState]) {
                ObjectsReference.Instance.uiBlueprints.SetVisible(buildableType);

                var blueprintName = ObjectsReference.Instance.scriptableObjectManager.GetName(ItemCategory.BUILDABLE, ItemType.EMPTY, buildableType);
                var blueprintDescription = ObjectsReference.Instance.scriptableObjectManager.GetDescription(ItemCategory.BUILDABLE, ItemType.EMPTY, buildableType);

                ObjectsReference.Instance.uiQueuedMessages.AddMessage("Give "+blueprintName);
            }
        }
    }
}
