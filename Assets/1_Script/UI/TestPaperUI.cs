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
        
        private float timer = 0f;
        private float checkTerm = 0.3f;
        public override void LogicUpdate()
        {
            if (mousePosition.y > 400f)
            {
                RectTransform rt_image = image.GetComponent<RectTransform>();
                Vector3 vector3 = new Vector3(0f, -136.5f, 0f);
                
                rt_image.anchoredPosition = Vector3.Lerp(rt_image.anchoredPosition, vector3, Time.deltaTime);
            }
            else if (mousePosition.y < -400f)
            {
                RectTransform rt_image = image.GetComponent<RectTransform>();
                Vector3 vector3 = new Vector3(0f, 135.5f, 0f);
                
                rt_image.anchoredPosition = Vector3.Lerp(rt_image.anchoredPosition, vector3, Time.deltaTime);
            }

            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                changePage(-1);
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                changePage(1);
            }
            
            timer += Time.deltaTime;
            if (timer <= checkTerm) return;
            timer = 0;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(rt, Input.mousePosition, mainCamera, out mousePosition);
        }
    }
}
