using Enums;
using Game;
using UI.InGame;
using UnityEngine;
using UnityEngine.Localization.Settings;

namespace Building {
    public class Foundry : MonoSingleton<Foundry> {
        [SerializeField] private GameObject debrisStack;
        [SerializeField] private GameObject lavaStack;
        [SerializeField] private GameObject ingots;
    
        int ingotsQuantity;
        private int debrisQuantity;
        private float timeToConvert;

        private bool isHeating;

        private void Start() {
            timeToConvert = 4f;
            isHeating = false;
            debrisQuantity = 0;
        }

        public void Load_One_More_Debris() {
            if (Inventory.Instance.GetQuantity(ItemThrowableType.DEBRIS) > 0 && debrisQuantity < 8) {
                debrisQuantity++;
                Inventory.Instance.RemoveQuantity(ItemThrowableType.DEBRIS, 1);
                debrisStack.SetActive(true);

                if (!isHeating) {
                    Invoke(nameof(Start_To_Heat), 2f);
                    isHeating = true;
                }
            }
        }

        private void Start_To_Heat() {
            debrisStack.SetActive(false);
            lavaStack.SetActive(true);
        
            if (debrisQuantity > 0) {
                 Invoke(nameof(Add_One_more_Ingot), timeToConvert);
            }
        }

        private void Add_One_more_Ingot() {
            if (ingotsQuantity < 8) {
                ingots.SetActive(true);
                ingotsQuantity++;
                debrisQuantity--;

                MeshRenderer[] ingotsrenderers = ingots.GetComponentsInChildren<MeshRenderer>();  
            
                foreach (var meshRenderer in ingotsrenderers) {
                    meshRenderer.enabled = false;
                }

                for (var i = 0; i < ingotsQuantity; i++) {
                    ingotsrenderers[i].enabled = true;
                }

                if (debrisQuantity > 0) {
                    Invoke(nameof(Add_One_more_Ingot), timeToConvert);
                }

                else {
                    Finish_Heating();
                }
            }
        }

        private void Finish_Heating() {
            isHeating = false;
            lavaStack.SetActive(false);
        }

        public void Give_Ingots_To_Player() {
            if (ingotsQuantity == 0) return;
            
            Inventory.Instance.AddQuantity(ItemThrowableType.INGOT, ItemThrowableCategory.CRAFTABLE, ingotsQuantity);
            UIQueuedMessages.Instance.AddMessage(
                "+ "+
                ingotsQuantity+
                " "+
                LocalizationSettings.StringDatabase.GetLocalizedString("ingot")
            );
            ingotsQuantity = 0;
            debrisQuantity = 0;
            ingots.SetActive(false);
        }
    }
}
