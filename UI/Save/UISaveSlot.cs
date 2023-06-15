using System;
using System.Globalization;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Save {
    public class UISaveSlot : MonoBehaviour {
        [SerializeField] private GameObject activatedMask;
        [SerializeField] private GameObject loadButtonGameObject;
        [SerializeField] private GameObject saveButtonGameObject;
        [SerializeField] private GameObject renameButtonGameObject;
        [SerializeField] private GameObject renameInputFieldGameObject;
        [SerializeField] private GameObject deleteButtonGameObject;

        [SerializeField] private GameObject yesButton;
        [SerializeField] private GameObject noButton;

        [SerializeField] private GameObject saveRootGameObject;

        [SerializeField] private GameObject textPanel;
        public TextMeshProUGUI saveName;
        public TextMeshProUGUI saveDate;
        [SerializeField] private Image thumbail;

        public string saveUuid = "";
        
        public void Select() {
            ObjectsReference.Instance.uiSave.selectedSaveSlot = this;
            ObjectsReference.Instance.uiSave.UnselectAll();
            activatedMask.SetActive(true);
        }

        public void Unselect() {
            activatedMask.SetActive(false);
            loadButtonGameObject.SetActive(false);
            saveButtonGameObject.SetActive(false);
            renameButtonGameObject.SetActive(false);
            deleteButtonGameObject.SetActive(false);
            yesButton.SetActive(false);
            noButton.SetActive(false);
            textPanel.SetActive(true);
        }

        public void ShowSaveOptions() {
            loadButtonGameObject.SetActive(true);
            renameButtonGameObject.SetActive(true);
            deleteButtonGameObject.SetActive(true);
            textPanel.SetActive(false);

            if (ObjectsReference.Instance.gameManager.gameContext == GameContext.IN_GAME) {
                saveButtonGameObject.SetActive(true);
            }
        }
        
        public void Load() {
            ObjectsReference.Instance.audioManager.PlayEffect(EffectType.BUTTON_INTERACTION, 0);
            ObjectsReference.Instance.gameManager.Play(saveUuid, false);
            ObjectsReference.Instance.uiSave.UnselectAll();
        }

        public void Save () {
            var date = DateTime.ParseExact(DateTime.Now.ToString("U"), "U", CultureInfo.CurrentCulture).ToString(CultureInfo.CurrentCulture);
            
            if (ObjectsReference.Instance.loadData.SaveExists(saveUuid)) {
                var savedData = ObjectsReference.Instance.loadData.GetSavedDataByUuid(saveUuid);
            
                saveName.text = savedData.saveName;
                saveDate.text = date;
            }

            else {
                saveDate.text = date;
            }
            
            ObjectsReference.Instance.gameSave.SaveGame(saveUuid);

            UpdateThumbail();
        }
        
        public void Rename() {
            renameInputFieldGameObject.SetActive(true);
        }

        public void ValidateRename() {
            saveName.text = renameInputFieldGameObject.GetComponentInChildren<TMP_InputField>().text;
            ObjectsReference.Instance.saveData.SaveName(saveUuid, saveName.text);
            renameInputFieldGameObject.SetActive(false);
        }
        
        public void UpdateThumbail() {
            var savePath = ObjectsReference.Instance.loadData.GetSavePathByUuid(saveUuid);
            var screenshotFilePath = Path.Combine(savePath, "screenshot.png");
            
            var bytes = File.ReadAllBytes(screenshotFilePath);
            var texture2D = new Texture2D(2, 2);
            texture2D.LoadImage(bytes);
            
            var thumbailSprite = Sprite.Create(texture2D, new Rect(0, 0, texture2D.width, texture2D.height), new Vector2(0.5f, 0.5f));
            thumbail.sprite = thumbailSprite;
        }

        public void ShowDeleteConfirmationButtons() {
            deleteButtonGameObject.SetActive(false);
            yesButton.SetActive(true);
            noButton.SetActive(true);
        }

        public void HideDeleteConfirmationButtons() {
            deleteButtonGameObject.SetActive(true);
            yesButton.SetActive(false);
            noButton.SetActive(false);
        }

        public void Delete() {
            ObjectsReference.Instance.audioManager.PlayEffect(EffectType.BUTTON_INTERACTION, 0);
            Destroy(saveRootGameObject);
            ObjectsReference.Instance.saveData.DeleteSave(saveUuid);
        }
    }
}
