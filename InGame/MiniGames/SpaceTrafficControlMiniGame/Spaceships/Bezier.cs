using UnityEngine;

namespace InGame.MiniGames.SpaceTrafficControlMiniGame.Spaceships {
    public class Bezier {
        private Vector3[] points;
        private int count;

        public Bezier(Vector3[] points, int count = 50) {
            this.points = points;
            this.count = count;
        }

        public Vector3[] Generate() {
            Vector3[] newPoints = new Vector3[count];

            float step = 1f / (count - 1);

            for (int i = 0; i < count; i++) {
                float t = step * i;
                newPoints[i] = CalculateBezierPoint(t, points);
            }

            return newPoints;
        }

        private Vector3 CalculateBezierPoint(float t, Vector3[] points) {
            if (points.Length == 2)
                return Vector3.Lerp(points[0], points[1], t);
            
            Vector3[] newPoints = new Vector3[points.Length - 1];

            for (int i = 0; i < points.Length - 1; i++) {
                newPoints[i] = Vector3.Lerp(points[i], points[i + 1], t);
            }

            return CalculateBezierPoint(t, newPoints);
        }
    }
}