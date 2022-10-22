using UI;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoSingleton<InputManager> {
    public BananaType ShortCut1 = BananaType.EMPTY_HAND;
    public BananaType ShortCut2 = BananaType.EMPTY_HAND;
    public BananaType ShortCut3 = BananaType.EMPTY_HAND;
    
    public void Assigne_Weapon_To_Shortcut_1(InputAction.CallbackContext context) {
        if (context.performed && GameManager.Instance.isPlaying) {
            if (UIManager.Instance.inventory.GetComponent<CanvasGroup>().alpha > 0.5f) {
                ShortCut1 = UIManager.Instance.selectedWeapon;
            }
        }
    }
}
