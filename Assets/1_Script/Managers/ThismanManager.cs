using System;
using System.Collections;
using Class.StateMachine;
using UnityEngine;

namespace Class
{
    /// <summary>
    /// 경비 디스맨의 등장과 행동을 관리하는 매니저입니다.
    /// </summary>
    public class ThismanManager : MonoBehaviour
    {
        #region Spawn Settings
        [Header("디스맨 등장 설정")]
        [SerializeField] private float checkInterval = 5f;           // 디스맨 등장 체크 간격
        [SerializeField, Range(0f, 1f)] private float spawnProbability = 0.3f;  // 등장 확률
        [SerializeField] private float approachTime = 10f;          // 접근 시간
        [SerializeField] private float retreatTime = 8f;            // 퇴장 시간
        #endregion

        #region Time Settings
        [Header("시간 설정")]
        private const float INITIAL_WAIT_TIME = 150f;              // 첫 등장 대기 시간 (2분 30초)
        private const float ACTIVE_TIME = 180f;                    // 활성화 시간 (3분)
        private const float FOOTSTEP_INTERVAL = 2.338f;            // 발자국 소리 간격
        private const float FOOTSTEP_VOLUME_INCREASE = 0.07f;      // 발자국 소리 볼륨 증가율
        #endregion

        #region State Variables
        private bool isPlayerHidden;
        private bool isGameOver;
        private bool isActiveTime;
        private bool isChecking;
        private bool isApproaching;
        private float checkTimer;
        private PlayerController playerController;

        public bool IsApproaching => isApproaching;
        #endregion

        #region Unity Methods
        private void Start()
        {
            if (GameManagerEx.Instance.CurrentStage < 3) return;
            StartCoroutine(ManageActiveTime());
        }

        private void Update()
        {
            if (!isActiveTime || !isChecking || isApproaching) return;

            checkTimer += Time.deltaTime;
            if (checkTimer >= checkInterval)
            {
                CheckForSpawn();
                checkTimer = 0f;
            }
        }
        #endregion

        #region Initialization
        public void Init()
        {
            isChecking = true;
            checkTimer = 0f;
            isApproaching = false;
            playerController = GameObject.Find(Constants.NAME_PLAYER)?.GetComponent<PlayerController>();
        }
        #endregion

        #region Spawn Logic
        private void CheckForSpawn()
        {
            float randomValue = UnityEngine.Random.Range(0f, 1f);
            if (randomValue < (spawnProbability - Mathf.Epsilon))
            {
                StartCoroutine(HandleThismanApproach());
            }
        }

        private IEnumerator HandleThismanApproach()
        {
            isActiveTime = false;
            isApproaching = true;

            yield return StartCoroutine(ApproachCoroutine());

            isPlayerHidden = GameManagerEx.Instance.Controller.StateMachine.CurrentState == GameManagerEx.Instance.Controller.hideState;

            if (!isPlayerHidden || !GameManagerEx.Instance.IsTimerSet)
            {
                isGameOver = false;
                GameManagerEx.Instance.OnStageFailed(GameManagerEx.Instance.CurrentStage);
            }
            else
            {
                yield return StartCoroutine(RetreatCoroutine());
            }
        }
        #endregion

        #region Movement Coroutines
        private IEnumerator ApproachCoroutine()
        {
            float elapsedTime = 0f;
            while (elapsedTime < approachTime)
            {
                PlayFootstepSound(elapsedTime);
                yield return new WaitForSeconds(FOOTSTEP_INTERVAL);
                elapsedTime += 0.6f;
            }
        }

        private IEnumerator RetreatCoroutine()
        {
            float elapsedTime = 0f;
            while (elapsedTime < retreatTime)
            {
                // TODO: 발자국 소리 점진적 감소 구현
                yield return new WaitForSeconds(0.6f);
                elapsedTime += 0.6f;
            }
            isApproaching = false;
        }
        #endregion

        #region Sound Effects
        private void PlayFootstepSound(float elapsedTime)
        {
            SoundManager.Instance.CreateAudioSource(
                transform.position, 
                SfxClipTypes.Thisman_Walk, 
                0.0f, 
                elapsedTime * FOOTSTEP_VOLUME_INCREASE
            );
        }
        #endregion

        #region Time Management
        private IEnumerator ManageActiveTime()
        {
            yield return new WaitForSeconds(INITIAL_WAIT_TIME);
            isActiveTime = true;
            yield return new WaitForSeconds(ACTIVE_TIME);
            isActiveTime = false;

            while (!isGameOver)
            {
                yield return new WaitForSeconds(ACTIVE_TIME);
                isActiveTime = true;
            }
        }
        #endregion

        #region Public Methods
        public void IncreaseSpawnProbability()
        {
            spawnProbability *= 2;
        }
        #endregion
    }
}
