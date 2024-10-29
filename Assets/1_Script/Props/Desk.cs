using System.Collections.Generic;
using UnityEngine;

namespace Class
{

    public class Desk : PropsBase
    {
        private PropTypes proptype = PropTypes.Desk;
        public List<PropTypes> props = new List<PropTypes>();
        public override PropTypes PropType { get => proptype; }


        protected override void Init()
        {

        }

    }

}