using System;
using System.Collections;
using UnityEngine;

namespace Class
{
    public class ThismanManager : MonoBehaviour
    {

        [SerializeField] private float checkTerm;                       // checkTerm 마다 디스맨이 등장할지말지 여부 확인
        [SerializeField, Range(0f, 1f)] private float probability;      // checkTerm 마다 디스맨 전조증상 나타날 확률
        [SerializeField] private float moveInTime;                      // 걸어오는 시간. 알아채기 + 숨기까지 충분한 시간 필요.
        [SerializeField] private float moveOutTime;                     // 걸어가는 시간

        [SerializeField] private bool isPlayerHide;

        private bool isChecking = false;
        private bool isComing = false;
        private float thismanTimer = 0f;

        public bool IsComing { get { return isComing; } }


        public void Init()
        {
            isChecking = true;
            thismanTimer = 0f;
            isComing = false;
        }

        private void Update()
        {
            if (!isChecking || isComing) return;

            thismanTimer += Time.deltaTime;

            if (thismanTimer > checkTerm)
            {
                if (UnityEngine.Random.Range(0f, 1f) < (probability - Mathf.Epsilon))
                {
                    StartCoroutine(ExecuteEarlySign());
                }
                thismanTimer = 0f;
            }
        }

        // 디스맨이 나타나기 전의 전조 증상입니다.
        private IEnumerator ExecuteEarlySign()
        {
            isComing = true;
            yield return StartCoroutine(MoveInCoroutine());

            isPlayerHide = GameManagerEx.Instance.Controller.StateMachine.CurState == GameManagerEx.Instance.Controller.hideState;

            if (!isPlayerHide || !GameManagerEx.Instance.IsTimerSet) // Player가 안숨음 or Timer Expired
            {
                GameManagerEx.Instance.OnStageFailed(-1);
            }
            else        // 안숨음
            {
                yield return StartCoroutine(MoveOutCoroutine());
            }

        }

        private IEnumerator MoveInCoroutine()
        {
            float elapsedTime = 0f;


            while (elapsedTime < moveInTime)
            {
                // TODO : 걷는 소리. 점점 크게
                Debug.Log("뚜벅");
                yield return new WaitForSeconds(0.6f); // 걷는clip Length 를 받아와야 합니다.
                elapsedTime += 0.6f;
            }

        }

        private IEnumerator MoveOutCoroutine()
        {
            float elapsedTime = 0f;

            while (elapsedTime < moveOutTime)
            {
                // TODO : 걷는 소리. 점점 작게
                Debug.Log("저벅");
                yield return new WaitForSeconds(0.6f); // 걷는clip Length 를 받아와야 합니다.
                elapsedTime += 0.6f;
            }
            isComing = false;
        }



        private void OnGameEnd()
        {
            isChecking = false;
        }

    }
}
