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

        private void Awake()
        {
            Init();
        }

        [Header("GameObjects")]
        [SerializeField] private GameObject audioPrefab;
        
        
        [Header("AudioClips")]
        [SerializeField] public AudioClip[] sfxClips;

        // TODO : Object pooling : Sound Prefab
        public void CreateAudioSource(Vector3 pos, SfxClipTypes clipIdx)
        {
            GameObject go = Instantiate(audioPrefab, pos, Quaternion.identity, transform);
            go.GetComponent<AudioSource>().clip = sfxClips[(int)clipIdx];
            go.GetComponent<AudioSource>().volume = 1f;
            go.GetComponent<AudioSource>().Play();

            Destroy(go, sfxClips[(int)clipIdx].length);

        }


    }

}

