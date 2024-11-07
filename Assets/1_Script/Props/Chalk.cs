using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Class{
    public class Chalk : Usable
    {

        private PropTypes proptype = PropTypes.Chalk;
        public override PropTypes PropType => proptype;

        public override void Interact(PlayerController controller)
        {
            // TODO: 스테이지 번호 받아서, 칠판 컨트롤러와 연결, 힌트 뛰우기.
        }
    }
}