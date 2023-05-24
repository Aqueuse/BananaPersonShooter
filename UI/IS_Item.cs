﻿using UnityEngine;

namespace UI
{
    [RequireComponent(typeof(RectTransform))]
    public class IS_Item : MonoBehaviour
    {

        public RectTransform RT() { return transform as RectTransform; }

        public float Heigh() { return RT().sizeDelta.y; }

        public void Setting()
        {
            RT().pivot = new Vector2(0.5f, 1);
        }
    }
}