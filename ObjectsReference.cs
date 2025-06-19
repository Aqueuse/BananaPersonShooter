using Audio;
using Cameras;
using InGame;
using InGame.CommandRoomPanelControls;
using InGame.Dialogues;
using InGame.Gestion;
using InGame.Interactions;
using InGame.Inventories;
using InGame.Items.ItemsBehaviours;
using InGame.Items.ItemsData;
using InGame.MainBlock;
using InGame.MiniGames.MarketingCampaignMiniGame;
using InGame.MiniGames.SpaceTrafficControlMiniGame.Spaceships;
using InGame.Player;
using InGame.Player.BananaGunActions;
using InGame.SpaceTrafficControl;
using Save;
using Settings;
using SharedInputs;
using UI;
using UI.InGame;
using UI.InGame.BuildingMaterialMissingPanel;
using UI.InGame.CommandRoomControlPanels;
using UI.InGame.MainPanel;
using UI.InGame.MainPanel.Inventories;
using UI.InGame.Merchimps;
using UI.InGame.VisitorReceptionMiniGameUI;
using UI.Menus;
using UI.Save;
using UnityEngine;

public class ObjectsReference : MonoSingleton<ObjectsReference> {
    public MeshReferenceScriptableObject meshReferenceScriptableObject;
    public GameSettings gameSettings;

    [Header("Managers")]
    public GameManager gameManager;
    public InputManager inputManager;
    public AudioManager audioManager;

    [Header("Mini Games")]
    public CommandRoomControlPanelsManager commandRoomControlPanelsManager;
    public AdMarketingCampaignManager adMarketingCampaignManager;
    public SpaceTrafficControlManager spaceTrafficControlManager;
    public SpaceshipsSpawner spaceshipsSpawner;
    public CannonsManager cannonsManager;

    [Header("Banana Man")]
    public BananaMan bananaMan;
    public PlayerController playerController;
    public BananaGun bananaGun;
    public BottomSlots bottomSlots;
    public BuildAction buildAction;
    public ShootAction shootAction;
    public AspireAction aspireAction;
    public ScanAction scanAction;
    public Grab grab;
    public Trade trade;
    public Interact interact;
    [Space]
    public GameActions keyboardGameActions;
    public UiActions keyboardUiActions;
    public BananaGunActionsSwitch bananaGunActionsSwitch;
    [Space]
    public GhostsReference ghostsReference;

    [Header("banana man inventories")]
    public BananasInventory BananaManBananasInventory;
    public RawMaterialInventory bananaManRawMaterialInventory;
    public IngredientsInventory bananaManIngredientsInventory;
    public ManufacturedItemsInventory bananaManManufacturedItemsInventory;
    public FoodInventory bananaManFoodInventory;
    public BuildablesInventory bananaManBuildablesInventory;

    [Header("Gestion Mode")]
    public ScanWithMouseForDescription scanWithMouseForDescription;
    public MiniChimpDialoguesManager miniChimpDialoguesManager;
    public MiniMap miniMap;
    [Space]
    public GestionViewMode gestionViewMode;
    
    [Header("Cameras")]
    public MainCamera mainCamera;
    public CameraPlayer cameraPlayer;
    public CameraGestionDragRotate gestionDragCamera;
    public CameraGestionRelativeMove gestionRelativeMoveCamera;
    public FootStepSurfaceDetector footStepSurfaceDetector;
    [Space]
    public Cinematiques cinematiques;
    public Death death;
    
    [Header("Saving System")]
    public WorldData worldData;
    public GameSave gameSave;
    public GameReset gameReset;
    
    [Header("UI")]
    public UIManager uiManager;
    [Space]
    public UInventoriesManager uInventoriesManager;
    public UiMainPanel uiMainPanel;
    public UIMissingMaterialBuildingPanel uiMissingMaterialBuildingPanel;
    [Space] 
    public UIBananasInventory bananaManUiBananasInventory;
    public UIRawMaterialsInventory bananaManUIRawMaterialsInventory;
    public UIIngredientsInventory bananaManUiIngredientsInventory;
    public UIManufacturedItemsInventory bananaManUiManufacturedItemsInventory;
    public UIFoodInventory bananaManUiFoodInventory;
    public UIBlueprintsInventory bananaManUiBlueprintsInventory;
    public UIToolTipOnMouseHover uiToolTipOnMouseHover;
    [Space]
    public UIFace uiFace;
    public UIQueuedMessages uiQueuedMessages;
    public UIHomeMenu uiHomeMenu;
    public UiGameMenu uiGameMenu;
    public UIOptionsMenu uiOptionsMenu;
    public UICredits uiCredits;
    public UISave uiSave;
    public UISettings uiSettings;
    [Space]
    public UICommunicationPanel uiCommunicationPanel;
    public UIgestionPanel uIgestionPanel;
    public UICannons uiCannons;
    public UIMonkeyMensReception uiTouristReception;
    public UIMerchant uiMerchant;
}