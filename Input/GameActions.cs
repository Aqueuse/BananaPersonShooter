using InGame.Player;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameActions : InputActions {
    public Vector2 move;
    public Vector2 scrollSlotsValue;

    private PlayerController _playerController;

    private bool _scrolledRight;
    private bool _scrolledLeft;

    private float leftRotateIncrementer;
    private float rightRotateIncrementer;
    private float topRotateIncrementer;
    private float downRotateIncrementer;

    public InputActionReference moveActionReference;
    public InputActionReference jumpActionReference;
    public InputActionReference runActionReference;
    public InputActionReference rollActionReference;
    public InputActionReference eatActionReference;

    public InputActionReference grabActionReference;

    public InputActionReference interactActionReference;
    public InputActionReference pauseGameActionReference;

    public InputActionReference shootOrConfirmBuildActionReference;

    public InputActionReference buildActionReference;
    public InputActionReference targetObjectActionReference;
    public InputActionReference harvestActionReference;
    public InputActionReference repairActionReference;

    public InputActionReference rotateGhostActionReference;
    public InputActionReference moveAwayCloserGhostActionReference;

    public InputActionReference switchToInventoryActionReference;
    public InputActionReference switchToBananaSelectorActionReference;
    public InputActionReference switchToMapActionReference;

    private void Start() {
        move = new Vector2();
        scrollSlotsValue = new Vector2();

        _playerController = ObjectsReference.Instance.playerController;
    }

    private void OnEnable() {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        ObjectsReference.Instance.cameraPlayer.SetNormalSensibility();
                    
        ObjectsReference.Instance.playerController.canMove = true;
        ObjectsReference.Instance.bananaMan.GetComponent<Rigidbody>().isKinematic = false;

        ObjectsReference.Instance.uiManager.canvasGroupsByUICanvasType[UICanvasGroupType.CROSSHAIRS].alpha = 1;

        moveActionReference.action.Enable();
        moveActionReference.action.performed += Move;
        moveActionReference.action.canceled += Move;

        jumpActionReference.action.Enable();
        jumpActionReference.action.performed += Jump;

        runActionReference.action.Enable();
        runActionReference.action.performed += Run;
        runActionReference.action.canceled += Run;

        rollActionReference.action.Enable();
        rollActionReference.action.performed += Roll;

        eatActionReference.action.Enable();
        eatActionReference.action.performed += Eat;

        grabActionReference.action.Enable();
        grabActionReference.action.performed += Grab;
        grabActionReference.action.canceled += Grab;

        interactActionReference.action.Enable();
        interactActionReference.action.performed += Interact;

        pauseGameActionReference.action.Enable();
        pauseGameActionReference.action.performed += PauseGame;

        shootOrConfirmBuildActionReference.action.Enable();
        shootOrConfirmBuildActionReference.action.performed += ShootOrConfirmBuild;
        shootOrConfirmBuildActionReference.action.canceled += ShootOrConfirmBuild;

        buildActionReference.action.Enable();
        buildActionReference.action.performed += Build;
        buildActionReference.action.canceled += Build;

        targetObjectActionReference.action.Enable();
        targetObjectActionReference.action.performed += Scan;
        targetObjectActionReference.action.canceled += Scan;

        harvestActionReference.action.Enable();
        harvestActionReference.action.performed += Harvest;

        repairActionReference.action.Enable();
        repairActionReference.action.performed += Repair;

        rotateGhostActionReference.action.Enable();
        rotateGhostActionReference.action.performed += Rotate;

        moveAwayCloserGhostActionReference.action.Enable();
        moveAwayCloserGhostActionReference.action.performed += MoveAwayCloserGhostTarget;

        switchToInventoryActionReference.action.Enable();
        switchToInventoryActionReference.action.performed += SwitchToInventory;
         
        switchToBananaSelectorActionReference.action.Enable();
        switchToBananaSelectorActionReference.action.performed += SwitchToBananaSelector;
            
        switchToMapActionReference.action.Enable();
        switchToMapActionReference.action.performed += SwitchToMap;
    }

    private void OnDisable() {
        jumpActionReference.action.Disable();
        jumpActionReference.action.performed -= Jump;

        moveActionReference.action.Disable();
        moveActionReference.action.performed -= Move;
        moveActionReference.action.canceled -= Move;

        runActionReference.action.Disable();
        runActionReference.action.performed -= Run;

        rollActionReference.action.Disable();
        rollActionReference.action.performed -= Roll;

        eatActionReference.action.Disable();
        eatActionReference.action.performed -= Eat;

        grabActionReference.action.Disable();
        grabActionReference.action.performed -= Grab;
        grabActionReference.action.canceled -= Grab;

        interactActionReference.action.Disable();
        interactActionReference.action.performed -= Interact;

        pauseGameActionReference.action.Disable();
        pauseGameActionReference.action.performed -= PauseGame;
            
        shootOrConfirmBuildActionReference.action.Disable();
        shootOrConfirmBuildActionReference.action.performed -= ShootOrConfirmBuild;
        shootOrConfirmBuildActionReference.action.canceled -= ShootOrConfirmBuild;

        buildActionReference.action.Disable();
        buildActionReference.action.performed -= Build;
        buildActionReference.action.canceled -= Build;

        targetObjectActionReference.action.Disable();
        targetObjectActionReference.action.performed -= Scan;
        targetObjectActionReference.action.canceled -= Scan;

        harvestActionReference.action.Disable();
        harvestActionReference.action.performed -= Harvest;
            
        repairActionReference.action.Disable();
        repairActionReference.action.performed -= Repair;

        rotateGhostActionReference.action.Disable();
        rotateGhostActionReference.action.performed -= Rotate;

        moveAwayCloserGhostActionReference.action.Disable();
        moveAwayCloserGhostActionReference.action.performed -= MoveAwayCloserGhostTarget;

        switchToInventoryActionReference.action.Disable();
        switchToInventoryActionReference.action.performed -= SwitchToInventory;
            
        switchToMapActionReference.action.Disable();
        switchToMapActionReference.action.performed -= SwitchToMap;
            
        switchToBananaSelectorActionReference.action.Disable();
        switchToBananaSelectorActionReference.action.performed -= SwitchToBananaSelector;
    }

    private void Move(InputAction.CallbackContext context) {
        if (!_playerController.canMove) move = Vector2.zero;

        move.x = context.ReadValue<Vector2>().x;
        move.y = context.ReadValue<Vector2>().y;
    }

    private void Jump(InputAction.CallbackContext context) {
        if (!_playerController.canMove) return;

        if (context.performed) {
            _playerController.PlayerJump();
        }
    }

    private void Run(InputAction.CallbackContext context) {
        if (context.performed) {
            _playerController.PlayerSprint();
        }
            
        if (context.canceled) {
            _playerController.PlayerStopSprint();
        }
    }

    private void Roll(InputAction.CallbackContext context) {
        if (_playerController.isGrounded) _playerController.PlayerRoll();
    }

    private static void Eat(InputAction.CallbackContext context) {
        ObjectsReference.Instance.bananaMan.GainHealth();
    }

    private static void Grab(InputAction.CallbackContext context) {
        if (context.performed) ObjectsReference.Instance.grab.DoGrab(); 
        if (context.canceled) ObjectsReference.Instance.grab.Release();
    }
        
    private static void Interact(InputAction.CallbackContext context) {
        ObjectsReference.Instance.interact.Validate();
    }

    private static void PauseGame(InputAction.CallbackContext context) {
        if (ObjectsReference.Instance.bananaMan.isGrabingBananaGun)
            ObjectsReference.Instance.build.CancelBuild();

        else {
            ObjectsReference.Instance.inputManager.SwitchContext(InputContext.UI);

            ObjectsReference.Instance.gameManager.PauseGame();
            ObjectsReference.Instance.uiManager.ShowGameMenu();
        }
    }
        
    private void ShootOrConfirmBuild(InputAction.CallbackContext context) {
        if (!ObjectsReference.Instance.bananaMan.tutorialFinished) return;
            
        if (context.performed) {
            if (ObjectsReference.Instance.playerActionsSwitch.playerActions == PlayerActionsType.BUILD) {
                ObjectsReference.Instance.build.ValidateBuildable();                    
                ObjectsReference.Instance.build.CancelGhost();
                ObjectsReference.Instance.bananaGun.UngrabBananaGun();
                    
                ObjectsReference.Instance.playerActionsSwitch.SwitchToPlayerAction(PlayerActionsType.IDLE);                    
            }
                
            else {
                ObjectsReference.Instance.playerActionsSwitch.SwitchToPlayerAction(PlayerActionsType.THROW_BANANA);
                ObjectsReference.Instance.uiTools.ZoomShootIcon();
                ObjectsReference.Instance.bananaGun.GrabBananaGun();

                if (ObjectsReference.Instance.bananasInventory.GetQuantity(ObjectsReference.Instance.bananaMan.activeItem.bananaType) <= 0) return;
            
                ObjectsReference.Instance.throwBanana.LoadingGun();
                ObjectsReference.Instance.uiCrosshairs.SetCrosshair(ObjectsReference.Instance.bananaMan.activeItem.bananaType);
            }
        }
            
        if (context.canceled) {
            ObjectsReference.Instance.uiTools.UnzoomIcons();
            ObjectsReference.Instance.bananaGun.UngrabBananaGun();

            ObjectsReference.Instance.throwBanana.CancelThrow();

            ObjectsReference.Instance.playerActionsSwitch.SwitchToPlayerAction(PlayerActionsType.IDLE);
        }
    }
        
    private void Build(InputAction.CallbackContext context) {
        if (!ObjectsReference.Instance.bananaMan.tutorialFinished) return;

        if (context.performed) {
            ObjectsReference.Instance.playerActionsSwitch.SwitchToPlayerAction(PlayerActionsType.BUILD);

            ObjectsReference.Instance.uiTools.ZoomPlaceIcon();
            ObjectsReference.Instance.uInventoriesManager.GetCurrentUIHelper().ShowBuildHelper();

            ObjectsReference.Instance.bananaGun.GrabBananaGun();

            ObjectsReference.Instance.build.ActivatePlateformGhost();
        }

        if (context.canceled) {
            ObjectsReference.Instance.uInventoriesManager.GetCurrentUIHelper().ShowDefaultHelper();
            ObjectsReference.Instance.uiTools.UnzoomIcons();

            ObjectsReference.Instance.bananaGun.UngrabBananaGun();
                
            ObjectsReference.Instance.build.CancelGhost();
                
            ObjectsReference.Instance.playerActionsSwitch.SwitchToPlayerAction(PlayerActionsType.IDLE);
        }
    }
        
    private void Scan(InputAction.CallbackContext context) {
        if (!ObjectsReference.Instance.bananaMan.tutorialFinished) return;

        if (context.performed) {
            ObjectsReference.Instance.bananaGun.GrabBananaGun();
            ObjectsReference.Instance.uiTools.ZoomTakeIcon();

            ObjectsReference.Instance.build.CancelGhost();
                
            ObjectsReference.Instance.playerActionsSwitch.SwitchToPlayerAction(PlayerActionsType.SCAN);
        }

        if (context.canceled) {
            ObjectsReference.Instance.uiTools.UnzoomIcons();
            ObjectsReference.Instance.descriptionsManager.HideAllPanels();

            ObjectsReference.Instance.bananaGun.UngrabBananaGun();

            ObjectsReference.Instance.playerActionsSwitch.SwitchToPlayerAction(PlayerActionsType.IDLE);
        }
    }

    private void Harvest(InputAction.CallbackContext context) {
        if (!ObjectsReference.Instance.bananaMan.tutorialFinished) return;
        if (ObjectsReference.Instance.playerActionsSwitch.playerActions != PlayerActionsType.SCAN) return;

        ObjectsReference.Instance.scan.harvest();
    }

    private void Repair(InputAction.CallbackContext context) {
        if (!ObjectsReference.Instance.bananaMan.tutorialFinished) return;
        if (ObjectsReference.Instance.playerActionsSwitch.playerActions != PlayerActionsType.SCAN) return;

        ObjectsReference.Instance.scan.RepairBuildable();
    }
        
    private void Rotate(InputAction.CallbackContext context) {
        if (ObjectsReference.Instance.playerActionsSwitch.playerActions != PlayerActionsType.BUILD) return;

        var contextValue = context.ReadValue<Vector2>(); 
            
        if (contextValue.x < 0) {
            ObjectsReference.Instance.build.RotateGhost(Vector3.left);
        }

        if (contextValue.x > 0) {
            ObjectsReference.Instance.build.RotateGhost(Vector3.right);
        }
            
        if (contextValue.y < 0) {
            ObjectsReference.Instance.build.RotateGhost(Vector3.back);
        }

        if (contextValue.y > 0) {
            ObjectsReference.Instance.build.RotateGhost(Vector3.forward);
        }
    }

    private void MoveAwayCloserGhostTarget(InputAction.CallbackContext context) {
        if (ObjectsReference.Instance.playerActionsSwitch.playerActions == PlayerActionsType.BUILD) return;

        var placementLocalPosition = ObjectsReference.Instance.uiHud.buildablePlacementTransform.localPosition;

        if (context.ReadValue<Vector2>().y > 1) {
            if (placementLocalPosition.z < 25) {
                placementLocalPosition.z += 1f;
                ObjectsReference.Instance.uiHud.buildablePlacementTransform.localPosition = placementLocalPosition;
            }
        }

        if (context.ReadValue<Vector2>().y < 1) {
            if (placementLocalPosition.z > 4) {
                placementLocalPosition.z -= 1f;
                ObjectsReference.Instance.uiHud.buildablePlacementTransform.localPosition = placementLocalPosition;
            }
        }
    }
        
    private void ZoomDezoomCamera(InputAction.CallbackContext context) {
        bool isPressedleftAlt = Keyboard.current.leftAltKey.isPressed;
        bool isLeftStickPressed = Gamepad.current.leftStickButton.isPressed; 
            
        if (!isPressedleftAlt || !isLeftStickPressed) return;

        scrollSlotsValue = context.ReadValue<Vector2>();
        var scrollValue = scrollSlotsValue.y;
            
        if (scrollValue > 0) {
            ObjectsReference.Instance.cameraPlayer.ZoomCamera();
        }

        if (scrollValue < 0) {
            ObjectsReference.Instance.cameraPlayer.DezoomCamera();
        }
    }

    private void SwitchToInventory(InputAction.CallbackContext context) {
        ObjectsReference.Instance.inputManager.SwitchContext(InputContext.INVENTORY);
        ObjectsReference.Instance.gameManager.gameContext = GameContext.IN_INVENTORY;
        ObjectsReference.Instance.uiManager.SetActive(UICanvasGroupType.INVENTORIES, true);
        ObjectsReference.Instance.uInventoriesManager.FocusInventory();
    }

    private void SwitchToMap(InputAction.CallbackContext context) {
        ObjectsReference.Instance.inputManager.SwitchContext(InputContext.MAP);
        ObjectsReference.Instance.gameManager.gameContext = GameContext.IN_MAP;
            
        ObjectsReference.Instance.uiManager.SetActive(UICanvasGroupType.MAP, true);
    }

    private void SwitchToBananaSelector(InputAction.CallbackContext callbackContext) {
        Time.timeScale = 0.1f;
        ObjectsReference.Instance.inputManager.SwitchContext(InputContext.BANANA_SELECTOR);
        ObjectsReference.Instance.gameManager.gameContext = GameContext.IN_BANANA_SELECTOR;

        ObjectsReference.Instance.uiManager.SetActive(UICanvasGroupType.BANANA_SELECTOR, true);
    }
}