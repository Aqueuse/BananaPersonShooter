using System;
using InGame.Items.ItemsData;
using InGame.MiniGames.SpaceTrafficControlMiniGame.projectiles;
using Newtonsoft.Json;
using Save.Helpers;
using UnityEngine;

namespace InGame.Items.ItemsBehaviours {
    public class SpaceshipDebrisBehaviour : MonoBehaviour {
        public string spaceshipDebrisGuid;

        public SpaceshipType spaceshipType;
        public int prefabIndex;

        public bool isInSpace;

        public Vector3 effectSource;
        public BananaEffect bananaEffect;

        private Rigidbody _rigidbody;
        private Material _material;

        private static readonly int emissionProperty = Shader.PropertyToID("_emission");
        private static readonly int emissionColorProperty = Shader.PropertyToID("_emission_color");

        private void Awake() {
            _rigidbody = GetComponent<Rigidbody>();

            if(string.IsNullOrEmpty(spaceshipDebrisGuid)) {
                spaceshipDebrisGuid = Guid.NewGuid().ToString();
            }
        }

        private void Update() {
            if (bananaEffect != BananaEffect.ATTRACTION) return;

            transform.position = Vector3.MoveTowards(transform.position, effectSource, 200 * Time.deltaTime);

            if (Vector3.Distance(transform.position, effectSource) < 10) {
                _rigidbody.useGravity = true;
                _rigidbody.isKinematic = false;

                gameObject.layer = 7; // Gestion mode Selectable
                
                GetComponent<MeshCollider>().isTrigger = false;

                isInSpace = false;
                bananaEffect = BananaEffect.NONE;
                DeactivateEmission();
            }
        }

        public void Init(Laser laser) {
            Debug.Log("laser shoot");
            
            isInSpace = true;
            DestroyIfUnreachable();

            if (laser.bananaEffect == BananaEffect.ATTRACTION) {
                bananaEffect = BananaEffect.ATTRACTION;
                effectSource = laser.attractionPoint.position;
            }
            
            ActiveEmission(laser.goopColor);

            transform.parent = ObjectsReference.Instance.gameSave.debrisContainer.transform;
        }
        
        public void DestroyIfUnreachable() {
            Invoke(nameof(DestroyMe), 60);
        }

        private void DestroyMe() {
            if (isInSpace) Destroy(gameObject);
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
                prefabIndex = prefabIndex,
                spaceshipType = spaceshipType,
                isInSpace = isInSpace,
                bananaEffect = bananaEffect,
                effectSourcePosition = JsonHelper.FromVector3ToString(effectSource)
            };

            ObjectsReference.Instance.gameSave.spaceshipDebrisSave.AddSpaceshipDebrisToSpaceshipDebrisDictionnary(
                spaceshipType, JsonConvert.SerializeObject(spaceshipDebrisData)
            );
        }
        
        private void OnCollisionEnter(Collision other) {
            if (other.gameObject.layer == 0 && !isInSpace) {
                _rigidbody.useGravity = false;
                _rigidbody.isKinematic = true;
            }
        }
    }
}
