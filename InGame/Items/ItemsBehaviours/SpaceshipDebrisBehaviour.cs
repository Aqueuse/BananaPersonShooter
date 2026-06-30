using System;
using InGame.Items.ItemsData;
using InGame.MiniGames.SpaceTrafficControl.projectiles;
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

        public Vector3 effectSource;

        [SerializeField] private Rigidbody _rigidbody;

        private void Awake() {
            if(string.IsNullOrEmpty(spaceshipDebrisGuid)) {
                spaceshipDebrisGuid = Guid.NewGuid().ToString();
            }
        }

        private void Update() {
            if (!isAttracted) return;
            
            transform.position = Vector3.MoveTowards(transform.position, effectSource, 200 * Time.deltaTime);

            if (Vector3.Distance(transform.position, effectSource) < 10) {
                _rigidbody.useGravity = true;
                _rigidbody.isKinematic = false;

                gameObject.layer = 7; // Gestion mode Selectable
                
                GetComponent<MeshCollider>().isTrigger = false;

                isInSpace = false;
                isAttracted = false;
            }
        }

        public void Init(Laser laser) {
            isInSpace = true;
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
                isInSpace = isInSpace,
                effectSourcePosition = JsonHelper.FromVector3ToString(effectSource)
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
