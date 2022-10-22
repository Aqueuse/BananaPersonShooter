using System.Collections.Generic;
using UnityEngine;

public class WeaponsManager : MonoSingleton<WeaponsManager> {
    [SerializeField] private List<BananaType> bananaTypes;
    [SerializeField] private List<GameObject> weaponsGameObjects;

    private Dictionary<BananaType, GameObject> _weapons;

    private void Start() {
        _weapons = new Dictionary<BananaType, GameObject>();
            
        for (var i=0; i<weaponsGameObjects.Count; i++) {
            _weapons.Add(bananaTypes[i], weaponsGameObjects[i]);
        }
    }
    
    public void SetActiveWeapon(BananaType activeWeapon) {
        foreach (var weapon in _weapons) {
            if (weapon.Key == activeWeapon) {
                weapon.Value.SetActive(true);
            }
            else {
                weapon.Value.SetActive(false);
            }
        }
    }
}
