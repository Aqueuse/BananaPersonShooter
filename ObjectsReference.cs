using Audio;
using Cameras;
using InGame;
using InGame.CommandRoomPanelControls;
using InGame.Dialogues;
using InGame.Gestion;
using InGame.Interactions;
using InGame.Inventory;
using InGame.Items.ItemsBehaviours;
using InGame.Items.ItemsData;
using InGame.MiniChimpBlock;
using InGame.MiniGames.MarketingCampaignMiniGame;
using InGame.MiniGames.SpaceTrafficControlMiniGame.Spaceships;
using InGame.Player;
using InGame.Player.BananaGunActions;
using InGame.Pools;
using InGame.SpaceTrafficControl;
using Save;
using Settings;
using SharedInputs;
using UI;
using UI.Bananapedia;
using UI.InGame;
using UI.InGame.BananaSelector;
using UI.InGame.CommandRoomControlPanels;
using UI.InGame.Inventory;
using UI.InGame.Merchimps;
using UI.InGame.MiniChimpBlock;
using UI.InGame.MiniChimpBlock.Descriptions;
using UI.InGame.MiniChimpView;
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
    [Space]
    public GenericDictionary<CharacterType, MeteoritePool> debrisPoolByCharacterType;

    [Header("Banana Man")]
    public BananaMan bananaMan;
    public PlayerController playerController;
    public BananaGun bananaGun;
    public Scan scan;
    public Build build;
    public Shoot shoot;
    public Grab grab;
    public Trade trade;
    public Interact interact;
    [Space]
    public GameActions keyboardGameActions;
    public UiActions keyboardUiActions;
    public BananaGunActionsSwitch bananaGunActionsSwitch;
    [Space]
    public GhostsReference ghostsReference;

    [Header("inventories")]
    public BananasInventory bananasInventory;
    public DroppedInventory droppedInventory;
    public IngredientsInventory ingredientsInventory;
    public ManufacturedItemsInventory manufacturedItemsInventory;

    [Header("Gestion Mode")]
    public ScanWithMouseForDescription scanWithMouseForDescription;
    public MiniChimpDialoguesManager miniChimpDialoguesManager;
    public MiniMap miniMap;
    [Space]
    public MiniChimpViewMode miniChimpViewMode;
    
    [Header("Cameras")]
    public MainCamera mainCamera;
    public CameraPlayer cameraPlayer;
    public CameraGestionDragRotate gestionDragCamera;
    public CameraGestionRelativeMove gestionRelativeMoveCamera;
    public FootStepSurfaceDetector footStepSurfaceDetector;
    [Space]
    public Cinematiques cinematiques;
    public Death death;
    public Tutorial tutorial;
    
    [Header("Saving System")]
    public WorldData worldData;
    public GameSave gameSave;
    public GameReset gameReset;
    
    [Header("UI")]
    public UIManager uiManager;
    [Space]
    public UIBananaGun uiBananaGun;
    public UInventoriesManager uInventoriesManager;
    public UiMiniChimpBlock uiMiniChimpBlock;
    public UIDescriptionsManager uiDescriptionsManager;
    [Space]
    public UIDroppedInventory uiDroppedInventory;
    public UIIngredientsInventory uiIngredientsInventory;
    public UIBlueprintsInventory uiBlueprintsInventory;
    public UIManufacturedItemsInventory uiManufacturedItemsItemsInventory;
    public UInfobulle uInfobulle;
    [Space]
    public UIBananaSelector uiBananaSelector;
    [Space]
    public UICrosshairs uiCrosshairs;
    public UIFace uiFace;
    public UIQueuedMessages uiQueuedMessages;
    public UIHomeMenu uiHomeMenu;
    public UiGameMenu uiGameMenu;
    public UIOptionsMenu uiOptionsMenu;
    public UIBananapedia uiBananapedia;
    public UICredits uiCredits;
    public UISave uiSave;
    public UISettings uiSettings;
    [Space]
    public UIMarketingPanel uiMarketingPanel;
    public UISpaceTrafficControlPanel uiSpaceTrafficControlPanel;
    public UICannons uiCannons;
    public UIMonkeyMensReception uiTouristReception;
    public UIMerchant uiMerchant;
    [Space]
    public UIFlippers uiFlippers;
    [Space]
    public UIStats uiStats;
}