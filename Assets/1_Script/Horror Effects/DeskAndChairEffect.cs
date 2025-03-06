using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Class;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.Experimental.AssetDatabaseExperimental.AssetDatabaseCounters;

/// <summary>
/// 책상과 의자가 커지는 호러 효과를 구현하는 클래스입니다.
/// 선택된 책상과 의자의 크기를 점진적으로 키우고 복제본에 물리 효과를 적용합니다.
/// </summary>
public class DeskAndChairEffect : HorrorEffect
{
    #region Properties
    private EffectTypes effecttype = EffectTypes.DeskAndChairEffect;
    public override EffectTypes EffectType { get => effecttype; }
    #endregion

    #region Serialized Fields
    [Header("Parent References")]
    [SerializeField] private GameObject desksParent;
    [SerializeField] private GameObject chairsParent;

    [Header("Target Settings")]
    [SerializeField] private List<Desk> deskTargeted = new List<Desk>();
    [SerializeField] private List<Chair> chairTargeted = new List<Chair>();
    [SerializeField] private int aNumberOfProps = 3;

    [Header("Enlargement Settings")]
    [SerializeField] private float duration = 3.0f;
    [SerializeField] private Vector3 targetSize;
    #endregion

    #region Private Fields
    private Desk[] desks;
    private Chair[] chairs;
    private GameObject replicasParent;
    private Vector3 originalSize = Vector3.one;
    private float elapsedTime;
    #endregion

    #region Unity Methods
    private void Start()
    {
        InitializeParents();
        InitializeArrays();
        SelectTargetProps();
    }
    #endregion

    #region Public Methods
    /// <summary>
    /// 책상과 의자 효과를 활성화합니다.
    /// </summary>
    [ContextMenu("Activate")]
    public override void Activate()
    {
        StartCoroutine(EnlargementObject());
        ApplyPhysicsToReplicas();
    }
    #endregion

    #region Private Methods
    /// <summary>
    /// 부모 오브젝트들을 초기화합니다.
    /// </summary>
    private void InitializeParents()
    {
        var initProps = GameObject.FindGameObjectsWithTag("InitProps");
        foreach(GameObject obj in initProps)
        {
            if(obj.name == "Desks")
            {
                desksParent = obj;
            }
            else if(obj.name == "Chairs")
            {
                chairsParent = obj;
            }
        }

        if (replicasParent == null)
        {
            replicasParent = GameObject.Find(Constants.NAME_REPLICASPARENT);
        }
    }

    /// <summary>
    /// 책상과 의자 배열을 초기화합니다.
    /// </summary>
    private void InitializeArrays()
    {
        desks = desksParent.GetComponentsInChildren<Desk>();
        chairs = chairsParent.GetComponentsInChildren<Chair>();
    }

    /// <summary>
    /// 효과의 대상이 될 책상과 의자를 선택합니다.
    /// </summary>
    private void SelectTargetProps()
    {
        for(int i = 0; i < aNumberOfProps; i++)
        {
            int randomIndex = Random.Range(0, 20);
            deskTargeted.Add(desks[randomIndex]);
            chairTargeted.Add(chairs[randomIndex]);
        }
    }

    /// <summary>
    /// 복제본들에 물리 효과를 적용합니다.
    /// </summary>
    private void ApplyPhysicsToReplicas()
    {
        var replicas = replicasParent.GetComponentsInChildren<Rigidbody>();
        foreach (var item in replicas)
        {
            item.AddForce(Vector3.up * 3f);
        }
    }

    /// <summary>
    /// 선택된 책상과 의자의 크기를 점진적으로 키우는 코루틴입니다.
    /// </summary>
    private IEnumerator EnlargementObject()
    {
        PlayEnlargementSound();
        yield return EnlargeObjects();
        SetFinalSize();
    }

    /// <summary>
    /// 크기 변화 사운드를 재생합니다.
    /// </summary>
    private void PlayEnlargementSound()
    {
        SoundManager.Instance.CreateAudioSource(transform.position, SfxClipTypes.Change_chair_size, 1.0f);
    }

    /// <summary>
    /// 오브젝트들의 크기를 점진적으로 키웁니다.
    /// </summary>
    private IEnumerator EnlargeObjects()
    {
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;
            
            EnlargeChairs(t);
            EnlargeDesks(t);
            
            yield return null;
        }
    }

    /// <summary>
    /// 의자들의 크기를 점진적으로 키웁니다.
    /// </summary>
    private void EnlargeChairs(float t)
    {
        foreach (Chair chair in chairTargeted)
        {
            chair.transform.localScale = Vector3.Lerp(originalSize, targetSize, t);
        }
    }

    /// <summary>
    /// 책상들의 크기를 점진적으로 키웁니다.
    /// </summary>
    private void EnlargeDesks(float t)
    {
        foreach(Desk desk in deskTargeted)
        {
            desk.transform.localScale = Vector3.Lerp(originalSize, targetSize, t);
        }
    }

    /// <summary>
    /// 오브젝트들의 크기를 최종 크기로 설정합니다.
    /// </summary>
    private void SetFinalSize()
    {
        foreach (Chair chair in chairTargeted)
        {
            chair.transform.localScale = targetSize;
        }
        foreach (Desk desk in deskTargeted)
        {
            desk.transform.localScale = targetSize;
        }
    }
    #endregion
}
