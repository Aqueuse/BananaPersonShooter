using UnityEngine;

public class InputManager : MonoBehaviour {
    [SerializeField] private GenericDictionary<InputContext, InputActions[]> inputActionsByInputContext;
    public BananaGunMode bananaGunActions;

    public void SwitchContext(InputContext newInputContext) {
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
}