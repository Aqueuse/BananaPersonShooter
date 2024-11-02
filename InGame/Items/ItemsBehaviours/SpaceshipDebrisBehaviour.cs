using System;
using System.Linq;
using InGame.Items.ItemsData;
using InGame.MiniGames.SpaceTrafficControlMiniGame.projectiles;
using Newtonsoft.Json;
using Save.Helpers;
using UnityEngine;

namespace InGame.Items.ItemsBehaviours {
    public class SpaceshipDebrisBehaviour : MonoBehaviour {
        public string spaceshipDebrisGuid;
        public int spaceshipDebrisPrefabIndex;
        public CharacterType characterType;
        public bool isInSpace;
        
        public Transform attractionPoint;
        public bool isAttracted;
        
        private Rigidbody _rigidbody;
        private Material _material;

        [SerializeField] private float attractionForce = 10;
        
        private static readonly int emissionProperty = Shader.PropertyToID("_emission");
        private static readonly int emissionColorProperty = Shader.PropertyToID("_emission_color");

        private void Awake() {
            _rigidbody = GetComponent<Rigidbody>();
            
            if(string.IsNullOrEmpty(spaceshipDebrisGuid)) {
                spaceshipDebrisGuid = Guid.NewGuid().ToString();
            }
        }
    
        private void FixedUpdate() {
            if (!isAttracted) return;
            
            transform.position = Vector3.MoveTowards(transform.position, attractionPoint.position, 200 * Time.deltaTime);

            if (Vector3.Distance(transform.position, attractionPoint.position) < 10) {
                _rigidbody.useGravity = true;
                _rigidbody.isKinematic = false;
                
                GetComponent<MeshCollider>().isTrigger = false;

                isInSpace = false;
                isAttracted = false;
                DeactivateEmission();
            }
        }
        
        private void OnCollisionEnter(Collision other) {
            if (other.gameObject.layer == 0 & !isInSpace) {
                _rigidbody.useGravity = false;
                _rigidbody.isKinematic = true;
            }
        }

        public void DestroyIfUnreachable() {
            Invoke(nameof(DestroyMe), 60);
        }

        private void DestroyMe() {
            if (isInSpace) Destroy(gameObject);
        }

        public void Init(Laser laser) {
            isInSpace = true;
            DestroyIfUnreachable();

            if (laser.bananaEffects.Contains(BananaEffect.ATTRACTION)) {
                attractionPoint = laser.attractionPoint;
                isAttracted = true;
            }

            ActiveEmission(laser.goopColor);

            transform.parent = ObjectsReference.Instance.gameSave.spaceshipDebrisSave.spaceshipDebrisContainer.transform;
        }

        private void ActiveEmission(Color emissionColor) {
            _material = GetComponent<MeshRenderer>().materials[0];

            _material.SetFloat(emissionProperty, 1);
            _material.SetColor(emissionColorProperty, emissionColor);
        }

        private void DeactivateEmission() {
            _material.SetFloat(emissionProperty, 0);
        }

        public void GenerateSpaceshipDebrisData() {
            var spaceshipDebrisData = new SpaceshipDebrisData {
                droppedGuid = spaceshipDebrisGuid,
                spaceshipDebrisPosition = JsonHelper.FromVector3ToString(transform.position),
                spaceshipDebrisRotation = JsonHelper.FromQuaternionToString(transform.rotation),
                prefabIndex = spaceshipDebrisPrefabIndex,
                characterType = characterType,
                isInSpace = isInSpace
            };

            ObjectsReference.Instance.gameSave.spaceshipDebrisSave.AddSpaceshipDebrisToSpaceshipDebrisDictionnary(characterType, JsonConvert.SerializeObject(spaceshipDebrisData));
        }
    }
}
