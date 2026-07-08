using UnityEngine;

namespace UI.InGame {
    public class UIMap : MonoBehaviour {
        [SerializeField] private Material MiniMapSpritesMaterial;
        private static readonly int sizeMultiplier = Shader.PropertyToID("_SizeMultiplier");

        public void ShowBigMap() {
            ObjectsReference.Instance.uiManager.canvasGroupsByUICanvasType[UICanvasGroupType.BIG_MAP].alpha = 0.99f;
            ObjectsReference.Instance.uiManager.canvasGroupsByUICanvasType[UICanvasGroupType.MINI_MAP].alpha = 0f;

            MiniMapSpritesMaterial.SetFloat(sizeMultiplier, 1);
        }

        public void HideBigMap() {
            ObjectsReference.Instance.uiManager.canvasGroupsByUICanvasType[UICanvasGroupType.BIG_MAP].alpha = 0;
            ObjectsReference.Instance.uiManager.canvasGroupsByUICanvasType[UICanvasGroupType.MINI_MAP].alpha = 1f;
            
            MiniMapSpritesMaterial.SetFloat(sizeMultiplier, 10);
        }
    }
}