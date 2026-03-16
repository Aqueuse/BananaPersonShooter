using UnityEngine;

namespace InGame.MiniGames.SpaceTrafficControl.projectiles {
    public class Laser : MonoBehaviour {
        [SerializeField] private MeshRenderer projectileRenderer;

        public BananaEffect bananaEffect;
        public Color goopColor;
        
        public void SetColor(Color color) {
            projectileRenderer.material.color = color;
            goopColor = color;
        }
    }
}