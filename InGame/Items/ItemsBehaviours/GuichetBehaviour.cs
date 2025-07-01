using System.Collections.Generic;
using InGame.Monkeys.Chimpvisitors;
using UnityEngine;

namespace InGame.Items.ItemsBehaviours {
    public class GuichetBehaviour : MonoBehaviour {
        public List<VisitorBehaviour> visitorsToWatch;
        public Transform mapPointOfInterestCenter;

        public bool isOpen;
        
        private void Update() {
            if (visitorsToWatch.Count == 0) return;
            
            // whatch visitor list
            foreach (var visitorBehaviour in visitorsToWatch) {
                if (Vector3.Distance(transform.position, visitorBehaviour.transform.position) < 1) {
                    visitorBehaviour.monkeyMenData.bitKongQuantity -= 1;
                    ShowBitkongGain();
                    visitorsToWatch.Remove(visitorBehaviour);
                }
            }
        }

        public Stack<Vector3> GiveToRandomPointsOfInterest() {
            mapPointOfInterestCenter.rotation = Quaternion.Euler(0, 0, 0);
            
            List<Vector3> points = new List<Vector3>();
            
            for (int i = 0; i < 2; i++) {
                mapPointOfInterestCenter.Rotate(Vector3.left, Random.Range(0, 270));
                points.Add(mapPointOfInterestCenter.position + Vector3.forward * 15);
            }

            return new Stack<Vector3>(points);
        }

        private void ShowBitkongGain() {
            // show a UI with +1 bitkong
        }
    }
}