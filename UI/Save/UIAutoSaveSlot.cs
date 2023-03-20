using System;
using System.Globalization;
using System.IO;
using Audio;
using Enums;
using Game;
using Save;
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
        string autoSavePath;

        private void Start() {
            saveUuid = "auto_save";
            autoSavePath = LoadData.Instance.GetSavePathByUuid("auto_save");
        }

        public void Select() {
            AudioManager.Instance.PlayEffect(EffectType.BUTTON_INTERACTION, 0);
            UISave.Instance.UnselectAll();
            activatedMask.SetActive(true);
        }

        public void Unselect() {
            activatedMask.SetActive(false);
            loadButtonGameObject.SetActive(false);
            textPanel.SetActive(true);
        }

        public void ShowSaveOptions() {
            AudioManager.Instance.PlayEffect(EffectType.BUTTON_INTERACTION, 0);
            loadButtonGameObject.SetActive(true);
            textPanel.SetActive(false);
        }

        public void Load() {
            Unselect();
            GameManager.Instance.Play("auto_save", false);
        }

        public void AutoSave() {
            var date = DateTime.ParseExact(DateTime.Now.ToString("U"), "U", CultureInfo.CurrentCulture).ToString(CultureInfo.CurrentCulture);
            SetLastTimeSinceSave();

            SaveData.Instance.Save(saveUuid, date);
            UpdateThumbail();
        }
        
        private void SetLastTimeSinceSave() {
            var date = DateTime.ParseExact(DateTime.Now.ToString("U"), "U", CultureInfo.CurrentCulture);
            
            saveDate.text = date.ToString(CultureInfo.CurrentCulture);
        }

        private void UpdateThumbail() {
            autoSavePath = LoadData.Instance.GetSavePathByUuid("auto_save");
            
            string screenshotFilePath = Path.Combine(autoSavePath, "screenshot.png");
                var bytes = File.ReadAllBytes(screenshotFilePath);
                Texture2D texture2D = new Texture2D(2, 2);
                texture2D.LoadImage(bytes);
                
                var thumbailSprite = Sprite.Create(texture2D, new Rect(0, 0, texture2D.width, texture2D.height), new Vector2(0.5f, 0.5f));
                thumbail.sprite = thumbailSprite;
        }
    }
}
