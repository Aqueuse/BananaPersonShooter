using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemPreviewBlock : MonoBehaviour {
    [SerializeField] private Image _image;

    public void SetBlock(Sprite sprite) {
        _image.sprite = sprite;
    }
}
