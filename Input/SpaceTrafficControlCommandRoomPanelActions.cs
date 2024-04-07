using UnityEngine;
using UnityEngine.InputSystem;

public class SpaceTrafficControlCommandRoomPanelActions : InputActions {
    [SerializeField] private InputActionReference switchToLeftCannonActionReference;
    [SerializeField] private InputActionReference switchToRightCannonActionReference;
        
    [SerializeField] private InputActionReference switchToCannon1ActionReference;
    [SerializeField] private InputActionReference switchToCannon2ActionReference;
    [SerializeField] private InputActionReference switchToCannon3ActionReference;
    [SerializeField] private InputActionReference switchToCannon4ActionReference;
    [SerializeField] private InputActionReference switchToCannon5ActionReference;
    [SerializeField] private InputActionReference switchToCannon6ActionReference;
    [SerializeField] private InputActionReference switchToCannon7ActionReference;
    [SerializeField] private InputActionReference switchToCannon8ActionReference;
        
    [SerializeField] private InputActionReference shootActionReference;
    [SerializeField] private InputActionReference rotateCannonActionReference;
    [SerializeField] private InputActionReference quitActionReference;
        
    private void OnEnable() {
        switchToLeftCannonActionReference.action.Enable();
        switchToLeftCannonActionReference.action.performed += SwitchToCannonLeft;

        switchToRightCannonActionReference.action.Enable();
        switchToRightCannonActionReference.action.performed += SwitchToCannonRight;

        switchToCannon1ActionReference.action.Enable();
        switchToCannon1ActionReference.action.performed += SwitchToCannon1;
        switchToCannon2ActionReference.action.Enable();
        switchToCannon2ActionReference.action.performed += SwitchToCannon2;
        switchToCannon3ActionReference.action.Enable();
        switchToCannon3ActionReference.action.performed += SwitchToCannon3;
        switchToCannon4ActionReference.action.Enable();
        switchToCannon4ActionReference.action.performed += SwitchToCannon4;
        switchToCannon5ActionReference.action.Enable();
        switchToCannon5ActionReference.action.performed += SwitchToCannon5;
        switchToCannon6ActionReference.action.Enable();
        switchToCannon6ActionReference.action.performed += SwitchToCannon6;
        switchToCannon7ActionReference.action.Enable();
        switchToCannon7ActionReference.action.performed += SwitchToCannon7;
        switchToCannon8ActionReference.action.Enable();
        switchToCannon8ActionReference.action.performed += SwitchToCannon8;
            
        shootActionReference.action.Enable();
        shootActionReference.action.performed += Shoot;

        rotateCannonActionReference.action.Enable();
        rotateCannonActionReference.action.performed += RotateCannon;
            
        quitActionReference.action.Enable();
        quitActionReference.action.performed += Quit;
    }

    private void OnDisable() {
        switchToLeftCannonActionReference.action.Disable();
        switchToLeftCannonActionReference.action.performed -= SwitchToCannonLeft;

        switchToRightCannonActionReference.action.Disable();
        switchToRightCannonActionReference.action.performed -= SwitchToCannonRight;

        switchToCannon1ActionReference.action.Disable();
        switchToCannon1ActionReference.action.performed -= SwitchToCannon1;
        switchToCannon2ActionReference.action.Disable();
        switchToCannon2ActionReference.action.performed -= SwitchToCannon2;
        switchToCannon3ActionReference.action.Disable();
        switchToCannon3ActionReference.action.performed -= SwitchToCannon3;
        switchToCannon4ActionReference.action.Disable();
        switchToCannon4ActionReference.action.performed -= SwitchToCannon4;
        switchToCannon5ActionReference.action.Disable();
        switchToCannon5ActionReference.action.performed -= SwitchToCannon5;
        switchToCannon6ActionReference.action.Disable();
        switchToCannon6ActionReference.action.performed -= SwitchToCannon6;
        switchToCannon7ActionReference.action.Disable();
        switchToCannon7ActionReference.action.performed -= SwitchToCannon7;
        switchToCannon8ActionReference.action.Disable();
        switchToCannon8ActionReference.action.performed -= SwitchToCannon8;

        shootActionReference.action.Disable();
        shootActionReference.action.performed -= Shoot;

        rotateCannonActionReference.action.Enable();
        rotateCannonActionReference.action.performed -= RotateCannon;

        quitActionReference.action.Disable();
        quitActionReference.action.performed -= Quit;
    }

    private void SwitchToCannonLeft(InputAction.CallbackContext context) {
        ObjectsReference.Instance.cannonsManagement.SwitchToLeftCannon();   
    }

    private void SwitchToCannonRight(InputAction.CallbackContext context) {
        ObjectsReference.Instance.cannonsManagement.SwitchToRightCannon();
    }
        
    private void SwitchToCannon1(InputAction.CallbackContext context) { ObjectsReference.Instance.cannonsManagement.SwitchToCannon(RegionType.MAP01); }
    private void SwitchToCannon2(InputAction.CallbackContext context) { ObjectsReference.Instance.cannonsManagement.SwitchToCannon(RegionType.MAP02); }
    private void SwitchToCannon3(InputAction.CallbackContext context) { ObjectsReference.Instance.cannonsManagement.SwitchToCannon(RegionType.MAP03); }
    private void SwitchToCannon4(InputAction.CallbackContext context) { ObjectsReference.Instance.cannonsManagement.SwitchToCannon(RegionType.MAP04); }
    private void SwitchToCannon5(InputAction.CallbackContext context) { ObjectsReference.Instance.cannonsManagement.SwitchToCannon(RegionType.MAP05); }
    private void SwitchToCannon6(InputAction.CallbackContext context) { ObjectsReference.Instance.cannonsManagement.SwitchToCannon(RegionType.MAP06); }
    private void SwitchToCannon7(InputAction.CallbackContext context) { ObjectsReference.Instance.cannonsManagement.SwitchToCannon(RegionType.MAP07); }
    private void SwitchToCannon8(InputAction.CallbackContext context) { ObjectsReference.Instance.cannonsManagement.SwitchToCannon(RegionType.MAP08); }

    private void RotateCannon(InputAction.CallbackContext context) {
        ObjectsReference.Instance.cannonsManagement.RotateCannon(context.ReadValue<Vector2>().x, context.ReadValue<Vector2>().y);
    }

    private void Shoot(InputAction.CallbackContext context) {
        ObjectsReference.Instance.cannonsManagement.ShootOnActivatedRegion();
    }
        
    private void Quit(InputAction.CallbackContext callbackContext) {
        ObjectsReference.Instance.cannonsManagement.StopCannonControl();
    }
}