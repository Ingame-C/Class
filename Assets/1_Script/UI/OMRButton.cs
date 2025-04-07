using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Class.UI
{
    public class OMRButton : MonoBehaviour
    {

        private Toggle toggle;
        private LecternManager lecternManager;

        private void Awake()
        {
            toggle = GetComponent<Toggle>();
             lecternManager = GameObject.Find("@LecternManager").GetComponent<LecternManager>();
        }

        private void Start()
        {
            toggle.onValueChanged.AddListener(OmrCheckEvent);
        }

        private void OmrCheckEvent(bool value)
        {
            var index = GetParentIndex() - 1;
            Debug.Log(index);
            lecternManager.isOnList[index] = value;
        }

        private int GetParentIndex()
        {
            
            return int.Parse(transform.parent.name);
        }
        
        


    }
}