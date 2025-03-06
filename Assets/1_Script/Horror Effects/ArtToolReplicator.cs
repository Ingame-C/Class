using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Class;
using UnityEngine;

/// <summary>
/// 미술 도구들을 복제하여 호러 효과를 만드는 클래스입니다.
/// </summary>
public class ArtToolReplicator : HorrorEffect
{
    #region Properties
    private EffectTypes effecttype = EffectTypes.ArtToolReplicator;
    public override EffectTypes EffectType { get => effecttype; }
    #endregion

    #region Serialized Fields
    [Header("Parents")]
    [Space]
    [SerializeField] private GameObject artToolsParent;
    [SerializeField] private GameObject replicasParent;

    [Header("Prefabs")]
    [Space]
    [SerializeField] private GameObject crayonReplica;
    [SerializeField] private GameObject coloredPenReplica;
    [SerializeField] private GameObject palletReplica;
    #endregion

    #region Unity Methods
    private void Start()
    {
        InitializeParents();
    }
    #endregion

    #region Public Methods
    /// <summary>
    /// 미술 도구 복제 효과를 활성화합니다.
    /// </summary>
    [ContextMenu("Activate")]
    public override void Activate()
    {
        StartCoroutine(makeReplicaMany());
    }
    #endregion

    #region Private Methods
    /// <summary>
    /// 부모 오브젝트들을 초기화합니다.
    /// </summary>
    private void InitializeParents()
    {
        if(artToolsParent == null)
        {
            artToolsParent = GameObject.Find(Constants.NAME_ARTTOOLSPARENT);
        }
        if(replicasParent == null)
        {
            replicasParent = GameObject.Find(Constants.NAME_REPLICASPARENT);
        }
    }

    /// <summary>
    /// 지정된 타입의 미술 도구 복제본을 생성합니다.
    /// </summary>
    /// <param name="propTypes">복제할 미술 도구의 타입</param>
    /// <returns>생성된 복제본 게임오브젝트</returns>
    private GameObject makeReplica(PropTypes propTypes)
    {
        GameObject prefab = GetReplicaPrefab(propTypes);
        return prefab != null ? Instantiate(prefab) : null;
    }

    /// <summary>
    /// 미술 도구 타입에 따른 복제본 프리팹을 반환합니다.
    /// </summary>
    private GameObject GetReplicaPrefab(PropTypes propTypes)
    {
        return propTypes switch
        {
            PropTypes.Crayons => crayonReplica,
            PropTypes.Pallet => palletReplica,
            PropTypes.ColoredPencil => coloredPenReplica,
            _ => null
        };
    }

    /// <summary>
    /// 미술 도구들을 여러 번 복제하는 코루틴입니다.
    /// </summary>
    private IEnumerator makeReplicaMany()
    {
        var artTools = GetArtTools();
        const int REPLICATION_COUNT = 5;
        const float REPLICATION_DELAY = 2f;

        for(int i = 0; i < REPLICATION_COUNT; i++)
        {
            List<GameObject> replicas = CreateReplicas(artTools);
            SetReplicasParent(replicas);
            yield return new WaitForSeconds(REPLICATION_DELAY);
        }
    }

    /// <summary>
    /// 모든 미술 도구들을 가져옵니다.
    /// </summary>
    private IEnumerable<GameObject> GetArtTools()
    {
        return Enumerable.Range(0, artToolsParent.transform.childCount)
                        .Select(i => artToolsParent.transform.GetChild(i).gameObject);
    }

    /// <summary>
    /// 미술 도구들의 복제본을 생성합니다.
    /// </summary>
    private List<GameObject> CreateReplicas(IEnumerable<GameObject> artTools)
    {
        List<GameObject> replicas = new List<GameObject>();

        foreach (var tool in artTools)
        {
            GameObject replica = CreateReplicaForTool(tool);
            if (replica != null)
            {
                replica.transform.position = tool.transform.position;
                replicas.Add(replica);
            }
        }

        return replicas;
    }

    /// <summary>
    /// 특정 미술 도구의 복제본을 생성합니다.
    /// </summary>
    private GameObject CreateReplicaForTool(GameObject tool)
    {
        if (tool.TryGetComponent(out ColoredPencil _))
        {
            return makeReplica(PropTypes.ColoredPencil);
        }
        else if (tool.TryGetComponent(out Crayons _))
        {
            return makeReplica(PropTypes.Crayons);
        }
        else if (tool.TryGetComponent(out Pallet _))
        {
            return makeReplica(PropTypes.Pallet);
        }
        return null;
    }

    /// <summary>
    /// 복제본들을 부모 오브젝트의 자식으로 설정합니다.
    /// </summary>
    private void SetReplicasParent(List<GameObject> replicas)
    {
        replicas.ForEach(replica => replica.transform.SetParent(replicasParent.transform));
    }
    #endregion
}
