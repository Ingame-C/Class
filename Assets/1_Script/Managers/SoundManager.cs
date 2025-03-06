using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

namespace Class {

    /// <summary>
    /// 게임의 사운드를 관리하는 매니저 클래스입니다.
    /// 오디오 소스의 생성, 재생, 풀링을 담당합니다.
    /// </summary>
    public class SoundManager : MonoBehaviour
    {
        #region Singleton
        private static SoundManager instance;
        public static SoundManager Instance { get { return instance; } }

        private void InitializeSingleton()
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

        private void InitializePool(int count = 10)
        {
            poolRoot = new GameObject { name = "_poolRoot" }.transform;
            poolRoot.parent = this.transform;

            for (int i = 0; i < count; i++)
            {
                audioSources.Push(CreateAudioSource());
            }
        }

        private AudioSource CreateAudioSource()
        {
            GameObject go = Instantiate(audioPrefab);
            go.name = audioPrefab.name;
            go.transform.parent = poolRoot;
            go.gameObject.SetActive(false);
            return go.GetComponent<AudioSource>();
        }

        private void PushToPool(AudioSource source)
        {
            source.gameObject.SetActive(false);
            audioSources.Push(source);
        }

        private AudioSource PopFromPool()
        {
            AudioSource source;
            if (audioSources.Count == 0) 
                source = CreateAudioSource();
            else 
                source = audioSources.Pop();

            source.gameObject.SetActive(true);
            return source;
        }

        private IEnumerator PushToPoolAfterDelay(AudioSource source, float delay)
        {
            yield return new WaitForSeconds(delay);
            PushToPool(source);
        }
        #endregion

        #region Unity Methods
        private void Awake()
        {
            InitializeSingleton();
            InitializePool();
        }
        #endregion

        #region Serialized Fields
        [Header("GameObjects")]
        [SerializeField] private GameObject audioPrefab;
        
        [Header("AudioClips")]
        [SerializeField] public AudioClip[] sfxClips;
        #endregion

        #region Private Fields
        private bool isBlockSound = false;
        #endregion

        #region Public Methods
        /// <summary>
        /// 지정된 위치에 3D 사운드를 생성하고 재생합니다.
        /// </summary>
        /// <param name="position">사운드가 재생될 위치</param>
        /// <param name="clipType">재생할 사운드 클립의 타입</param>
        /// <param name="spatialBlend">3D 사운드의 공간감 (0: 2D, 1: 3D)</param>
        /// <param name="volume">사운드의 볼륨 (기본값: 1)</param>
        public void CreateAudioSource(Vector3 position, SfxClipTypes clipType, float spatialBlend, float volume = 1f)
        {
            if (isBlockSound) return;

            AudioSource audioSource = PopFromPool();
            ConfigureAudioSource(audioSource, position, clipType, spatialBlend, volume);
            PlayAndRecycleAudioSource(audioSource);
        }

        /// <summary>
        /// 모든 사운드를 차단합니다.
        /// </summary>
        public void BlockSound()
        {
            isBlockSound = true;
        }

        /// <summary>
        /// 사운드 차단을 해제합니다.
        /// </summary>
        public void ReleaseSound()
        {
            isBlockSound = false;
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// 오디오 소스를 설정합니다.
        /// </summary>
        private void ConfigureAudioSource(AudioSource source, Vector3 position, SfxClipTypes clipType, float spatialBlend, float volume)
        {
            source.transform.position = position;
            source.clip = sfxClips[(int)clipType];
            source.volume = volume;
            source.spatialBlend = spatialBlend;
        }

        /// <summary>
        /// 오디오 소스를 재생하고 재사용을 위해 풀로 반환합니다.
        /// </summary>
        private void PlayAndRecycleAudioSource(AudioSource source)
        {
            source.Play();
            StartCoroutine(PushToPoolAfterDelay(source, source.clip.length));
        }
        #endregion
    }
}

