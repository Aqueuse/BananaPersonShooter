using System;
using System.Globalization;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Save {
    public class UISaveSlot : MonoBehaviour {
        [SerializeField] private CanvasGroup activatedMaskCanvasGroup;
        [SerializeField] private CanvasGroup loadButtonGameObjectCanvasGroup;
        [SerializeField] private CanvasGroup saveButtonGameObjectCanvasGroup;
        [SerializeField] private CanvasGroup renameButtonGameObjectCanvasGroup;
        [SerializeField] private CanvasGroup renameInputFieldGameObjectCanvasGroup;
        [SerializeField] private CanvasGroup deleteButtonGameObjectCanvasGroup;

        [SerializeField] private CanvasGroup yesButtonCanvasGroup;
        [SerializeField] private CanvasGroup noButtonCanvasGroup;

        [SerializeField] private GameObject saveRootGameObject;

        [SerializeField] private CanvasGroup textPanelCanvasGroup;
        public TextMeshProUGUI saveName;
        public TextMeshProUGUI saveDate;
        [SerializeField] private Image thumbail;

        public string saveUuid = "";
        
        public void Select() {
            ObjectsReference.Instance.uiSave.UnselectAll();
            
            SetActive(activatedMaskCanvasGroup, true);
        }

        public void Unselect() {
            SetActive(activatedMaskCanvasGroup, false);
            
            SetActive(loadButtonGameObjectCanvasGroup, false);
            SetActive(saveButtonGameObjectCanvasGroup, false);
            SetActive(renameButtonGameObjectCanvasGroup, false);
            SetActive(deleteButtonGameObjectCanvasGroup, false);
            SetActive(yesButtonCanvasGroup, false);
            SetActive(noButtonCanvasGroup, false);
            SetActive(textPanelCanvasGroup, false);
        }

        public void ShowSaveOptions() {
            SetActive(loadButtonGameObjectCanvasGroup, true);
            SetActive(renameButtonGameObjectCanvasGroup, true);
            SetActive(deleteButtonGameObjectCanvasGroup, true);
            SetActive(textPanelCanvasGroup, false);

            if (ObjectsReference.Instance.gameManager.gameContext == GameContext.IN_GAME_MENU) {
                SetActive(saveButtonGameObjectCanvasGroup, true);
            }
        }
        
        public void Load() {
            ObjectsReference.Instance.audioManager.PlayEffect(SoundEffectType.BUTTON_INTERACTION, 0);
            ObjectsReference.Instance.gameManager.Play(saveUuid, false);
            ObjectsReference.Instance.uiSave.UnselectAll();
        }

        public void Save () {
            var date = DateTime.ParseExact(DateTime.Now.ToString("U"), "U", CultureInfo.CurrentCulture).ToString(CultureInfo.CurrentCulture);
            
            if (ObjectsReference.Instance.gameSave.SaveExists(saveUuid)) {
                var savedData = ObjectsReference.Instance.gameSave.GetSavedByUuid(saveUuid);
            
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
            SetActive(renameButtonGameObjectCanvasGroup, true);
        }

        public void ValidateRename() {
            saveName.text = renameInputFieldGameObjectCanvasGroup.GetComponentInChildren<TMP_InputField>().text;
            ObjectsReference.Instance.gameSave.dataSave.SetSaveName(saveUuid, saveName.text);
            
            SetActive(renameButtonGameObjectCanvasGroup, false);
        }
        
        public void UpdateThumbail() {
            var savePath = ObjectsReference.Instance.gameSave.GetSavePathByUuid(saveUuid);
            var screenshotFilePath = Path.Combine(savePath, "screenshot.png");
            
            var bytes = File.ReadAllBytes(screenshotFilePath);
            var texture2D = new Texture2D(2, 2);
            texture2D.LoadImage(bytes);
            
            var thumbailSprite = Sprite.Create(texture2D, new Rect(0, 0, texture2D.width, texture2D.height), new Vector2(0.5f, 0.5f));
            thumbail.sprite = thumbailSprite;
        }

        public void ShowDeleteConfirmationButtons() {
            SetActive(deleteButtonGameObjectCanvasGroup, false);
            SetActive(yesButtonCanvasGroup, true);
            SetActive(noButtonCanvasGroup, true);
        }

        public void HideDeleteConfirmationButtons() {
            SetActive(deleteButtonGameObjectCanvasGroup, true);
            SetActive(yesButtonCanvasGroup, false);
            SetActive(noButtonCanvasGroup, false);
        }

        public void Delete() {
            ObjectsReference.Instance.audioManager.PlayEffect(SoundEffectType.BUTTON_INTERACTION, 0);
            Destroy(saveRootGameObject);
            ObjectsReference.Instance.gameSave.DeleteSave(saveUuid);
        }

        private void SetActive(CanvasGroup canvasGroup, bool isActive) {
            if (isActive) {
                canvasGroup.alpha = 1;
                canvasGroup.interactable = true;
                canvasGroup.blocksRaycasts = true;
            }
            else {
                canvasGroup.alpha = 0;
                canvasGroup.interactable = false;
                canvasGroup.blocksRaycasts = false;
            }
        }
    }
}
