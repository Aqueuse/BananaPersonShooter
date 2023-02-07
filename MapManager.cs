using Audio;
using Enums;
using MiniChimps;
using Monkeys;
using Monkeys.Gorilla;
using Save;
using UI.InGame.Statistics;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class MapManager : MonoSingleton<MapManager> {
    [SerializeField] private GenericDictionary<MonkeyType, GameObject> monkeyPrefabsByType;
    [SerializeField] private GameObject monkeySpawnPoint;
    
    public MonkeyType activeMonkeyType;
    public float cleanliness;

    private Vector3 _bananaManDirectionToCenter;
    public Monkey activeMonkey;
    
    public int maxDebrisQuantity;
    private int _actualDebrisQuantity;

    private string sceneName;

    private void Start() {
        sceneName = SceneManager.GetActiveScene().name.ToUpper();
        maxDebrisQuantity = 28;

        SpawnMonkey(activeMonkeyType);
        
        if (LoadMapData.Instance.HasData()) GameSave.Instance.RespawnDebrisOnMap(sceneName);

        RecalculateHapiness();
    }
    
    public void RecalculateHapiness() {
        _actualDebrisQuantity = GameObject.FindGameObjectWithTag("debrisContainer").GetComponentInChildren<Transform>().childCount;
            
        cleanliness = 50-((_actualDebrisQuantity /(float)maxDebrisQuantity)*50);
        activeMonkey.happiness = activeMonkey.sasiety + cleanliness;

        GameSave.Instance.mapDatasBySceneNames[sceneName].cleanliness = cleanliness;
        GameSave.Instance.mapDatasBySceneNames[sceneName].monkeySasiety = activeMonkey.sasiety;
        
        UIStatistics.Instance.Refresh_Map_Statistics(activeMonkeyType);
        
        if (activeMonkey.happiness < 20 && activeMonkey.monkeyState != MonkeyState.ANGRY) {
            activeMonkey.monkeyState = MonkeyState.ANGRY;
            HautParleurs.Instance.PlayHappinessLevelClip(MonkeyState.ANGRY);
        }
            
        if (activeMonkey.happiness is >= 20 and < 30 && activeMonkey.monkeyState != MonkeyState.SAD) {
            activeMonkey.monkeyState = MonkeyState.SAD;
            activeMonkey.GetComponent<GorillaSounds>().PlaySadMonkeySounds();
            HautParleurs.Instance.PlayHappinessLevelClip(MonkeyState.SAD);
        }
            
        if (activeMonkey.happiness >= 60 && activeMonkey.monkeyState != MonkeyState.HAPPY) {
            activeMonkey.monkeyState = MonkeyState.HAPPY;
            HautParleurs.Instance.PlayHappinessLevelClip(MonkeyState.HAPPY);
        }
    }

    public void Clean() {
        if (cleanliness <= 50) {
            RecalculateHapiness();
            activeMonkey.associatedUI.SetSliderValue(activeMonkey.happiness, activeMonkey.monkeyState);
        }
    }
        
    public GameObject GetActiveBoss() {
        return activeMonkey.gameObject;
    }
        
    public void StartBossFight(MonkeyType monkeyType) {
        GameManager.Instance.isFigthing = true;
        AudioManager.Instance.PlayMusic(MusicType.BOSS, true);

        activeMonkeyType = monkeyType;
    }

    private void SpawnMonkey(MonkeyType monkeyType) {
        if (monkeyType == MonkeyType.KELSAIK) {
            NavMeshTriangulation navMeshTriangulation = NavMesh.CalculateTriangulation();
                
            int vertexIndex = Random.Range(0, navMeshTriangulation.vertices.Length);
                
            // instanciate monkey
            var boss = Instantiate(monkeyPrefabsByType[activeMonkeyType], monkeySpawnPoint.transform.position, Quaternion.identity);
            activeMonkey = boss.GetComponent<Monkey>();

            if (NavMesh.SamplePosition(navMeshTriangulation.vertices[vertexIndex], out NavMeshHit navMeshHit, 2f, 0)) {
                boss.GetComponent<NavMeshAgent>().Warp(navMeshHit.position);
            }

            boss.transform.localScale = new Vector3(1, 1, 1);
        }
    }
}