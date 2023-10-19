using Audio;
using Gestion;
using Gestion.Actions;
using Cameras;
using Data;
using Game;
using Game.Inventory;
using Game.Steam;
using Input;
using Interactions;
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

    public BananaMan bananaMan;
    public PlayerController playerController;
    public BananaGun bananaGun;
    public Harvest harvest;
    public Build build;
    public ThrowBanana throwBanana;

    public BananasInventory bananasInventory;
    public RawMaterialsInventory rawMaterialsInventory;
    public IngredientsInventory ingredientsInventory;
    public BlueprintsInventory blueprintsInventory;
    
    public SlotSwitch slotSwitch;
    public ScriptableObjectManager scriptableObjectManager;
    public GhostsReference ghostsReference;
    public GestionMode gestionMode;

    public MainCamera mainCamera;
    public CameraOneAxisRotation topDownCamera;
    public CameraOneAxisRotation sideCamera;
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
    public UIHud uiHud;

    public UIBananasInventory uiBananasInventory;
    public UIRawMaterialsInventory uiRawMaterialsInventory;
    public UIIngredientsInventory uiIngredientsInventory;
    public UIBlueprintsInventory uiBlueprintsInventory;
    
    public UIQuickSlotsManager uiQuickSlotsManager;
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
