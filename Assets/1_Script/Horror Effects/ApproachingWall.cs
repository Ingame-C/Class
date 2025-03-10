using System.Collections;
using System.Collections.Generic;
using Class;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

/// <summary>
/// 벽이 플레이어를 향해 다가오는 호러 효과를 구현합니다.
/// </summary>
public class ApproachingWall : HorrorEffect
{
    #region Properties
    private EffectTypes effecttype = EffectTypes.ApproachingWall;
    public override EffectTypes EffectType { get => effecttype; }
    #endregion

    #region Serialized Fields
    [Header("Wall Settings")]
    [SerializeField] private List<GameObject> walls = new List<GameObject>();
    [SerializeField] private float speed = 3f;
    [SerializeField] private float endTime = 50f;
    #endregion

    #region Private Fields
    private GameObject wallsParent;
    private float elapsedTime;
    private readonly List<Vector3> dir = new List<Vector3> 
    {
        new Vector3(0, 0, 0),
        new Vector3(-1, 0, 0),    // Left
        new Vector3(1, 0, 0),   // Right
    };
    #endregion

    #region Unity Methods
    private void Start()
    {
        InitializeWalls();
    }
    #endregion

    #region Public Methods
    /// <summary>
    /// 벽이 다가오는 효과를 활성화합니다.
    /// </summary>
    [ContextMenu("Activate")]
    public override void Activate()
    {
        StartCoroutine(SetWallApproach());
    }
    #endregion

    #region Private Methods
    /// <summary>
    /// 벽 오브젝트들을 초기화합니다.
    /// </summary>
    private void InitializeWalls()
    {
        FindWallsParent();
        AddWallsToList();
    }

    /// <summary>
    /// 벽들의 부모 오브젝트를 찾습니다.
    /// </summary>
    private void FindWallsParent()
    {
        var initProps = GameObject.FindGameObjectsWithTag(Constants.TAG_INITPROPS);
        foreach (GameObject prop in initProps)
        {
            if (prop.name == "Walls")
            {
                wallsParent = prop;
                break;
            }
        }
    }

    /// <summary>
    /// 벽 오브젝트들을 리스트에 추가합니다.
    /// </summary>
    private void AddWallsToList()
    {
        for(int i = 0; i < 2; i++)
        {
            walls.Add(wallsParent.transform.GetChild(i).gameObject);
        }
        walls.RemoveAt(0);
    }

    /// <summary>
    /// 벽이 다가오는 효과를 실행하는 코루틴입니다.
    /// </summary>
    private IEnumerator SetWallApproach()
    {
        PlayWallSound();
        SetWallTags();
        yield return MoveWalls();
        ResetElapsedTime();
    }

    /// <summary>
    /// 벽 이동 사운드를 재생합니다.
    /// </summary>
    private void PlayWallSound()
    {
        SoundManager.Instance.CreateAudioSource(transform.position, SfxClipTypes.Wall_move, 0.0f);
    }

    /// <summary>
    /// 벽 오브젝트들의 태그를 설정합니다.
    /// </summary>
    private void SetWallTags()
    {
        for (int i = 0; i < 2; i++)
        {
            var wall = walls[i].GetComponentsInChildren<Transform>();
            foreach (var item in wall)
            {
                item.GameObject().tag = Constants.TAG_LAVAOBJECT;
            }
        }
    }

    /// <summary>
    /// 벽을 이동시키는 코루틴입니다.
    /// </summary>
    private IEnumerator MoveWalls()
    {
        while (elapsedTime < endTime)
        {
            for (int i = 0; i < 2; i++)
            {
                walls[i].transform.position += dir[i] * speed * Time.deltaTime;
            }
            yield return null;
        }
    }

    /// <summary>
    /// 경과 시간을 초기화합니다.
    /// </summary>
    private void ResetElapsedTime()
    {
        elapsedTime = 0;
    }
    #endregion
}


