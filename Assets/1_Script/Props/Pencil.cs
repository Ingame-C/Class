using UnityEngine;

namespace Class
{
    public class Pencil : Grabbable, IInteractable
    {
        private PropTypes proptype = PropTypes.Pencil;
        public override PropTypes PropType { get => proptype; }

        protected override void Init()
        {

        }

    }
}
