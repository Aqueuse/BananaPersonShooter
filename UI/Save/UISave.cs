using System;
using System.Globalization;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Save {
    public class UISave : MonoBehaviour {
        [SerializeField] private GameObject savePrefab;
        public GameObject newSaveButton;

        [SerializeField] private Transform savesContainer;
        
        [SerializeField] private CanvasGroup deletePanelCanvasGroup;

        [SerializeField] private CanvasGroup renamePanelCanvasGroup;
        [SerializeField] private TMP_InputField renameInputField;

        [SerializeField] private CanvasGroup thumbnailPanelCanvasGroup;

        [SerializeField] private CanvasGroup bottomPanelCanvasGroup;
        [SerializeField] private Button saveButton;
        [SerializeField] private Button renameButton;
        [SerializeField] private Button deleteButton;
        
        [SerializeField] private Image thumbail;
        [SerializeField] private TextMeshProUGUI savedDateText;
        
        public UISaveSlot selectedSaveSlot;
        public UISaveSlot autosaveSlot;

        #region Save
        public void CreateNewSave() {
            var saveUuid = DateTime.Now.ToString("yyyyMMddHHmmss");

            var date = DateTime.ParseExact(DateTime.Now.ToString("U"), "U", CultureInfo.CurrentCulture).ToString(CultureInfo.CurrentCulture);

            var save = Instantiate(savePrefab, savesContainer);
            save.GetComponent<UISaveSlot>().saveUuid = saveUuid;
            save.GetComponent<UISaveSlot>().savedDate = date;

            ObjectsReference.Instance.gameSave.SaveGame(saveUuid, "new save", date);
        }

        public void UpdateSave () {
            var date = DateTime.ParseExact(DateTime.Now.ToString("U"), "U", CultureInfo.CurrentCulture).ToString(CultureInfo.CurrentCulture);
            selectedSaveSlot.savedDate = date;

            ObjectsReference.Instance.gameSave.SaveGame(selectedSaveSlot.saveUuid, selectedSaveSlot.saveName.text, date);
            
            UpdateThumbail(selectedSaveSlot.savedDate);
        }

        public void AppendSaveSlot(string saveUuid) {
            var save = Instantiate(savePrefab, savesContainer);
            var savedData = ObjectsReference.Instance.gameSave.GetSavedByUuid(saveUuid);

            save.GetComponent<UISaveSlot>().saveUuid = saveUuid;
            save.GetComponent<UISaveSlot>().savedDate = savedData.lastSavedDate;
            save.GetComponent<UISaveSlot>().saveName.text = savedData.saveName;
        }

        public void UpdateAutoSave(string date) {
            if (autosaveSlot == null) {
                autosaveSlot = Instantiate(savePrefab, savesContainer).GetComponent<UISaveSlot>();
                
                autosaveSlot.saveUuid = "autosave";
                autosaveSlot.savedDate = date;
                autosaveSlot.saveName.text = "autosave";
            }

            else {
                autosaveSlot.savedDate = date;
            }
        }
        #endregion
        
        public void Load() {
            ObjectsReference.Instance.gameManager.Play(selectedSaveSlot.saveUuid, false);
        }
        
        #region Rename
        public void ShowRenamePanel() {
            renameInputField.text = selectedSaveSlot.saveName.text;
            SetActive(renamePanelCanvasGroup, true);
        }
        
        private void HideRenamePanel() {
            SetActive(renamePanelCanvasGroup, false);
        }

        public void ValidateRename() {
            selectedSaveSlot.saveName.text = renameInputField.text;
            ObjectsReference.Instance.gameSave.dataSave.SetSaveName(selectedSaveSlot.saveUuid, selectedSaveSlot.saveName.text);
            
            HideRenamePanel();
        }
        #endregion

        #region Thumbnail
        public void UpdateThumbail(string savedDate) {
            var savePath = ObjectsReference.Instance.gameSave.GetSavePathByUuid(selectedSaveSlot.saveUuid);
            var screenshotFilePath = Path.Combine(savePath, "screenshot.png");
            
            var bytes = File.ReadAllBytes(screenshotFilePath);
            var texture2D = new Texture2D(2, 2);
            texture2D.LoadImage(bytes);
            
            var thumbailSprite = Sprite.Create(texture2D, new Rect(0, 0, texture2D.width, texture2D.height), new Vector2(0.5f, 0.5f));
            thumbail.sprite = thumbailSprite;

            savedDateText.text = savedDate;
            
            SetActive(thumbnailPanelCanvasGroup, true);
        }

        public void HideThumbnail() {
            SetActive(thumbnailPanelCanvasGroup, false);
        }
        #endregion


        #region Delete
        public void ShowDeleteConfirmationPanel() {
            SetActive(deletePanelCanvasGroup, true);
        }

        public void HideDeleteConfirmationPanel() {
            SetActive(deletePanelCanvasGroup, false);
        }
        
        public void Delete() {
            ObjectsReference.Instance.gameSave.DeleteSave(selectedSaveSlot.saveUuid);
            Destroy(selectedSaveSlot.gameObject);
            selectedSaveSlot = null;
            
            HideDeleteConfirmationPanel();
            HideThumbnail();
        }
        #endregion
        
        public void ShowOptions(bool isAutosave) {
            renameButton.gameObject.SetActive(!isAutosave);
            deleteButton.gameObject.SetActive(!isAutosave);
            
            saveButton.gameObject.SetActive(ObjectsReference.Instance.gameManager.gameContext != GameContext.IN_HOME); 
            
            SetActive(bottomPanelCanvasGroup, true);
        }
        
        public void HideOptions() {
            SetActive(bottomPanelCanvasGroup, true);
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
