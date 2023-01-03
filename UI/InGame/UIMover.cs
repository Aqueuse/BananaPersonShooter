using System;
using Building;
using UnityEngine;
using UnityEngine.UI;

namespace UI.InGame {
    public class UIMover : MonoBehaviour {
        [SerializeField] private Sprite moverGetImage;
        [SerializeField] private Sprite moverPutImage;

        private Image moverImage;

        private void Start() {
            moverImage = GetComponent<Image>();
        }

        public void SwitchGetPut(MoverContext moverContext) {
            moverImage.sprite = moverContext == MoverContext.GET ? moverGetImage : moverPutImage;
        }
    }
}
