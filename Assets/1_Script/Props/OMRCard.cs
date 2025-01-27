using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Class
{
    public class OMRProp : Grabbable
    {
        private PropTypes proptype = PropTypes.OMRCard;
        public override PropTypes PropType { get => proptype; }

        private bool isAllChecked;
        public bool IsAllChecked { get => isAllChecked; }
        protected override void Init()
        {

        }

        // TODO: OMR 카드의 체크가 끝났을 시, isAllChecked를 True로 하는 로직 필요.


    }
}