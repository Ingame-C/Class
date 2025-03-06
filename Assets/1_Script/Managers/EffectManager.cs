using System;
using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using System.Collections;

namespace Class
{
    /// <summary>
    /// 호러 이펙트를 포함한 모든 이펙트를 관리하는 매니저 클래스입니다.
    /// 이펙트의 생성, 활성화, 타이밍 등을 제어합니다.
    /// </summary>
    public class EffectManager : MonoBehaviour
    {
        #region Singleton
        private static EffectManager instance;
        public static EffectManager Instance { get { return instance; } }
        #endregion

        #region Serialized Fields
        [Header("Probability Settings")]
        [SerializeField, Range(0f, 1f)] private float probability;

        [Header("Timing Settings")]
        [SerializeField] private float checkTerm;
        [SerializeField, Range(0f, 300f)] private float startTime = 150f;
        #endregion

        #region Private Fields
        private float timer = 0f;
        private bool isEffectActivatable = false;
        private bool isAlreadyActivated = false;
        private List<EffectTypes> commonHorrorEffects = new List<EffectTypes>();
        private List<HorrorEffect> horrorEffects;
        #endregion

        #region Unity Methods
        private void Awake()
        {
            InitializeManager();
            StartCoroutine(SetActivateEffect());
        }

        private void Update()
        {
            CheckAndActivateEffect();
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// 이펙트 로직을 초기화합니다.
        /// </summary>
        public void ResetEffectLogic()
        {
            isAlreadyActivated = false;
            isEffectActivatable = false;
            Debug.Log("Effect Logic is reseted");
            StartCoroutine(SetActivateEffect());
        }

        /// <summary>
        /// 랜덤한 이펙트를 선택하여 활성화합니다.
        /// </summary>
        public void GetRandomEffectActivate()
        {
            var currentStage = GameManagerEx.Instance.CurrentStage;
            List<EffectTypes> effects = GetAvailableEffects(currentStage);

            var random = UnityEngine.Random.Range(0, effects.Count);
            var randomEffect = effects[random];

            ActivateSelectedEffect(randomEffect);
            isAlreadyActivated = true;
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// 매니저를 초기화합니다.
        /// </summary>
        private void InitializeManager()
        {
            LoadHorrorEffects();
            InitializeSingleton();
            InitializeCommonEffects();
        }

        /// <summary>
        /// 호러 이펙트 프리팹들을 로드합니다.
        /// </summary>
        private void LoadHorrorEffects()
        {
            horrorEffects = Resources.LoadAll<HorrorEffect>("Prefabs/Effects").ToList();
        }

        /// <summary>
        /// 싱글톤 인스턴스를 초기화합니다.
        /// </summary>
        private void InitializeSingleton()
        {
            if (instance == null)
            {
                GameObject go = GameObject.Find("@EffectManager");
                if (go == null)
                {
                    go = new GameObject { name = "@EffectManager" };
                    go.AddComponent<EffectManager>();
                }

                DontDestroyOnLoad(go);
                instance = go.GetComponent<EffectManager>();
            }
            else
            {
                Destroy(this.gameObject);
                return;
            }
        }

        /// <summary>
        /// 공통 호러 이펙트를 초기화합니다.
        /// </summary>
        private void InitializeCommonEffects()
        {
            for (EffectTypes effectType = EffectTypes.None + 1; effectType != EffectTypes.CommonEnd; effectType++)
            {
                commonHorrorEffects.Add(effectType);
            }
        }

        /// <summary>
        /// 현재 스테이지에서 사용 가능한 이펙트 목록을 반환합니다.
        /// </summary>
        private List<EffectTypes> GetAvailableEffects(int currentStage)
        {
            List<EffectTypes> effects = new List<EffectTypes>(commonHorrorEffects);

            // 스테이지 별로 EffectTypes를 추가하는 로직이 필요합니다.
            if (currentStage == 1)
            {
                effects.Add(EffectTypes.ApproachingWall);
                effects.Add(EffectTypes.ArtToolReplicator);
            }

            return effects;
        }

        /// <summary>
        /// 선택된 이펙트를 활성화합니다.
        /// </summary>
        private void ActivateSelectedEffect(EffectTypes effectType)
        {
            foreach (HorrorEffect effect in horrorEffects)
            {
                if(effect.EffectType == effectType)
                {
                    var summonedEffect = Instantiate(effect);
                    StartCoroutine(ActivateEffectCoroutine(summonedEffect.gameObject));
                    break;
                }
            }
        }

        /// <summary>
        /// 이펙트 활성화 가능 여부를 확인하고 이펙트를 활성화합니다.
        /// </summary>
        private void CheckAndActivateEffect()
        {
            if (!isEffectActivatable || isAlreadyActivated) return;
            
            timer += Time.deltaTime;
            if (timer <= checkTerm) return;

            var rand = UnityEngine.Random.Range(0f, 1f);
            timer = 0;
            Debug.Log($"EFFECT RANDOM CHECK : {rand}, {(probability - Mathf.Epsilon)}");

            if (rand >= (probability - Mathf.Epsilon)) return;

            GetRandomEffectActivate();
        }
        #endregion

        #region Coroutines
        /// <summary>
        /// 이펙트 활성화 가능 상태를 설정하는 코루틴입니다.
        /// </summary>
        private IEnumerator SetActivateEffect()
        {
            yield return new WaitForSeconds(startTime);
            isEffectActivatable = true;
        }

        /// <summary>
        /// 선택된 이펙트를 활성화하는 코루틴입니다.
        /// </summary>
        private IEnumerator ActivateEffectCoroutine(GameObject gameObject)
        {
            yield return new WaitForSeconds(0.5f);
            var effect = gameObject.GetComponent<HorrorEffect>();
            effect?.Activate();
            Debug.Log(effect?.EffectType);
        }
        #endregion
    }
}

