using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Class {
    /// <summary>
    /// 힌트 표시를 관리하는 컨트롤러입니다.
    /// 스테이지별로 다른 힌트 표시 방식을 처리합니다.
    /// </summary>
    public class HintController : MonoBehaviour
    {
        #region Serialized Fields
        [Header("Game Objects")]
        [SerializeField] private GameObject hint;           // 힌트 오브젝트
        [SerializeField] private GameObject blackBoard;     // 칠판 오브젝트
        [SerializeField] private GameObject hintGuard;      // 힌트 가드 오브젝트

        [Header("Hint Guard Settings")]
        [SerializeField] private float endTime = 3f;        // 가드 이동 종료 시간
        [SerializeField] private float speed = 3f;          // 가드 이동 속도

        [Header("Stage 1 Settings")]
        [SerializeField] private GameObject tvScreen;       // TV 화면 오브젝트
        [SerializeField] private Material[] DesksImages;    // 책상 이미지 배열

        [Header("Stage 2 Settings")]
        [SerializeField] private GameObject Lectern;        // 강의대 오브젝트
        [SerializeField] private Material emissiveLecternMaterial;  // 발광 강의대 재질
        #endregion

        #region Private Fields
        private int currentStage;      // 현재 스테이지 번호
        private float elapsedTime;     // 경과 시간
        #endregion

        #region Unity Methods
        private void Start()
        {
            currentStage = GameManagerEx.Instance.CurrentStage;
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// 현재 스테이지에 맞는 힌트를 표시합니다.
        /// </summary>
        public void SetHintAppear()
        {
            switch (currentStage)
            {
                case 1:
                    ShowStage1Hint();
                    break;
                case 2:
                    ShowStage2Hint();
                    break;
                default:
                    StartCoroutine(MoveHintGuard());
                    break;
            }
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// 스테이지 1의 힌트를 표시합니다.
        /// TV 화면에 책상 이미지를 표시합니다.
        /// </summary>
        private void ShowStage1Hint()
        {
            int imageIndex = DeskManager.Instance.PresetIndex;
            tvScreen.GetComponent<MeshRenderer>().material = DesksImages[imageIndex];
        }

        /// <summary>
        /// 스테이지 2의 힌트를 표시합니다.
        /// 강의대를 발광 상태로 만듭니다.
        /// </summary>
        private void ShowStage2Hint()
        {
            Lectern.GetComponent<MeshRenderer>().material = emissiveLecternMaterial;
        }

        /// <summary>
        /// 힌트 가드를 이동시키는 코루틴입니다.
        /// </summary>
        private IEnumerator MoveHintGuard()
        {
            elapsedTime = 0f;
            while (elapsedTime < endTime)
            {
                hintGuard.transform.position += Vector3.right * speed * Time.deltaTime;
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            
            elapsedTime = 0f;
            hintGuard.SetActive(false);
        }
        #endregion
    }
}
