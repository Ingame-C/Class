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
        private Image image;
        [SerializeField] private List<Material> materials;
        private int index = 0;
        private void Start()
        {
            Init();

            image = transform.GetChild(0).GetComponent<Image>();
        }

        private void changePage(int mode)
        {
            
            if (index + mode < 0 || index + mode >= materials.Count) return;
            index += mode;
            
            // TODO: 종이 넘기는 소리 추가
            image.material = materials[index];
        }

        public override void LogicUpdate()
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(rt, Input.mousePosition, mainCamera, out mousePosition);
            
            if (Math.Abs(mousePosition.y) > 400f)
            {
                RectTransform rt_image = image.GetComponent<RectTransform>();
                float y = mousePosition.y * -1f;
                rt_image.anchoredPosition = new Vector3(0, rt_image.anchoredPosition.y + (y * 0.005f), 0);
            }

            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                changePage(-1);
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                changePage(1);
            }
            Debug.Log(index);

        }
    }
}
