using Class;
using UnityEngine;

/// <summary>
/// 거울이 깨지는 호러 효과를 구현하는 클래스입니다.
/// 여러 거울 컨트롤러를 관리하고 랜덤하게 거울을 깨뜨립니다.
/// </summary>
public class BreakMirrorEffect : HorrorEffect
{
    #region Properties
    public override EffectTypes EffectType => EffectTypes.MirrorBreak;
    #endregion

    #region Serialized Fields
    [Header("Mirror Controllers")]
    [SerializeField] private MirrorBreakController[] m_Controllers;
    #endregion

    #region Unity Methods
    private void Start()
    {
        InitializeControllers();
    }
    #endregion

    #region Public Methods
    /// <summary>
    /// 거울 깨짐 효과를 활성화합니다.
    /// </summary>
    [ContextMenu("Activate")]
    public override void Activate()
    {
        if (!IsValidControllers()) return;

        PlayBreakSound();
        BreakRandomMirrors();
    }
    #endregion

    #region Private Methods
    /// <summary>
    /// 거울 컨트롤러들을 초기화합니다.
    /// </summary>
    private void InitializeControllers()
    {
        const int CONTROLLER_COUNT = 5;
        for(int i = 0; i < CONTROLLER_COUNT; i++)
        {
            if (m_Controllers[i] == null)
            {
                m_Controllers[i] = FindMirrorController(i + 1);
            }
        }
    }

    /// <summary>
    /// 지정된 인덱스의 거울 컨트롤러를 찾습니다.
    /// </summary>
    private MirrorBreakController FindMirrorController(int index)
    {
        return GameObject.Find($"Demolisher Script_{index}")?.GetComponent<MirrorBreakController>();
    }

    /// <summary>
    /// 컨트롤러 배열이 유효한지 확인합니다.
    /// </summary>
    private bool IsValidControllers()
    {
        return m_Controllers.Length > 0;
    }

    /// <summary>
    /// 거울이 깨지는 소리를 재생합니다.
    /// </summary>
    private void PlayBreakSound()
    {
        SoundManager.Instance.CreateAudioSource(transform.position, SfxClipTypes.Mirror_Break, 1.0f);
    }

    /// <summary>
    /// 랜덤하게 선택된 거울들을 깨뜨립니다.
    /// </summary>
    private void BreakRandomMirrors()
    {
        const int BREAK_COUNT = 4;
        for (int i = 0; i < BREAK_COUNT; i++)
        {
            int randomIndex = Random.Range(0, m_Controllers.Length);
            m_Controllers[randomIndex].Demolish();
        }
    }
    #endregion
}