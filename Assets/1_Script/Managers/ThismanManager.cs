using System;
using System.Collections;
using Class.StateMachine;
using UnityEngine;

namespace Class
{
    // 경비 디스맨의 매니저 입니다.
    public class ThismanManager : MonoBehaviour
    {

        [SerializeField] private float checkTerm;                       // checkTerm 마다 디스맨이 등장할지말지 여부 확인
        [SerializeField, Range(0f, 1f)] private float probability;      // checkTerm 마다 디스맨 전조증상 나타날 확률
        [SerializeField] private float moveInTime;                      // 걸어오는 시간. 알아채기 + 숨기까지 충분한 시간 필요.
        [SerializeField] private float moveOutTime;                     // 걸어가는 시간

        [SerializeField] private bool isPlayerHide;

        
        private bool isGameOver = false;
        private bool isTimeToCome = false;
        private bool isChecking = false;
        private bool isComing = false;
        private float thismanTimer = 0f;
        private PlayerController controller;

        public bool IsComing { get { return isComing; } }


        public void Init()
        {
            isChecking = true;
            thismanTimer = 0f;
            isComing = false;
            controller =  GameObject.Find(Constants.NAME_PLAYER)?.GetComponent<PlayerController>();
        }

        private void Start()
        {
            // 3 스테이지 이전이라면 경비 디스맨은 나타나지 않는다.
            if (GameManagerEx.Instance.CurrentStage < 3) return;
            
            StartCoroutine(WaitTimeToCome());
        }

        private void Update()
        {
            if (!isTimeToCome) return;
            if (!isChecking || isComing) return;

            thismanTimer += Time.deltaTime;

            if (thismanTimer > checkTerm)
            {
                float rand = UnityEngine.Random.Range(0f, 1f);
                Debug.Log($"RANDOM CHECK : {rand}, {(probability - Mathf.Epsilon)}");
                if (rand < (probability - Mathf.Epsilon))
                {
                    StartCoroutine(ExecuteEarlySign());
                }
                thismanTimer = 0f;
            }
        }

        // 디스맨이 나타나기 전의 전조 증상입니다.
        private IEnumerator ExecuteEarlySign()
        {
            isTimeToCome = false;
            isComing = true;
            yield return StartCoroutine(MoveInCoroutine());

            isPlayerHide = GameManagerEx.Instance.Controller.StateMachine.CurState == GameManagerEx.Instance.Controller.hideState;

            if (!isPlayerHide || !GameManagerEx.Instance.IsTimerSet) // Player가 안숨음 or Timer Expired
            {
                isGameOver = false;
                GameManagerEx.Instance.OnStageFailed(-1);
            }
            else        // 숨음
            {
                yield return StartCoroutine(MoveOutCoroutine());
            }

        }

        private IEnumerator MoveInCoroutine()
        {
            float elapsedTime = 0f;


            while (elapsedTime < moveInTime)
            {
                SoundManager.Instance.CreateAudioSource(transform.position, SfxClipTypes.Thisman_Walk, 0.0f, elapsedTime * 0.07f);
                yield return new WaitForSeconds(2.338f); // 걷는clip Length 를 받아와야 합니다.
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

        public void IncreaseProb()
        {
            probability *= 2;
        }
        
        // 경비 디스맨의 등장 가능한 시간대를 조정하는 함수 입니다.
        IEnumerator WaitTimeToCome()
        {
            // 2분 30초부터 5분 30초까지 디스맨이 나타날 수 있음.
            yield return new WaitForSeconds(150f);
            isTimeToCome = true;
            yield return new WaitForSeconds(180f);
            isTimeToCome = false;
            // 그 이후 3분 마다 디스맨이 나타날 수도 있음.
            while (!isGameOver)
            {
                yield return new WaitForSeconds(180f);
                isTimeToCome = true;
            }
        }
    }
}
