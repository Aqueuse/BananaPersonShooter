using UnityEngine;

namespace InGame.SpaceTrafficControl {
    public class Hangars : MonoSingleton<Hangars> {
        [SerializeField] private GenericDictionary<int, Transform> spaceshipSpawnTransformByHangarNumber;
        [SerializeField] private GenericDictionary<int, GameObject> hangarDoorByHangarNumber;

        [SerializeField] private GenericDictionary<int, GameObject> spaceshipsByHangarNumber;

        [SerializeField] private Transform hangarCameraTransform;

        public void Spawn3DSpaceshipInHangar(int hangarNumber, GameObject spaceshipPrefab) {
            // verify if there is already a spaceship at hangar
            if (spaceshipsByHangarNumber[hangarNumber] == null) {
                // instantiate spaceship at hangar
                var spaceship3D = Instantiate(
                    original: spaceshipPrefab,
                    position: spaceshipSpawnTransformByHangarNumber[hangarNumber].position,
                    rotation: spaceshipSpawnTransformByHangarNumber[hangarNumber].rotation);

                // spawn visitors or init merchant or spawn pirates
            }
        }

        public void CloseHangar(int hangarNumber) {
            hangarDoorByHangarNumber[hangarNumber].SetActive(true);
        }

        public void RotateCameraLeft() {
            hangarCameraTransform.Rotate(-45, 0, 0);
        }

        public void RotateCameraRight() {
            hangarCameraTransform.Rotate(45, 0, 0);
        }
    }
}