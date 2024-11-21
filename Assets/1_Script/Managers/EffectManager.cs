using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Class
{
    // Horror Effect를 비롯한 Effect들을 다루기 위한 매니저 입니다.
    // TODO: Effect들을 원하는 시간과 공간에 적절하게 발동시킬 수 있어야 함.

    public class EffectManager : MonoBehaviour
    {
        #region 싱글톤 패턴

        private static EffectManager instance;
        public static EffectManager Instance { get { return instance; } }


        private void Init()
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

        #endregion


        // TODO: 오브젝트 풀링과 이펙트의 발동을 다루는 로직을 만들어야 함.
        // Sound Manager의 예를 참고하여 구현할 것.

        /*

        #region Object Pooling

        private Transform poolRoot;
        private Stack<AudioSource> audioSources = new Stack<AudioSource>();

        private void InitPool(int cnt = 10)
        {
            poolRoot = new GameObject { name = "_poolRoot" }.transform;
            poolRoot.parent = this.transform;

            for (int i = 0; i < cnt; i++)
            {
                audioSources.Push(Create());
            }
        }

        private AudioSource Create()
        {
            GameObject go = Instantiate(audioPrefab);
            go.name = audioPrefab.name;
            go.transform.parent = poolRoot;
            go.gameObject.SetActive(false);
            return go.GetComponent<AudioSource>();
        }
        private void Push(AudioSource source)
        {
            source.gameObject.SetActive(false);
            audioSources.Push(source);
        }
        private AudioSource Pop()
        {
            AudioSource source;
            if (audioSources.Count() == 0) source = Create();
            else source = audioSources.Pop();

            source.gameObject.SetActive(true);
            return source;
        }

        private IEnumerator PushAfterDelay(AudioSource source, float delay)
        {
            yield return new WaitForSeconds(delay);
            Push(source);
        }

        #endregion 

        private void Awake()
        {
            Init();
            InitPool();
        }

        [Header("GameObjects")]
        [SerializeField] private GameObject audioPrefab;

        [Header("AudioClips")]
        [SerializeField] public AudioClip[] sfxClips;

        public void CreateAudioSource(Vector3 pos, SfxClipTypes clipIdx)
        {
            AudioSource audioSource = Pop();
            audioSource.transform.position = pos;
            audioSource.clip = sfxClips[(int)clipIdx];
            audioSource.volume = 1f;
            audioSource.Play();

            StartCoroutine(PushAfterDelay(audioSource, audioSource.clip.length));
        }
        */


    }

}

