using UI.InGame;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoSingleton<InputManager> {
    public void SwitchToLeftSlot(InputAction.CallbackContext context) {
        if (context.performed && GameManager.Instance.isGamePlaying) {
            UISlotsManager.Instance.Select_Left_Slot();
        }
    }
    
    public void SwitchToRightSlot(InputAction.CallbackContext context) {
        if (context.performed && GameManager.Instance.isGamePlaying) {
            UISlotsManager.Instance.Select_Right_Slot();
        }
    }
    
    public void Scroll_Slots(InputAction.CallbackContext context) {
        if (context.performed && GameManager.Instance.isGamePlaying) {
            float scrollValue = context.ReadValue<Vector2>().y;
            if (scrollValue < 0) UISlotsManager.Instance.Select_Left_Slot();
            if (scrollValue > 0) UISlotsManager.Instance.Select_Right_Slot();
        }
    }
    
    public void Select_Slot_0(InputAction.CallbackContext context) {
        if (context.performed && GameManager.Instance.isGamePlaying) {
            UISlotsManager.Instance.Switch_To_Slot(0);
        }
    }

    public void Select_Slot_1(InputAction.CallbackContext context) {
        if (context.performed && GameManager.Instance.isGamePlaying) {
            UISlotsManager.Instance.Switch_To_Slot(1);
        }
    }

    public void Select_Slot_2(InputAction.CallbackContext context) {
        if (context.performed && GameManager.Instance.isGamePlaying) {
            UISlotsManager.Instance.Switch_To_Slot(2);
        } 
    }

    public void Select_Slot_3(InputAction.CallbackContext context) {
        if (context.performed && GameManager.Instance.isGamePlaying) {
            UISlotsManager.Instance.Switch_To_Slot(3);
        }
    }

    public void Select_Slot_4(InputAction.CallbackContext context) {
        if (context.performed && GameManager.Instance.isGamePlaying) {
            UISlotsManager.Instance.Switch_To_Slot(4);
        }
    }
}
