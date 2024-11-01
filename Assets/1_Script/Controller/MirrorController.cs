using UnityEngine;

namespace Class
{

    public class MirrorController : MonoBehaviour
    {

        [SerializeField] private Transform parentTransform;

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

            transform.LookAt(parentTransform.TransformPoint(localPlayer));
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


    }

}