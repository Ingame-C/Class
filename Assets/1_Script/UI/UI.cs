using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;


namespace Class.UI
{
    public abstract class UI : MonoBehaviour
    {
        protected PlayerController controller;
        protected RectTransform rt;
        protected Camera mainCamera;
        protected virtual void Init()
        {
            controller = GameObject.Find(Constants.NAME_PLAYER).GetComponent<PlayerController>();
            rt = this.transform as RectTransform;
            mainCamera = Camera.main;
        }
        
        // State의 LogicUpdate에서 작동하는 함수.
        public abstract void LogicUpdate();
    }
}