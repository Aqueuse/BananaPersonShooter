using Enums;
using UnityEngine;

namespace Monkeys.Chimployee {
    public class ChimployeeDialogueAction : MonoBehaviour {
        [SerializeField] private ChimployeeDialogue _chimployeeDialogue;

        private void Start() {
            switch (_chimployeeDialogue) {
                case ChimployeeDialogue.chimployee_first_interaction:
                    Chimployee.AuthorizeDoorsAccess();
                    ObjectsReference.Instance.uIadvancements.SetSpeakToMiniChimpAdvancementBanana();
                    break;
                case ChimployeeDialogue.chimployee_please_feed_monkey:
                    ObjectsReference.Instance.uIadvancements.SetAdvancementBanana(AdvancementState.USE_BANANA_CANNON);
                    break;
                case ChimployeeDialogue.chimployee_please_clean_map:
                    ObjectsReference.Instance.gameData.bananaManSavedData.playerAdvancements.Add(AdvancementState.CLEANED_MAP);
                    ObjectsReference.Instance.uIadvancements.SetAdvancementBanana(AdvancementState.CLEANED_MAP);
                    break;
            }
        }
    }
}
