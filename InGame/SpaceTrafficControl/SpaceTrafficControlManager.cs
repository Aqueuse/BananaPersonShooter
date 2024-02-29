using InGame.Items.ItemsBehaviours.SpaceshipsBehaviours;
using Save.Templates;
using UnityEngine;

namespace InGame.SpaceTrafficControl {
    public class SpaceTrafficControlManager : MonoBehaviour {
        [SerializeField] private GameObject[] pirateSpaceshipPrefabs;
        [SerializeField] private GameObject[] merchantSpaceshipPrefabs;
        [SerializeField] private GameObject[] visitorsSpaceshipPrefabs;

        public GenericDictionary<string, SpaceshipBehaviour> spaceshipBehavioursByGuid;

        /// LOAD ///

        public void LoadSpaceshipBehaviour(CharacterType characterType, SpaceshipSavedData spaceshipSavedData) {
            var spaceship = Instantiate(GetSpaceshipPrefabByIndex(characterType, spaceshipSavedData.prefabIndex));
            
            spaceshipBehavioursByGuid.Add(spaceship.GetComponent<SpaceshipBehaviour>().spaceshipGuid, spaceship.GetComponent<SpaceshipBehaviour>());

            switch (spaceshipSavedData.characterType) {
                case CharacterType.PIRATE:
                    spaceship.GetComponent<PirateSpaceshipBehaviour>().piratesData = spaceshipSavedData.pirateDatas;
                    break;
                
                case CharacterType.VISITOR:
                    spaceship.GetComponent<VisitorSpaceshipBehaviour>().visitorDatas = spaceshipSavedData.visitorDatas;
                    break;
                
                case CharacterType.MERCHIMP:
                    spaceship.GetComponent<MerchantSpaceshipBehaviour>().merchantData = spaceshipSavedData.merchantData;
                    break;
            }
            
            spaceship.GetComponent<SpaceshipBehaviour>().Init();
        }
        
        ///  SPAWN  ///

        public void SpawnSpaceshipBehaviour(CharacterType characterType, int hangarNumber) {
            var spaceship = Instantiate(GetRandomSpaceshipByCharacterType(characterType));
            
            spaceship.GetComponent<SpaceshipBehaviour>().GenerateGuid();
            spaceship.GetComponent<SpaceshipBehaviour>().GenerateName();
            
            spaceshipBehavioursByGuid.Add(spaceship.GetComponent<SpaceshipBehaviour>().spaceshipGuid, spaceship.GetComponent<PirateSpaceshipBehaviour>());
            
            spaceship.GetComponent<SpaceshipBehaviour>().Init();
        }
        
        ///  REMOVE ///

        public void DestroySpacehip(int spaceshipGuid) {
            
        }
        
        public SpaceshipBehaviour GetSpaceshipBehaviourByGuid(string spaceshipGuid) {
            return spaceshipBehavioursByGuid[spaceshipGuid];
        }
        
        private GameObject GetRandomSpaceshipByCharacterType(CharacterType characterType) {
            switch (characterType) {
                case CharacterType.PIRATE:
                    return pirateSpaceshipPrefabs[Random.Range(0, pirateSpaceshipPrefabs.Length)];
                case CharacterType.VISITOR:
                    return visitorsSpaceshipPrefabs[Random.Range(0, visitorsSpaceshipPrefabs.Length)];
                case CharacterType.MERCHIMP:
                    return merchantSpaceshipPrefabs[Random.Range(0, merchantSpaceshipPrefabs.Length)];
            }

            return null;
        }

        private GameObject GetSpaceshipPrefabByIndex(CharacterType characterType, int prefabIndex) {
            switch (characterType) {
                case CharacterType.PIRATE:
                    return pirateSpaceshipPrefabs[prefabIndex];
                case CharacterType.VISITOR:
                    return visitorsSpaceshipPrefabs[prefabIndex];
                case CharacterType.MERCHIMP:
                    return merchantSpaceshipPrefabs[prefabIndex];
            }

            return null;
        }
    }
}
