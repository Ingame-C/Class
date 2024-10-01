using UnityEngine;

namespace Class
{

    public class Chair : PropsBase, IInteractable
    {
        private PropTypes proptype = PropTypes.Chair;
        public override PropTypes PropType { get => proptype; }


        protected override void Init()
        {

        }

        // 상호작용 UI를 나타낼 때에, 오브젝트의 이름을 가져오는 용도. 
        public override string ToString()
        {
            // to do: 유니코드 오류 고쳐서 한글 띄우게끔 하기.
            return "Chair";
        }

    }

}