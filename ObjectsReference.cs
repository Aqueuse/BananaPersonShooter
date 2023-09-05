using Audio;
using Building;
using Cameras;
using Data;
using Game;
using Game.Inventory;
using Game.Steam;
using Input;
using Items;
using Player;
using Save;
using Settings;
using UI;
using UI.InGame;
using UI.InGame.Gestion;
using UI.InGame.Inventory;
using UI.InGame.QuickSlots;
using UI.Menus;
using UI.Save;
using VFX;

public class ObjectsReference : MonoSingleton<ObjectsReference> {
    public GameManager gameManager;
    public InputManager inputManager;
    public MapsManager mapsManager;
    public AudioManager audioManager;
    public InteractionsManager interactionsManager;
    public BuildablesManager buildablesManager;

    public BananaMan bananaMan;
    public PlayerController playerController;
    public BananaGun bananaGun;
    public BananaGunGet bananaGunGet;
    public BananaGunPut bananaGunPut;

    public BananasInventory bananasInventory;
    public RawMaterialsInventory rawMaterialsInventory;
    public IngredientsInventory ingredientsInventory;
    public BlueprintsInventory blueprintsInventory;
    
    public SlotSwitch slotSwitch;
    public ScriptableObjectManager scriptableObjectManager;
    public GhostsReference ghostsReference;

    public MainCamera mainCamera;
    public SurfaceDetector surfaceDetector;

    public Cinematiques cinematiques;
    public Death death;
    public ScenesSwitch scenesSwitch;
    public Tutorial tutorial;

    public GameActions gameActions;

    public GameData gameData;
    public GameSave gameSave;
    public GameLoad gameLoad;
    public GameReset gameReset;
    public LoadData loadData;
    public SaveData saveData;

    public GameSettings gameSettings;
    public SteamIntegration steamIntegration;
    
    public Teleportation teleportation;

    public UIManager uiManager;
    public UInventoriesManager uInventoriesManager;

    public UIBananasInventory uiBananasInventory;
    public UIRawMaterialsInventory uiRawMaterialsInventory;
    public UIIngredientsInventory uiIngredientsInventory;
    public UIBlueprintsInventory uiBlueprintsInventory;
    
    public UISlotsManager uiSlotsManager;
    public UICrosshairs uiCrosshairs;
    public UIFace uiFace;
    public UIQueuedMessages uiQueuedMessages;
    public UIHomeMenu uiHomeMenu;
    public UiGameMenu uiGameMenu;
    public UIOptionsMenu uiOptionsMenu;
    public UICredits uiCredits;
    public UISave uiSave;
    public UIbananaCannonMiniGame uIbananaCannonMiniGame;

    public descriptionsManager descriptionsManager;
}
