using UnityEngine;
using UnityEngine.UI;

namespace Class
{
    /// <summary>
    /// 플레이어의 시점 중앙에 UI를 표시하는 클래스입니다.
    /// 상호작용 가능한 오브젝트를 감지하여 시각적 피드백을 제공합니다.
    /// </summary>
    [RequireComponent(typeof(PlayerController))]
    public class DisplayUI : MonoBehaviour
    {
        #region Serialized Fields
        [Header("Viewport Center")]
        [SerializeField] private Image viewportCenter;
        [SerializeField] private Color originalColor;
        [SerializeField] private Color interactableColor;
        [SerializeField] private Color transparentColor;
        #endregion

        #region Private Fields
        private PlayerController controller;
        #endregion

        #region Unity Methods
        private void Awake()
        {
            InitializeComponents();
        }

        private void Update()
        {
            UpdateViewportColor();
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// 필요한 컴포넌트들을 초기화합니다.
        /// </summary>
        private void InitializeComponents()
        {
            controller = GetComponent<PlayerController>();
        }

        /// <summary>
        /// 뷰포트 중앙 UI의 색상을 업데이트합니다.
        /// UI 상태와 상호작용 가능 여부에 따라 다른 색상을 적용합니다.
        /// </summary>
        private void UpdateViewportColor()
        {
            if (controller.UIisSet)
            {
                viewportCenter.color = transparentColor;
                return;
            }
            
            viewportCenter.color = controller.IsDetectInteractable ? interactableColor : originalColor;
        }
        #endregion
    }
}