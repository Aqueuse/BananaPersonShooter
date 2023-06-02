using System;
using System.Globalization;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Save {
    public class UIAutoSaveSlot : MonoBehaviour {
        [SerializeField] private GameObject activatedMask;
        [SerializeField] private GameObject loadButtonGameObject;

        [SerializeField] private GameObject textPanel;
        [SerializeField] private TextMeshProUGUI saveDate;
        [SerializeField] private Image thumbail;

        public string saveUuid;
        string _autoSavePath;

        private void Start() {
            saveUuid = "auto_save";
            _autoSavePath = ObjectsReference.Instance.loadData.GetSavePathByUuid("auto_save");
        }

        public void Select() {
            ObjectsReference.Instance.audioManager.PlayEffect(EffectType.BUTTON_INTERACTION, 0);
            ObjectsReference.Instance.uiSave.UnselectAll();
            activatedMask.SetActive(true);
        }

        public void Unselect() {
            activatedMask.SetActive(false);
            loadButtonGameObject.SetActive(false);
            textPanel.SetActive(true);
        }

        public void ShowSaveOptions() {
            ObjectsReference.Instance.audioManager.PlayEffect(EffectType.BUTTON_INTERACTION, 0);
            loadButtonGameObject.SetActive(true);
            textPanel.SetActive(false);
        }

        public void Load() {
            Unselect();
            ObjectsReference.Instance.gameManager.Play("auto_save", false);
        }

        public void AutoSave() {
            var date = DateTime.ParseExact(DateTime.Now.ToString("U"), "U", CultureInfo.CurrentCulture).ToString(CultureInfo.CurrentCulture);
            SetLastTimeSinceSave();

            ObjectsReference.Instance.saveData.Save(saveUuid, date);
            UpdateThumbail();
        }
        
        private void SetLastTimeSinceSave() {
            var date = DateTime.ParseExact(DateTime.Now.ToString("U"), "U", CultureInfo.CurrentCulture);
            
            saveDate.text = date.ToString(CultureInfo.CurrentCulture);
        }

        private void UpdateThumbail() {
            _autoSavePath = ObjectsReference.Instance.loadData.GetSavePathByUuid("auto_save");
            
            string screenshotFilePath = Path.Combine(_autoSavePath, "screenshot.png");
                var bytes = File.ReadAllBytes(screenshotFilePath);
                Texture2D texture2D = new Texture2D(2, 2);
                texture2D.LoadImage(bytes);
                
                var thumbailSprite = Sprite.Create(texture2D, new Rect(0, 0, texture2D.width, texture2D.height), new Vector2(0.5f, 0.5f));
                thumbail.sprite = thumbailSprite;
        }
    }
}
