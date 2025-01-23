using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Class
{
    public class Lectern : PropsBase
    {
        private int curStage;
        public Grabbable Grabbable;
        private PropTypes proptype = PropTypes.Lectern;
        public override PropTypes PropType { get => proptype; }
        protected override void Init()
        {

        }

    }
}