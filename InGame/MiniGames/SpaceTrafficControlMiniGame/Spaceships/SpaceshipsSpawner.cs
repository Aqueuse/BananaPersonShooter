using Save.Templates;
using UI.InGame.SpaceTrafficControlMiniGame;
using UnityEngine;
using Random = UnityEngine.Random;

namespace InGame.MiniGames.SpaceTrafficControlMiniGame.Spaceships {
    public class SpaceshipsSpawner : MonoBehaviour {
        public RectTransform[] gatesTransform;

        [HideInInspector] public RectTransform entryTransform;
        [HideInInspector] public RectTransform exitTransform;
        
        [SerializeField] private float spaceshipPropulsionSpeed = 0.4f;

        [SerializeField] private GameObject distantDotPrefab;
        
        [SerializeField] private GameObject pirateSpaceshipPrefab;
        [SerializeField] private GameObject visitorSpaceshipPrefab;
        [SerializeField] private GameObject merchantSpaceshipPrefab;

        [SerializeField] private Transform spaceshipsContainer;

        private GameObject _spaceshipInstance;
        
        public void ShowDistantDot(string spaceshipName) {
            // add a distant dot with the spaceship name and distance on the border of the canvas
            var distantDotInstance = Instantiate(original: distantDotPrefab, position: GetRandomizeEntryPoint(), 
                rotation: Quaternion.identity, parent: spaceshipsContainer);

            distantDotInstance.GetComponent<UIDistantDot>().NameSpaceship(spaceshipName);
        }
        
        private Vector3 GetRandomizeEntryPoint() {
            var entryRandomTransform = gatesTransform[Random.Range(0, 4)].localPosition;
            entryRandomTransform.y = Random.Range(-50, 50);
            entryTransform.localPosition = entryRandomTransform;
            return entryTransform.position;
        }
    
        private Vector3 GetRandomizeExitPoint() {
            var exitRandomTransform = exitTransform.localPosition;
            exitRandomTransform.y = Random.Range(-20, 20);
            exitTransform.localPosition = exitRandomTransform;

            return exitTransform.position;
        }

        public void Show2DSpaceship(SpaceshipSavedData spaceshipSavedData) {
            switch (spaceshipSavedData.characterType) {
                case CharacterType.PIRATE:
                    _spaceshipInstance = Instantiate(original: pirateSpaceshipPrefab, position: GetRandomizeEntryPoint(), 
                        rotation: Quaternion.identity, parent: spaceshipsContainer);
                    break;
                
                case CharacterType.VISITOR:
                    _spaceshipInstance = Instantiate(original: visitorSpaceshipPrefab, position: GetRandomizeEntryPoint(), 
                        rotation: Quaternion.identity, parent: spaceshipsContainer);
                    break;

                case CharacterType.MERCHIMP:
                    _spaceshipInstance = Instantiate(original: merchantSpaceshipPrefab, position: GetRandomizeEntryPoint(), 
                        rotation: Quaternion.identity, parent: spaceshipsContainer);
                    break;
            }

            var spaceship2D = _spaceshipInstance.GetComponent<Spaceship2D>(); 

            spaceship2D.spaceshipGuid = spaceshipSavedData.spaceshipGuid;
            spaceship2D.NameSpaceship(spaceshipSavedData.spaceshipName);
            spaceship2D.InitiatePropulsion(GetRandomizeExitPoint(), spaceshipPropulsionSpeed);
        }
    }
}
