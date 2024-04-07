using Audio;
using Cameras;
using InGame;
using InGame.CommandRoomPanelControls;
using InGame.Interactions;
using InGame.Inventory;
using InGame.Items.ItemsBehaviours;
using InGame.Items.ItemsData;
using InGame.MiniGames.MarketingCampaignMiniGame;
using InGame.MiniGames.SpaceTrafficControlMiniGame.Spaceships;
using InGame.Monkeys;
using InGame.Player;
using InGame.Player.PlayerActions;
using InGame.SpaceTrafficControl;
using Save;
using Settings;
using UI;
using UI.Bananapedia;
using UI.InGame;
using UI.InGame.BananaSelector;
using UI.InGame.CommandRoomControlPanels;
using UI.InGame.Gestion;
using UI.InGame.Inventory;
using UI.InGame.Merchimps;
using UI.InGame.VisitorReceptionMiniGameUI;
using UI.Menus;
using UI.Save;
using VFX;

public class ObjectsReference : MonoSingleton<ObjectsReference> {
    public MeshReferenceScriptableObject meshReferenceScriptableObject;

    public GameManager gameManager;
    public InputManager inputManager;
    public AudioManager audioManager;
    public Interact interact;

    public CommandRoomControlPanelsManager commandRoomControlPanelsManager;
    public ChimpManager chimpManager;
    public AdMarketingCampaignManager adMarketingCampaignManager;
    public SpaceTrafficControlManager spaceTrafficControlManager;
    public SpaceshipsSpawner spaceshipsSpawner;
    public CannonsManagement cannonsManagement;

    public BananaMan bananaMan;
    public PlayerController playerController;
    public BananaGun bananaGun;
    public PlayerActionsSwitch playerActionsSwitch;
    public Scan scan;
    public Build build;
    public ThrowBanana throwBanana;
    public Grab grab;
    public Trade trade;

    public BananasInventory bananasInventory;
    public RawMaterialsInventory rawMaterialsInventory;
    public IngredientsInventory ingredientsInventory;
    public ManufacturedItemsInventory manufacturedItemsInventory;
    
    public Map map;
    
    public GhostsReference ghostsReference;
    
    public MainCamera mainCamera;
    public CameraPlayer cameraPlayer;
    public CameraGestion gestionCamera;
    public FootStepSurfaceDetector footStepSurfaceDetector;

    public Cinematiques cinematiques;
    public Death death;
    public Tutorial tutorial;

    public GameActions gameActions;
    public UiActions uiActions;

    public WorldData worldData;
    public GameSave gameSave;
    public GameReset gameReset;

    public GameSettings gameSettings;
    
    public Teleportation teleportation;
    
    public DescriptionsManager descriptionsManager;
    public GestionBuild gestionBuild;

    public UIManager uiManager;
    public UInventoriesManager uInventoriesManager;
    public UIHud uiHud;

    public UIRawMaterialsInventory uiRawMaterialsInventory;
    public UIIngredientsInventory uiIngredientsInventory;
    public UIBlueprintsInventory uiBlueprintsInventory;
    public UIManufacturedItemsInventory uiManufacturedItemsItemsInventory;

    public UIBananaSelector uiBananaSelector;
    
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

    public UIMarketingPanel uiMarketingPanel;
    public UISpaceTrafficControlPanel uiSpaceTrafficControlPanel;
    public UIVisitorReception uiTouristReception;
    public UIMerchant uiMerchant;
    
    public UITools uiTools;
}
