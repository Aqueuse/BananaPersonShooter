using UnityEngine;

namespace InGame.MiniGames.SpaceTrafficControlMiniGame.projectiles {
    public class Laser : MonoBehaviour {
        [SerializeField] private MeshRenderer projectileRenderer;
        public Transform attractionPoint;

        public BananaEffect[] bananaEffects;
        public Color goopColor;
        
        public void SetColor(Color color) {
            projectileRenderer.material.color = color;
            goopColor = color;
        }
    }
}