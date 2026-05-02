using System.Collections.Generic;
using UnityEngine;

public class CollectionUIManager : MonoBehaviour
{
    public static CollectionUIManager Instance;

    [Header("List")]
    public Transform content;
    public CollectionSlotUI slotPrefab;

    [Header("Detail")]
    public CollectionDetailUI detailUI;

    private Dictionary<int, CollectionState> stateDict;
    private List<int> allIds;
    private IEnhanceService enhanceService;

    void Awake()
    {
        Instance = this;

        stateDict = new Dictionary<int, CollectionState>();
        allIds = new List<int>();

        gameObject.SetActive(false);
    }

    void Start()
    {
        enhanceService = new LocalEnhanceService();

        LoadAllIdsFromDatabase();

        // Session 이벤트 구독
        if (PlayerSession.Instance != null)
            PlayerSession.Instance.OnPlayerDataUpdated += RefreshFromSession;

        // 이미 데이터 있으면 즉시 반영
        RefreshFromSession();

        detailUI.onEnhance = OnEnhance;
    }

    void OnDestroy()
    {
        if (PlayerSession.Instance != null)
            PlayerSession.Instance.OnPlayerDataUpdated -= RefreshFromSession;
    }

    public void Open()
    {
        gameObject.SetActive(true);
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }

    // =========================
    // DB에서 전체 컬렉션 ID 가져오기
    // =========================
    void LoadAllIdsFromDatabase()
    {
        allIds.Clear();

        foreach (var data in CollectionDatabase.Instance.collectionList)
        {
            allIds.Add(data.collectionId);
        }
    }

    // =========================
    // Session → State 변환
    // =========================
    void BuildStateFromSession()
    {
        stateDict.Clear();

        if (PlayerSession.Instance == null)
            return;

        var items = PlayerSession.Instance.PlayerItems;

        if (items == null)
            return;

        foreach (var item in items)
        {
            int id = item.eid;

            stateDict[id] = new CollectionState
            {
                collectionId = id,
                level = item.enhance_level,
                exp = item.dup_count,
                failCount = item.enhance_fail_count
            };
        }
    }

    // =========================
    // UI 전체 갱신
    // =========================
    void RefreshFromSession()
    {
        BuildStateFromSession();
        Populate();

        if (allIds.Count > 0)
            OnClickSlot(allIds[0]);
    }

    // =========================
    // 슬롯 생성
    // =========================
    void Populate()
    {
        foreach (Transform child in content)
            Destroy(child.gameObject);

        foreach (var id in allIds)
        {
            var slot = Instantiate(slotPrefab, content);
            slot.Init(id);
            slot.SetClickAction(OnClickSlot);
        }
    }

    // =========================
    // 상태 가져오기 (Not Owned 처리 포함)
    // =========================
    public CollectionState GetState(int id)
    {
        if (stateDict.TryGetValue(id, out var state))
            return state;

        return new CollectionState
        {
            collectionId = id,
            level = 0,
            exp = 0,
            failCount = 0
        };
    }

    // =========================
    // 슬롯 클릭
    // =========================
    void OnClickSlot(int collectionId)
    {
        var data = CollectionDatabase.Instance.Get(collectionId);
        var state = GetState(collectionId);

        detailUI.Show(data, state);
    }

    // =========================
    // 강화 버튼
    // =========================
    void OnEnhance(int collectionId)
    {
        var state = GetState(collectionId);

        enhanceService.Enhance(collectionId, state, (updatedState) =>
        {
            if (updatedState == null)
            {
                Debug.LogError("updatedState is NULL");
                return;
            }

            stateDict[collectionId] = updatedState;

            ApplyToSession(collectionId, updatedState);

            OnClickSlot(collectionId);
            Populate();
        });
    }

    // =========================
    // Session 반영
    // =========================
    void ApplyToSession(int collectionId, CollectionState state)
    {
        if (PlayerSession.Instance == null)
            return;

        PlayerSession.Instance.UpdateItem(
            collectionId,
            state.level,
            state.exp,
            state.failCount
        );
    }
}