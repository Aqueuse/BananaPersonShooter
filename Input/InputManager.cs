using System;
using System.Collections.Generic;
using Enums;
using Input.interactables;
using Input.UIActions;
using TMPro;
using UI.InGame.Inventory;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Input {
    public class InputManager : MonoBehaviour {
        [SerializeField] private EventSystem eventSystem;
        public UISchemaSwitcher uiSchemaSwitcher;
        public UISchemaSwitchType uiSchemaContext;

        [SerializeField] private TextMeshProUGUI upScrollHelperGamepad;
        [SerializeField] private TextMeshProUGUI upScrollHelperKeyboard;
        [SerializeField] private TextMeshProUGUI downScrollHelperGamepad;
        [SerializeField] private TextMeshProUGUI downScrollHelperKeyboard;
        
        private GameActions _gameActions;

        public BananasDryerAction bananasDryerAction;
        
        [SerializeField] private GenericDictionary<string, bool> previousControllers = new();
        private string[] currentControllers;
        private Dictionary<string, bool> tempCurrentControllers;
        private string controllerId;
        private bool wasConnected;
        private bool isConnected;
        
        public SchemaContext schemaContext = SchemaContext.KEYBOARD;

        private void Start() {
            _gameActions = GetComponent<GameActions>();
            uiSchemaSwitcher = GetComponent<UISchemaSwitcher>();

            currentControllers = UnityEngine.Input.GetJoystickNames();

            if (currentControllers.Length > 0) {
                ObjectsReference.Instance.uiManager.canvasGroupsByUICanvasType[UICanvasGroupType.BUILD_HELPER_GAMEPAD].alpha = 1;
                ObjectsReference.Instance.uiManager.canvasGroupsByUICanvasType[UICanvasGroupType.BUILD_HELPER_KEYBOARD].alpha = 0;
                upScrollHelperGamepad.alpha = 1;
                downScrollHelperGamepad.alpha = 1;
                upScrollHelperKeyboard.alpha = 0;
                downScrollHelperKeyboard.alpha = 0;
                schemaContext = SchemaContext.GAMEPAD;
            }
            else {
                ObjectsReference.Instance.uiManager.canvasGroupsByUICanvasType[UICanvasGroupType.BUILD_HELPER_GAMEPAD].alpha = 0;
                ObjectsReference.Instance.uiManager.canvasGroupsByUICanvasType[UICanvasGroupType.BUILD_HELPER_KEYBOARD].alpha = 1;
                upScrollHelperGamepad.alpha = 0;
                downScrollHelperGamepad.alpha = 0;
                upScrollHelperKeyboard.alpha = 1;
                downScrollHelperKeyboard.alpha = 1;
                schemaContext = SchemaContext.KEYBOARD;
            }

            InvokeRepeating(nameof(CheckControllerConnection), 0, 1f);
        }
        
        private void CheckControllerConnection() {
            currentControllers = UnityEngine.Input.GetJoystickNames();

            tempCurrentControllers = new Dictionary<string, bool>(previousControllers);
            
            foreach (var controller in tempCurrentControllers) {
                controllerId = controller.Key;
                wasConnected = controller.Value;
                
                isConnected = Array.IndexOf(currentControllers, controllerId) != -1;
                
                if (wasConnected && !isConnected) {
                    if (ObjectsReference.Instance.gameManager.gameContext == GameContext.IN_GAME) {
                        ObjectsReference.Instance.inputManager.uiSchemaContext = UISchemaSwitchType.GAME_MENU;
                        ObjectsReference.Instance.inputManager.SwitchContext(InputContext.UI);
                        
                        ObjectsReference.Instance.gameManager.PauseGame();
                        ObjectsReference.Instance.uiManager.Show_game_menu();
                        ObjectsReference.Instance.uiManager.canvasGroupsByUICanvasType[UICanvasGroupType.GAMEPAD_DISCONNECTED].alpha = 1;
                        
                        ObjectsReference.Instance.uiManager.canvasGroupsByUICanvasType[UICanvasGroupType.BUILD_HELPER_GAMEPAD].alpha = 0;
                        ObjectsReference.Instance.uiManager.canvasGroupsByUICanvasType[UICanvasGroupType.BUILD_HELPER_KEYBOARD].alpha = 1;
                        upScrollHelperGamepad.alpha = 0;
                        downScrollHelperGamepad.alpha = 0;
                        upScrollHelperKeyboard.alpha = 1;
                        downScrollHelperKeyboard.alpha = 1;

                        schemaContext = SchemaContext.KEYBOARD;
                    }
                }
                else if (!wasConnected && isConnected) {
                    if (ObjectsReference.Instance.gameManager.gameContext == GameContext.IN_GAME) {
                        ObjectsReference.Instance.uiManager.canvasGroupsByUICanvasType[UICanvasGroupType.GAMEPAD_DISCONNECTED].alpha = 0;
                        
                        ObjectsReference.Instance.uiManager.canvasGroupsByUICanvasType[UICanvasGroupType.BUILD_HELPER_GAMEPAD].alpha = 1;
                        ObjectsReference.Instance.uiManager.canvasGroupsByUICanvasType[UICanvasGroupType.BUILD_HELPER_KEYBOARD].alpha = 0;
                        upScrollHelperGamepad.alpha = 1;
                        downScrollHelperGamepad.alpha = 1;
                        upScrollHelperKeyboard.alpha = 0;
                        downScrollHelperKeyboard.alpha = 0;

                        ObjectsReference.Instance.inputManager.SwitchContext(InputContext.GAME);                        
                        schemaContext = SchemaContext.GAMEPAD;
                    }
                }

                previousControllers[controllerId] = isConnected;
            }
 
            // Parcours des nouveaux contrôleurs
            foreach (string currentControllerId in currentControllers) {
                // Ajouter le contrôleur à la liste des contrôleurs précédents
                if (currentControllerId.Length > 0) previousControllers.TryAdd(currentControllerId, true);
            }
        }

        public void SwitchContext(InputContext newInputContext) {
            if (newInputContext == InputContext.UI) {
                _gameActions.enabled = false;
                
                uiSchemaSwitcher.SwitchUISchema(uiSchemaContext);

                eventSystem.enabled = true;
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;

                ObjectsReference.Instance.playerController.canMove = false;
            }
            else {
                _gameActions.enabled = true;
                uiSchemaSwitcher.DisableAllUISchemas();
                
                eventSystem.enabled = false;
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Confined;
                
                ObjectsReference.Instance.playerController.canMove = true;
            }
        }
    }
}
