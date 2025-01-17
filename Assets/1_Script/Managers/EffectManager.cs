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
        [Header("Random Factor")]
        [SerializeField, Range(0f, 1f)] private float probability;
        [SerializeField] private float checkTerm;

        private float timer = 0f;
        private bool isEffectActivatable = false;
        private bool isAlreadyActivated = false;

        #region 싱글톤 패턴

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
        }
        #endregion

        private void Awake()
        {
            Init();
            StartCoroutine(SetActivateEffect());
        }

        private void Update()
        {
            if (!isEffectActivatable || !isAlreadyActivated) return;

            timer += Time.deltaTime;
            if (timer <= checkTerm) return;


            var rand = UnityEngine.Random.Range(0f, 1f);

            if (rand >= (probability - Mathf.Epsilon)) return;

            int curStage = GameManagerEx.Instance.CurrentStage;
            EffectTypes effectTypes = EffectTypes.None;

            // 1 스테이지
            // 파경, 학용품 복사
            if (curStage == 1)
            {
                var rand2 = UnityEngine.Random.Range(0, 2);

                if (rand2 == 0) effectTypes = EffectTypes.ArtToolReplicator;
                else if (rand2 == 1) effectTypes = EffectTypes.MirrorBreak;
            }

            // 효과 발동
            foreach (var item in horrorEffects)
            {
                if (item.EffectType == effectTypes)
                {
                    item.Activate();
                }
            }
        }

        IEnumerator SetActivateEffect()
        {
            yield return new WaitForSeconds(150f);
            isEffectActivatable = true;
        }

    }

}

