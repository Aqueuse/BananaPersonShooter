using System.Collections.Generic;
using System.Linq;
using Game;
using UnityEngine;

namespace UI.InGame.MapUI {
    public class DotsPositionnement : MonoBehaviour {
        [SerializeField] private GameObject dotDebrisPrefab;
        [SerializeField] private GameObject dotBananaTreePrefab;
        
        [SerializeField] private GameObject initialDebrisPositions;
        [SerializeField] private GameObject initialBananaTreesPositions;

        [SerializeField] private GameObject dotsDebrisContainer;
        [SerializeField] private GameObject dotsBananaTreesContainer;

        private List<Transform> _debrisTransforms;
        private List<Transform> _bananaTreesTransforms;

        public string mapName;
        private Map _map;

        private List<Vector3> _debrisPositions;
        private List<Vector3> _bananaTreesPositions;

        GameObject _debrisDot;
        GameObject _bananaTreeDot;
        
        private void Start() {
            _map = MapsManager.Instance.mapBySceneName[mapName];
            
            _debrisPositions = new List<Vector3>();
            _bananaTreesPositions = new List<Vector3>();

            RefreshMap();
        }

        private void RefreshMap() {
            //// Bananiers ////
            _bananaTreesTransforms = initialBananaTreesPositions.gameObject.GetComponentsInChildren<Transform>().ToList();
            _bananaTreesTransforms.RemoveAt(0);
            
            foreach (var position in _bananaTreesTransforms) {
                _bananaTreesPositions.Add(position.localPosition);
            }
            
            foreach (var position in _bananaTreesPositions) {
                _bananaTreeDot = Instantiate(dotBananaTreePrefab, dotsBananaTreesContainer.transform);
                _bananaTreeDot.GetComponent<RectTransform>().localPosition = new Vector3(position.x, position.z, 0);
            }

            //// Debris ////
            if (_map.debrisPosition.Length > 0) {
                _debrisPositions = _map.debrisPosition.ToList();
            }

            else {
                _debrisTransforms = initialDebrisPositions.gameObject.GetComponentsInChildren<Transform>().ToList();
                _debrisTransforms.RemoveAt(0);

                _bananaTreesTransforms = initialBananaTreesPositions.gameObject.GetComponentsInChildren<Transform>().ToList();
                _bananaTreesTransforms.RemoveAt(0);
                
                foreach (var position in _debrisTransforms) {
                    _debrisPositions.Add(position.localPosition);
                }
            }

            foreach (var position in _debrisPositions) {
                _debrisDot = Instantiate(dotDebrisPrefab, dotsDebrisContainer.transform);
                _debrisDot.GetComponent<RectTransform>().localPosition = new Vector3(position.x, position.z, 0);
            }

            
            if (!MapsManager.Instance.mapBySceneName[mapName].isShowingDebris) {
                dotsDebrisContainer.SetActive(false);
            }
            
            if (!MapsManager.Instance.mapBySceneName[mapName].isShowingBananaTrees) {
                dotsBananaTreesContainer.SetActive(false);
            }
        }

        public void ShowHideDebris() {
            dotsDebrisContainer.SetActive(!dotsDebrisContainer.activeInHierarchy);
            MapsManager.Instance.mapBySceneName[mapName].isShowingDebris = dotsDebrisContainer.activeInHierarchy;
        }

        public void ShowHideBananaTrees() {
            dotsBananaTreesContainer.SetActive(!dotsBananaTreesContainer.activeInHierarchy);
            MapsManager.Instance.mapBySceneName[mapName].isShowingBananaTrees = dotsBananaTreesContainer.activeInHierarchy;
        }
    }
}
