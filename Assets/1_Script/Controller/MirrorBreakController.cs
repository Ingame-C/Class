using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Class;
using Hanzzz.MeshDemolisher;
using TMPro;
using UnityEngine;

namespace Class {
    /// <summary>
    /// 거울 파괴 효과를 관리하는 컨트롤러입니다.
    /// 거울이 파괴될 때 메시 분할과 파티클 효과를 처리합니다.
    /// </summary>
    public class MirrorBreakController : MonoBehaviour
    {
        #region Serialized Fields
        [Header("Target Settings")]
        [SerializeField] private GameObject targetGameObject;    // 파괴될 거울 오브젝트
        [SerializeField] private Transform breakPointsParent;    // 파괴 지점들의 부모 오브젝트
        [SerializeField] private Material interiorMaterial;      // 거울 내부 재질
        [SerializeField] private GameObject _camera;             // 거울 카메라
        [SerializeField] private int index;                      // 거울 인덱스

        [Header("Break Settings")]
        [SerializeField][Range(0f, 1f)] private float resultScale;  // 파괴 후 결과물 크기
        [SerializeField] private Transform resultParent;             // 파괴 결과물의 부모 오브젝트
        #endregion

        #region Private Fields
        private static MeshDemolisher meshDemolisher = new MeshDemolisher();
        private Vector3 prevPosition;    // 이전 프레임의 위치
        #endregion

        #region Unity Methods
        private void Start()
        {
            prevPosition = targetGameObject.transform.position;

            if (_camera == null)
            {
                _camera = GameObject.Find($"Mirror Camera {index}");
            }
        }

        private void FixedUpdate()
        {
            UpdateCameraPosition();
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// 거울을 파괴하는 효과를 실행합니다.
        /// </summary>
        [ContextMenu("Demolish")]
        public void Demolish()
        {
            PlayBreakSound();
            
            List<Transform> breakPoints = Enumerable.Range(0, breakPointsParent.childCount).Select(x => breakPointsParent.GetChild(x)).ToList();

            var watch = System.Diagnostics.Stopwatch.StartNew();
            List<GameObject> res = meshDemolisher.Demolish(targetGameObject, breakPoints, interiorMaterial);
            watch.Stop();
            //logText.text = $"Demolish time: {watch.ElapsedMilliseconds}ms.";

            res.ForEach(x => x.transform.SetParent(resultParent, true));
            res.ForEach(x => x.AddComponent<BoxCollider>());
            res.ForEach(x => x.AddComponent<Rigidbody>());
            res.ForEach(x => x.GetComponent<Rigidbody>().AddForce(Vector3.right * 3, ForceMode.Impulse));
            res.ForEach(x => Destroy(x, 1));
            Enumerable.Range(0, resultParent.childCount).Select(i => resultParent.GetChild(i)).ToList().ForEach(x => x.localScale = resultScale * Vector3.one);
            //ClearPreviousResults();
            targetGameObject.SetActive(false);
        }

        [ContextMenu("Reset")]
        public void Reset()
        {
            //Enumerable.Range(0,breakPointsParent.childCount).Select(i=>breakPointsParent.GetChild(i)).ToList().ForEach(x=>DestroyImmediate(x.gameObject));
            Enumerable.Range(0, resultParent.childCount).Select(i => resultParent.GetChild(i)).ToList().ForEach(x => DestroyImmediate(x.gameObject));
            targetGameObject.SetActive(true);
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// 카메라 위치를 거울 오브젝트의 이동에 맞춰 업데이트합니다.
        /// </summary>
        private void UpdateCameraPosition()
        {
            Vector3 deltaPosition = targetGameObject.transform.position - prevPosition;
            if (deltaPosition.magnitude > 0f)
            {
                _camera.transform.position += deltaPosition;
            }
            prevPosition = targetGameObject.transform.position;
        }

        /// <summary>
        /// 거울 파괴 소리를 재생합니다.
        /// </summary>
        private void PlayBreakSound()
        {
            SoundManager.Instance.CreateAudioSource(transform.position, SfxClipTypes.Mirror_Break, 1.0f);
        }

        /// <summary>
        /// 이전 파괴 결과물을 제거합니다.
        /// </summary>
        private void ClearPreviousResults()
        {
            Enumerable.Range(0, resultParent.childCount)
                     .Select(i => resultParent.GetChild(i))
                     .ToList()
                     .ForEach(x => DestroyImmediate(x.gameObject));
        }
        #endregion
    }
}
