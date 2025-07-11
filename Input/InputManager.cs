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
        ObjectsReference.Instance.gameManager.gameContext = GameContext.IN_GAME;
        SwitchContext(InputContext.GAME);
        
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        ObjectsReference.Instance.uiManager.canvasGroupsByUICanvasType[UICanvasGroupType.CROSSHAIRS].alpha = 1;
        
        ObjectsReference.Instance.cameraPlayer.SetNormalSensibility();

        ObjectsReference.Instance.bananaGunActionsSwitch.gameObject.SetActive(true);
    }
}