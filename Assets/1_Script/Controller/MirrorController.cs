using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.InputSystem.XR;

namespace Class
{
    public class MirrorController : MonoBehaviour
    {
        [Header("Transforms")]
        [Space]
        [SerializeField] private Transform parentTransform;  // 거울의 기준이 되는 부모 Transform
        [SerializeField] private Transform screenTransform;  // 거울의 화면 Transform

        private Transform playerTransform;
        private Camera mirrorCam;

        private void Awake()
        {
            mirrorCam = GetComponent<Camera>();
        }

        private void Start()
        {
            playerTransform = GameObject.Find(Constants.NAME_PLAYER)?.transform;
        }

        private void Update()
        {
            Vector3 directionToPlayer = playerTransform.position - mirrorCam.transform.position;
            directionToPlayer.y = 0;  // Y 축은 무시하고, 수평 방향만 반영

            // 반사된 위치를 계산하여 카메라 회전 (거울처럼 반사된 이미지를 비추기 위해)
            if (directionToPlayer.sqrMagnitude > 0)
            {
                // 플레이어의 반사된 방향을 계산
                float angle = Mathf.Atan2(directionToPlayer.z, directionToPlayer.x) * Mathf.Rad2Deg;

                // 카메라의 초기 회전값을 고려하여 반사된 각도 설정
                float adjustedAngle = angle + 90f;  // 초기 회전값을 고려해 90도를 더함

                // 카메라의 회전을 반사된 방향으로 설정
                mirrorCam.transform.rotation = Quaternion.Euler(0, adjustedAngle, 0);

            }

        }

        private void OnPreCull()
        {
            mirrorCam.ResetProjectionMatrix();
            mirrorCam.projectionMatrix = mirrorCam.projectionMatrix * Matrix4x4.Scale(new Vector3(-1, 1, 1));
        }

        private void OnPreRender()
        {
            // 거울 카메라 렌더링 전에 컬링 반전 (거울 반사 처리)
            GL.invertCulling = true;
        }

        private void OnPostRender()
        {
            // 렌더링 후 컬링 복구
            GL.invertCulling = false;
        }

        [ContextMenu("Equalizing position with screen")]
        public void EqualizingPositionWithScreen()
        {
            if (screenTransform == null) return;

            Vector3 pos = screenTransform.position;
            transform.position = pos;
        }
    }
}
