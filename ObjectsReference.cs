using Audio;
using Building;
using Cameras;
using Data;
using Game;
using Game.Advancement;
using Game.Steam;
using Input;
using Items;
using Monkeys.Chimployee;
using Player;
using Save;
using Settings;
using UI;
using UI.InGame;
using UI.InGame.Advancements;
using UI.InGame.Blueprints;
using UI.InGame.Inventory;
using UI.InGame.QuickSlots;
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

    public BananaMan bananaMan;
    public PlayerController playerController;
    public BananaGun bananaGun;
    public BananaGunGet bananaGunGet;
    public BananaGunPut bananaGunPut;

    public Inventory inventory;
    public SlotSwitch slotSwitch;
    public ScriptableObjectManager scriptableObjectManager;
    public GhostsReference ghostsReference;

    public MainCamera mainCamera;
    public SurfaceDetector surfaceDetector;

    public Cinematiques cinematiques;
    public Death death;
    public ScenesSwitch scenesSwitch;

    public GameActions gameActions;
    public Advancements advancements;

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
    public UInventory uInventory;
    public UIBlueprints uiBlueprints;
    public UISlotsManager uiSlotsManager;
    public Chimployee chimployee;
    public UICrosshairs uiCrosshairs;
    public UIadvancements uIadvancements;
    public UIHelper uiHelper;
    public UIFace uiFace;
    public Uihud uihud;
    public UIQueuedMessages uiQueuedMessages;
    public UIHomeMenu uiHomeMenu;
    public UiGameMenu uiGameMenu;
    public UIOptionsMenu uiOptionsMenu;
    public UICredits uiCredits;
    public UISave uiSave;
    public UIbananaCannonMiniGame uIbananaCannonMiniGame;
}
