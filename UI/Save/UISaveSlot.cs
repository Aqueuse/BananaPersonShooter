using System.IO;
using Enums;
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
        [SerializeField] private TextMeshProUGUI saveName;
        [SerializeField] private TextMeshProUGUI saveDate;
        [SerializeField] private Image thumbail;

        public string saveUuid = "";

        public void Select() {
            //AudioManager.Instance.PlayEffect(EffectType.BUTTON_INTERACTION, 0);
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
            //AudioManager.Instance.PlayEffect(EffectType.BUTTON_INTERACTION, 0);
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
        }

        public void SetToExistingSave(string existingSaveUuid) {
            saveUuid = existingSaveUuid;

            var savedData = ObjectsReference.Instance.loadData.GetSavedDataByUuid(existingSaveUuid);
            
            saveName.text = savedData.saveName;
            saveDate.text = savedData.lastSavedDate;
            
            UpdateThumbail();
        }

        public void UpdateToExistingSave() {
            var savedData = ObjectsReference.Instance.loadData.GetSavedDataByUuid(saveUuid);
            
            saveName.text = savedData.saveName;
            saveDate.text = savedData.lastSavedDate;
            
            ObjectsReference.Instance.gameSave.SaveGameData(saveUuid);

            UpdateThumbail();
        }

        public void UpdateToNewSave(string newSaveUuid, string date) {
            saveUuid = newSaveUuid;
            saveDate.text = date;

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
        
        private void UpdateThumbail() {
            var savePath = ObjectsReference.Instance.loadData.GetSavePathByUuid(saveUuid);
            string screenshotFilePath = Path.Combine(savePath, "screenshot.png");
            
            var bytes = File.ReadAllBytes(screenshotFilePath);
            Texture2D texture2D = new Texture2D(2, 2);
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
