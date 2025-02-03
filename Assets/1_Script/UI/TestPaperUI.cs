using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace  Class.UI
{
    public class TestPaperUI : UI
    {
        private Vector2 mousePosition;
        private List<Image> images = new List<Image>();
        
        private void Start()
        {
            Init();

            foreach (var image in this.GetComponentsInChildren<Image>())
            {
                images.Add(image);
            }
        }

        public override void LogicUpdate()
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(rt, Input.mousePosition, mainCamera, out mousePosition);
            
            if (Math.Abs(mousePosition.y) > 400f)
            {
                foreach (var image in images)
                {
                    RectTransform rt_image = image.GetComponent<RectTransform>();
                    rt_image.anchoredPosition = new Vector3(0, rt_image.anchoredPosition.y - (mousePosition.y * 0.005f), 0);
                }
            }
            
        }
    }
}
