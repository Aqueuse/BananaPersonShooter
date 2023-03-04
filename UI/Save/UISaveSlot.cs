using System.IO;
using Audio;
using Enums;
using Game;
using Save;
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
            AudioManager.Instance.PlayEffect(EffectType.BUTTON_INTERACTION);
            UISave.Instance.UnselectAll();
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
            AudioManager.Instance.PlayEffect(EffectType.BUTTON_INTERACTION);
            loadButtonGameObject.SetActive(true);
            renameButtonGameObject.SetActive(true);
            deleteButtonGameObject.SetActive(true);
            textPanel.SetActive(false);

            if (GameManager.Instance.gameContext == GameContext.IN_GAME) {
                saveButtonGameObject.SetActive(true);
            }
        }
        
        public void Load() {
            AudioManager.Instance.PlayEffect(EffectType.BUTTON_INTERACTION);
            GameManager.Instance.Play(saveUuid, false);
        }

        public void SetToExistingSave(string existingSaveUuid) {
            saveUuid = existingSaveUuid;
            AudioManager.Instance.PlayEffect(EffectType.BUTTON_INTERACTION);

            var savedData = LoadData.Instance.GetSavedDataByUuid(existingSaveUuid);
            
            saveName.text = savedData.saveName;
            saveDate.text = savedData.lastSavedDate;
            
            UpdateThumbail();
        }

        public void UpdateToExistingSave() {
            AudioManager.Instance.PlayEffect(EffectType.BUTTON_INTERACTION);

            var savedData = LoadData.Instance.GetSavedDataByUuid(saveUuid);
            
            saveName.text = savedData.saveName;
            saveDate.text = savedData.lastSavedDate;
            
            GameSave.Instance.SaveGameData(saveUuid);

            UpdateThumbail();
        }

        public void UpdateToNewSave(string newSaveUuid, string date) {
            AudioManager.Instance.PlayEffect(EffectType.BUTTON_INTERACTION);

            saveUuid = newSaveUuid;
            saveDate.text = date;

            UpdateThumbail();
        }

        public void Rename() {
            AudioManager.Instance.PlayEffect(EffectType.BUTTON_INTERACTION);
            renameInputFieldGameObject.SetActive(true);
        }

        public void ValidateRename() {
            saveName.text = renameInputFieldGameObject.GetComponentInChildren<TMP_InputField>().text;
            SaveData.Instance.SaveName(saveUuid, saveName.text);
            renameInputFieldGameObject.SetActive(false);
        }


        private void UpdateThumbail() {
            var savePath = LoadData.Instance.GetSavePathByUuid(saveUuid);
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
            AudioManager.Instance.PlayEffect(EffectType.BUTTON_INTERACTION);
            Destroy(saveRootGameObject);
            SaveData.Instance.DeleteSave(saveUuid);
        }
    }
}
