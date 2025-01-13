using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

namespace Class {

    public class SoundManager : MonoBehaviour
    {
        #region 싱글톤 패턴

        private static SoundManager instance;
        public static SoundManager Instance { get { return instance; } }


        private void Init()
        {
            if (instance == null)
            {
                GameObject go = GameObject.Find("@SoundManager");
                if (go == null)
                {
                    go = new GameObject { name = "@SoundManager" };
                    go.AddComponent<SoundManager>();
                }

                DontDestroyOnLoad(go);
                instance = go.GetComponent<SoundManager>();

            }
            else
            {
                Destroy(this.gameObject);
                return;
            }
        }

        #endregion

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

        private bool isBlockSound = false;

        public void CreateAudioSource(Vector3 pos, SfxClipTypes clipIdx, float sound3d)
        {
            if (isBlockSound) return;

            AudioSource audioSource = Pop();
            audioSource.transform.position = pos;
            audioSource.clip = sfxClips[(int)clipIdx];
            audioSource.volume = 1f;
            audioSource.spatialBlend = sound3d;
            audioSource.Play();

            StartCoroutine(PushAfterDelay(audioSource, audioSource.clip.length));
        }

        public void BlockSound()
        {
            isBlockSound = true;
        }
        public void ReleaseSound()
        {
            isBlockSound = false;
        }


    }

}

