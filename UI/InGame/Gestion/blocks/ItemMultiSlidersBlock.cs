using TMPro;
using UnityEngine;

namespace UI.InGame.Gestion.blocks {
    public class ItemMultiSlidersBlock : MonoBehaviour {
        [SerializeField] private TextMeshProUGUI slidersName;
        [SerializeField] private OneItemSliderBlock[] _oneItemSliderBlocks;

        public void SetBlock((int, string)[] slidersValues, int maxValue) {
            if (slidersValues.Length > _oneItemSliderBlocks.Length) {
                Debug.Log("not enough sliders on block");
                return;
            }
            
            foreach (var slider in _oneItemSliderBlocks) {
                slider.gameObject.SetActive(false);
            }
            
            for (var i=0; i<slidersValues.Length; i++) {
                _oneItemSliderBlocks[i].gameObject.SetActive(true);
                _oneItemSliderBlocks[i].SetBlock(
                    slidersValues[i].Item2,
                    slidersValues[i].Item1,
                    maxValue
                    );
            }
        }
    }
}
