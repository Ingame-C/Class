using UnityEngine;

namespace Class {

    // Grabbable과 분리하여, 함수 호출 한번으로 간단한 Interact를 하기위한 클래스
    public abstract class Usable : PropsBase
    {
        public abstract void Interact(PlayerController controller);
    }

}

