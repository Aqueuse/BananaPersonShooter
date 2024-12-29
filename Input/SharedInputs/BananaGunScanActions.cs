using InGame.Player.BananaGunActions;
using UnityEngine;
using UnityEngine.InputSystem;

namespace SharedInputs {
    public class BananaGunScanActions : InputActions {
        [SerializeField] private InputActionReference scanInputActionReference;

        [SerializeField] private Scan scan;
    }
}