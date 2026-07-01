using System;
using InGame.Items.ItemsData;
using Newtonsoft.Json;
using Save.Helpers;
using UnityEngine;

namespace InGame.Items.ItemsBehaviours {
    public class SpaceshipDebrisBehaviour : MonoBehaviour {
        public string spaceshipDebrisGuid;

        public SpaceshipType spaceshipType;
        public int prefabIndex;

        public bool isInSpace;
        public bool isAttracted;
        
        [SerializeField] private Rigidbody _rigidbody;
        
        [SerializeField] private MeshRenderer _meshRenderer;
        private static readonly int sizeMultiplier = Shader.PropertyToID("_SizeMultiplier");
        private static readonly int Emission = Shader.PropertyToID("_Emission");
        
        private void Awake() {
            if(string.IsNullOrEmpty(spaceshipDebrisGuid)) {
                spaceshipDebrisGuid = Guid.NewGuid().ToString();
            }
        }

        private void Update() {
            if (!isAttracted) return;
            
            transform.position = Vector3.MoveTowards(transform.position, ObjectsReference.Instance.cannonsManager.activeCannon.launcherTransform.position, 200 * Time.deltaTime);
            
            
            if (Vector3.Distance(transform.position, ObjectsReference.Instance.cannonsManager.activeCannon.launcherTransform.position) < 10) {
                _meshRenderer.material.SetFloat(sizeMultiplier, 1f);
                _meshRenderer.material.SetFloat(Emission, 0f);
                
                _rigidbody.useGravity = true;
                _rigidbody.isKinematic = false;

                gameObject.layer = 7; // Gestion mode Selectable
                
                GetComponent<MeshCollider>().isTrigger = false;

                isInSpace = false;
                isAttracted = false;
            }
        }

        public void Init() {
            isInSpace = true;
            _meshRenderer.material.SetFloat(sizeMultiplier, 10);
            _meshRenderer.material.SetFloat(Emission, 1f);
            
            _rigidbody.AddExplosionForce(5f, transform.position, 5f);
            _rigidbody.AddTorque(transform.position, ForceMode.Impulse);

            DestroyIfUnreachable();
            
            transform.parent = ObjectsReference.Instance.gameSave.savablesItemsContainer;
        }
        
        public void DestroyIfUnreachable() {
            Invoke(nameof(DestroyMe), 60);
        }

        private void DestroyMe() {
            if (isInSpace) Destroy(gameObject);
        }

        public void GenerateSpaceshipDebrisData() {
            var spaceshipDebrisData = new SpaceshipDebrisData {
                droppedGuid = spaceshipDebrisGuid,
                spaceshipDebrisPosition = JsonHelper.FromVector3ToString(transform.position),
                spaceshipDebrisRotation = JsonHelper.FromQuaternionToString(transform.rotation),
                prefabIndex = prefabIndex,
                spaceshipType = spaceshipType,
                isInSpace = isInSpace
            };

            ObjectsReference.Instance.gameSave.spaceshipDebrisSave.AddSpaceshipDebrisToSpaceshipDebrisDictionnary(
                spaceshipType, JsonConvert.SerializeObject(spaceshipDebrisData)
            );
        }
        
        private void OnCollisionEnter(Collision other) {
            if (isInSpace) return;
            
            if (other.gameObject.layer == 11 | other.gameObject.layer == 0) {
                _rigidbody.useGravity = false;
                _rigidbody.isKinematic = true;
            }
        }
    }
}
