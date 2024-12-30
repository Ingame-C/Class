using UnityEngine;

namespace Class
{
    public class MirrorController : MonoBehaviour
    {
        [Header("Transforms")]
        [Space]
        [SerializeField] private Transform parentTransform;  // 거울의 기준이 되는 부모 Transform
        [SerializeField] private Transform screenTransform;  // 거울의 화면 Transform

        private Camera mirrorCam;

        private void Awake()
        {
            mirrorCam = GetComponent<Camera>();
        }

        private void Update()
        {
            if (GameManagerEx.Instance.Controller == null) return;

            Vector3 localPlayer = parentTransform.InverseTransformPoint(GameManagerEx.Instance.Controller.transform.position);

            localPlayer.y = 0;
            localPlayer.x = -localPlayer.x;

            var player = GameObject.Find(Constants.NAME_PLAYER).transform;
            transform.LookAt(player);

        }

        public void OnPreCull()
        {
            mirrorCam.ResetProjectionMatrix();
            mirrorCam.projectionMatrix = mirrorCam.projectionMatrix * Matrix4x4.Scale(new Vector3(-1, 1, 1));
        }

        public void OnPreRender()
        {
            GL.invertCulling = true;
        }

        public void OnPostRender()
        {
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
