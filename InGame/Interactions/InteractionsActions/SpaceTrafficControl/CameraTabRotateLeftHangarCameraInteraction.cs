using InGame.SpaceTrafficControl;
using UnityEngine;

namespace InGame.Interactions.InteractionsActions.SpaceTrafficControl {
    public class CameraTabRotateLeftHangarCameraInteraction : Interaction {
        public override void Activate(GameObject interactedGameObject) {
            Hangars.Instance.RotateCameraLeft();
        }
    }
}