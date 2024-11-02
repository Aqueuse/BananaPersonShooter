using UnityEngine;

public class InputManager : MonoBehaviour {
    public InputContext inputContext;
    
    [SerializeField] private GenericDictionary<InputContext, InputActions[]> inputActionsByInputContext;
    public void SwitchContext(InputContext newInputContext) {
        inputContext = newInputContext;
        
        DisableAllInputs();

        foreach (var inputAction in inputActionsByInputContext[newInputContext]) {
            inputAction.enabled = true;
        }
    }
    
    // for some actions, UI need to also be activated (for navigation)
    public void AlsoActivateUIinputActions() {
        foreach (var inputAction in inputActionsByInputContext[InputContext.UI]) {
            inputAction.enabled = true;
        }
    }
    
    private void DisableAllInputs() {
        foreach (var inputActionArray in inputActionsByInputContext) {
            foreach (var inputAction in inputActionArray.Value) {
                inputAction.enabled = false;
            }
        }
    }
    
    public void SwitchBackToGame() {
        ObjectsReference.Instance.gameManager.gameContext = GameContext.IN_GAME;
        SwitchContext(InputContext.GAME);
                
        ObjectsReference.Instance.bananaGunActionsSwitch.gameObject.SetActive(true);
    }
}