using UnityEngine;

public class InputManager : MonoBehaviour {
    [SerializeField] private GenericDictionary<InputContext, InputActions> inputActionsByInputContext;

    public void SwitchContext(InputContext newInputContext) {
        DisableAllInputs();

        inputActionsByInputContext[newInputContext].enabled = true;
    }

    private void DisableAllInputs() {
        foreach (var inputAction in inputActionsByInputContext) {
            inputAction.Value.enabled = false;
        }
    }
}