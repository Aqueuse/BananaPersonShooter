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
    
    private void DisableAllInputs() {
        foreach (var inputActionArray in inputActionsByInputContext) {
            foreach (var inputAction in inputActionArray.Value) {
                inputAction.enabled = false;
            }
        }
    }
    
    public void SwitchBackToGame() {
        SwitchContext(InputContext.GAME);
        
        ObjectsReference.Instance.gameManager.gameContext = GameContext.BANANAMAN_CONTROL;
        ObjectsReference.Instance.bananaMan.SetToPlayable();
    }
}