using System;
using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using System.Collections;

namespace Class
{
    // Horror Effect를 비롯한 Effect들을 다루기 위한 매니저 입니다.
    // TODO: Effect들을 원하는 시간과 공간에 적절하게 발동시킬 수 있어야 함.

    public class EffectManager : MonoBehaviour
    {
        [Header("Probability")]
        [SerializeField, Range(0f, 1f)] private float probability;
        [Space]
        [Header("Check term")]
        [SerializeField] private float checkTerm;
        [Space]
        [Header("Start time")]
        [SerializeField, Range(0f, 300f)] private float startTime = 150f;

        private float timer = 0f;
        private bool isEffectActivatable = false;
        private bool isAlreadyActivated = false;
        private List<EffectTypes> commonHorrorEffects = new List<EffectTypes>();

        private static EffectManager instance;
        public static EffectManager Instance { get { return instance; } }

        private List<HorrorEffect> horrorEffects;
        private void Init()
        {
            horrorEffects = Resources.LoadAll<HorrorEffect>("Prefabs/Effects").ToList();

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

            // 공용 효과를 추가하는 로직입니다.
            for (EffectTypes effectType = EffectTypes.None + 1; effectType != EffectTypes.CommonEnd; effectType++)
            {
                commonHorrorEffects.Add(effectType);
            }
            
        }

        private void Awake()
        {
            Init();
            StartCoroutine(SetActivateEffect());
        }

        private void Update()
        {
            
            // 이펙트가 실행 불가한 시간이거나, 이미 실행됐다면 return
            if (!isEffectActivatable || isAlreadyActivated) return;
            
            timer += Time.deltaTime;
            // checkTerm이 아니라면 return
            if (timer <= checkTerm) return;

            var rand = UnityEngine.Random.Range(0f, 1f);
            timer = 0;
            Debug.Log($"EFFECT RANDOM CHECK : {rand}, {(probability - Mathf.Epsilon)}");

            // 확률을 뚫지 못했다면 return
            if (rand >= (probability - Mathf.Epsilon)) return;

            // 이펙트 실행!
            GetRandomEffectActivate();
        }

        
        public void GetRandomEffectActivate()
        {
            var currentStage = GameManagerEx.Instance.CurrentStage;
            List<EffectTypes> effects = new List<EffectTypes>(commonHorrorEffects);

            // 스테이지 별로 EffectTypes를 추가하는 로직이 필요합니다.
            if (currentStage == 1)
            {
                effects.Add(EffectTypes.ApproachingWall);
                effects.Add(EffectTypes.ArtToolReplicator);
            }

            var random = UnityEngine.Random.Range(0, effects.Count());
            var randomEffect = effects[random];

            foreach (HorrorEffect effect in horrorEffects)
            {
                if(effect.EffectType == randomEffect)
                {
                    var summonedEffect = Instantiate(effect);
                    StartCoroutine(ActivateEffect(summonedEffect.gameObject));
                    break;
                }
            }


            isAlreadyActivated = true;
        }

        IEnumerator SetActivateEffect()
        {
            yield return new WaitForSeconds(startTime);
            isEffectActivatable = true;
        }

        IEnumerator ActivateEffect(GameObject gameObject)
        {
            yield return new WaitForSeconds(0.5f);
            gameObject.GetComponent<HorrorEffect>()?.Activate();
            Debug.Log(gameObject.GetComponent<HorrorEffect>()?.EffectType);
            Destroy(gameObject, 5f);
        }

    }

}

