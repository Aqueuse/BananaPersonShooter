using Tweaks;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Input {
    public class HomeActions : MonoBehaviour {
        [SerializeField] private InputActionReference launchBananaInputActionReference;
        [SerializeField] private InputActionReference launchBananaWithMagicTrailInputActionReference;

        [SerializeField] private GameObject bananaPrefab;

        private void OnEnable() {
            launchBananaInputActionReference.action.Enable();
            launchBananaInputActionReference.action.performed += launchBanana;
            
            launchBananaWithMagicTrailInputActionReference.action.Enable();
            launchBananaWithMagicTrailInputActionReference.action.performed += launchBananaWithMagicTrail;
        }

        private void OnDisable() {
            launchBananaInputActionReference.action.Disable();
            launchBananaInputActionReference.action.performed -= launchBanana;
            
            launchBananaWithMagicTrailInputActionReference.action.Disable();
            launchBananaWithMagicTrailInputActionReference.action.performed -= launchBananaWithMagicTrail;
        }
    
        private void launchBanana(InputAction.CallbackContext context) {
            if (ObjectsReference.Instance.gameManager.gameContext != GameContext.IN_HOME) return;
            
            if (!ObjectsReference.Instance.uiManager.isOnSubMenus) {
                var ray = ObjectsReference.Instance.gameManager.cameraMain.ScreenPointToRay(Mouse.current.position.value);
                var direction = ray.GetPoint(1) - ray.GetPoint(0);
                var spawnedBanana = Instantiate(bananaPrefab, ray.GetPoint(2), Quaternion.LookRotation(direction));
                spawnedBanana.GetComponent<Rigidbody>().velocity = spawnedBanana.transform.forward * 10f;
            }
        }
        
        private void launchBananaWithMagicTrail(InputAction.CallbackContext context) {
            if (ObjectsReference.Instance.gameManager.gameContext != GameContext.IN_HOME) return;

            if(!ObjectsReference.Instance.uiManager.isOnSubMenus) {
                var ray = ObjectsReference.Instance.gameManager.cameraMain.ScreenPointToRay(Mouse.current.position.value);
                var direction = ray.GetPoint(1) - ray.GetPoint(0);
                var spawnedBanana = Instantiate(bananaPrefab, ray.GetPoint(2), Quaternion.LookRotation(direction));
                spawnedBanana.AddComponent<TrailRendererRandom>();
                spawnedBanana.GetComponent<Rigidbody>().velocity = spawnedBanana.transform.forward * 10f;
            }
        }
    }
}
