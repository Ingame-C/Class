using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;


namespace Class
{
    public class TestPaper : Usable
    {
        [SerializeField] private UI.UI testPaper;
        public override PropTypes PropType => PropTypes.TestPaper;

        private void Start()
        {
            if (testPaper == null)
                testPaper = GameObject.Find(Constants.NAME_TESTPAPERUI).GetComponent<UI.TestPaperUI>();
        }

        public override void Interact(PlayerController controller)
        {
            Debug.Log("아아아");
            controller.CurrentUI = testPaper;
        }
    }

}
