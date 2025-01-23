using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Class
{
    public class Lectern : PropsBase
    {
        private int curStage;
        public Grabbable Grabbable;
        public bool IsClear {  get => isCleared; }
        private bool isCleared;
        private PropTypes proptype = PropTypes.Lectern;
        public override PropTypes PropType { get => proptype; }
        protected override void Init()
        {

        }

        private void Start()
        {
            curStage = GameManagerEx.Instance.CurrentStage;
        }


        private void Update()
        {
            if (Grabbable == null || curStage != 2) return;
            if (Grabbable is OMRProp omrCard)
            {
                if (omrCard.IsAllChecked)
                {
                    Debug.Log("clear");
                    isCleared = true;
                }
            }
        }

    }
}