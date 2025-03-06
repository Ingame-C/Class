using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.InputSystem.XR;

namespace Class
{
    /// <summary>
    /// 거울 효과를 관리하는 컨트롤러입니다.
    /// 플레이어의 움직임에 따라 거울 카메라의 방향을 조정하고 반사 효과를 처리합니다.
    /// </summary>
    public class MirrorController : MonoBehaviour
    {
        #region Serialized Fields
        [Header("Transforms")]
        [SerializeField] private Transform parentTransform;  // 거울의 기준이 되는 부모 Transform
        [SerializeField] private Transform screenTransform;  // 거울의 화면 Transform
        #endregion

        #region Private Fields
        private Transform playerTransform;    // 플레이어의 Transform
        private Camera mirrorCam;            // 거울 카메라
        private const float ROTATION_OFFSET = 90f;  // 카메라 회전 오프셋
        #endregion

        #region Unity Methods
        private void Awake()
        {
            InitializeComponents();
        }

        private void Start()
        {
            InitializePlayerTransform();
        }

        private void Update()
        {
            UpdateMirrorRotation();
        }

        private void OnPreCull()
        {
            UpdateProjectionMatrix();
        }

        private void OnPreRender()
        {
            SetInvertedCulling();
        }

        private void OnPostRender()
        {
            ResetCulling();
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// 거울 카메라의 위치를 화면 Transform과 동일하게 맞춥니다.
        /// </summary>
        [ContextMenu("Equalizing position with screen")]
        public void EqualizingPositionWithScreen()
        {
            if (screenTransform == null) return;
            transform.position = screenTransform.position;
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// 필요한 컴포넌트들을 초기화합니다.
        /// </summary>
        private void InitializeComponents()
        {
            mirrorCam = GetComponent<Camera>();
        }

        /// <summary>
        /// 플레이어 Transform을 초기화합니다.
        /// </summary>
        private void InitializePlayerTransform()
        {
            playerTransform = GameObject.Find(Constants.NAME_PLAYER)?.transform;
        }

        /// <summary>
        /// 거울 카메라의 회전을 플레이어 위치에 따라 업데이트합니다.
        /// </summary>
        private void UpdateMirrorRotation()
        {
            Vector3 directionToPlayer = playerTransform.position - mirrorCam.transform.position;
            directionToPlayer.y = 0;  // Y 축은 무시하고, 수평 방향만 반영

            if (directionToPlayer.sqrMagnitude > 0)
            {
                float angle = Mathf.Atan2(directionToPlayer.z, directionToPlayer.x) * Mathf.Rad2Deg;
                float adjustedAngle = angle + ROTATION_OFFSET;  // 초기 회전값을 고려한 각도 조정
                mirrorCam.transform.rotation = Quaternion.Euler(0, adjustedAngle, 0);
            }
        }

        /// <summary>
        /// 거울 카메라의 투영 행렬을 업데이트합니다.
        /// </summary>
        private void UpdateProjectionMatrix()
        {
            mirrorCam.ResetProjectionMatrix();
            mirrorCam.projectionMatrix = mirrorCam.projectionMatrix * Matrix4x4.Scale(new Vector3(-1, 1, 1));
        }

        /// <summary>
        /// 컬링을 반전시켜 거울 반사 효과를 만듭니다.
        /// </summary>
        private void SetInvertedCulling()
        {
            GL.invertCulling = true;
        }

        /// <summary>
        /// 컬링을 원래 상태로 되돌립니다.
        /// </summary>
        private void ResetCulling()
        {
            GL.invertCulling = false;
        }
        #endregion
    }
}
