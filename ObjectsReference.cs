using Audio;
using Building;
using Cameras;
using Data;
using Game;
using Input;
using Items;
using Monkeys;
using Player;
using Save;
using Settings;
using UI;
using UI.InGame;
using UI.InGame.Blueprints;
using UI.InGame.Inventory;
using UI.InGame.QuickSlots;
using UI.InGame.Statistics;
using UI.Menus;
using UI.Save;
using UI.Tutorials;
using VFX;

public class ObjectsReference : MonoSingleton<ObjectsReference> {
    public GameManager gameManager;
    public InputManager inputManager;
    public MapsManager mapsManager;
    public AudioManager audioManager;
    public ItemsManager itemsManager;
    public MonkeysManager monkeysManager;

    public BananaMan bananaMan;
    public BananaGun bananaGun;
    public BananaGunGet bananaGunGet;
    public BananaGunPut bananaGunPut;

    public Inventory inventory;
    public SlotSwitch slotSwitch;
    public ScriptableObjectManager scriptableObjectManager;
    public GhostsReference ghostsReference;

    public MainCamera mainCamera;

    public Cinematiques cinematiques;
    public Death death;
    public ScenesSwitch scenesSwitch;

    public GameActions gameActions;

    public GameData gameData;
    public GameSave gameSave;
    public GameLoad gameLoad;
    public GameReset gameReset;
    public LoadData loadData;
    public SaveData saveData;

    public GameSettings gameSettings;

    public Teleportation teleportation;

    public UIManager uiManager;
    public UInventory uInventory;
    public UIBlueprints uiBlueprints;
    public UISlotsManager uiSlotsManager;
    public UIStatistics uiStatistics;
    public UICrosshair uiCrosshair;
    public UIFace uiFace;
    public Uihud uihud;
    public UIQueuedMessages uiQueuedMessages;
    public UIHomeMenu uiHomeMenu;
    public UiGameMenu uiGameMenu;
    public UIOptionsMenu uiOptionsMenu;
    public UISave uiSave;
    public TutorialsManager tutorialsManager;
    public UIbananaCannonMiniGame uIbananaCannonMiniGame;
}
