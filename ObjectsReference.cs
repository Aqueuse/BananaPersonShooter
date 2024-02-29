using Audio;
using Cameras;
using InGame;
using InGame.Interactions;
using InGame.Inventory;
using InGame.Items.ItemsBehaviours;
using InGame.Items.ItemsProperties;
using InGame.MiniGames.SpaceTrafficControlMiniGame;
using InGame.Monkeys;
using InGame.Player;
using InGame.Player.PlayerActions;
using InGame.SpaceTrafficControl;
using Input;
using Save;
using Settings;
using UI;
using UI.Bananapedia;
using UI.InGame;
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

    public ChimpManager chimpManager;
    public SpaceTrafficControlMiniGameManager spaceTrafficControlMiniGameManager;
    
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

    public GameData gameData;
    public GameSave gameSave;
    public GameLoad gameLoad;
    public GameReset gameReset;
    public LoadData loadData;
    public SaveData saveData;

    public GameSettings gameSettings;
    
    public Teleportation teleportation;
    
    public DescriptionsManager descriptionsManager;
    public GestionBuild gestionBuild;
    public SpaceTrafficControlManager spaceTrafficControlManager;

    public UIManager uiManager;
    public UInventoriesManager uInventoriesManager;
    public UIHud uiHud;

    public UIBananasInventory uiBananasInventory;
    public UIRawMaterialsInventory uiRawMaterialsInventory;
    public UIIngredientsInventory uiIngredientsInventory;
    public UIBlueprintsInventory uiBlueprintsInventory;
    public UIManufacturedItemsInventory uiManufacturedItemsItemsInventory;
    
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
    
    public UIVisitorReception uiVisitorReception;
    public UIMerchant uiMerchant;
    
    public UITools uiTools;
}
