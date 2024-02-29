using InGame.SpaceTrafficControl;
using UnityEngine;

namespace InGame.Interactions.InteractionsActions.SpaceTrafficControl {
    public class CameraTabRotateRightHangarCameraInteraction : Interaction {
        public override void Activate(GameObject interactedGameObject) {
           Hangars.Instance.RotateCameraRight();
        }
    }
}