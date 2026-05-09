using UnityEngine;
using System;
using System.Linq;
using NUnit.Framework;
using System.Collections.Generic;

public class PlayerSession : MonoBehaviour
{
    private PlayerDataManager pDataManager;
    public static PlayerSession Instance { get; private set; }

    public string Id { get; private set; }
    public string Username { get; private set; }
    public int Level { get; private set; }
    public int Exp { get; private set; }
    public int Currency { get; private set; }

    public List<PlayerItem> PlayerItems { get; private set; }

    // event for UI when player data is updated
    public event Action OnPlayerDataUpdated;

    //Singleton set
    private void Awake()
    {
        pDataManager = FindAnyObjectByType<PlayerDataManager>();
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // 서버로부터 받은 데이터를 세션에 갱신하는데 사용중
    public void UpdateSessionData(PlayerData pData)
    {
        this.Id = pData.id;
        this.Username = pData.username;
        this.Level = pData.level;
        this.Exp = pData.exp;
        this.Currency = pData.currency;
        this.PlayerItems = pData.items;

        OnPlayerDataUpdated?.Invoke();
    }
    
    
    public void UpdateItem(int eid, int level, int exp, int failCount)
    {
        foreach (var item in PlayerItems)
        {
            if (item.eid == eid)
            {
                item.enhance_level = level;
                item.dup_count = exp;
                item.enhance_fail_count = failCount;
            }
        }
        pDataManager.SaveAndFetch(() => OnPlayerDataUpdated?.Invoke());
    }

    // 메인 씬 종료 후 스테이지에서 획득한 경험치와 아이템을 세션에 갱신하는데 사용
    public void UpdateStageData(Dictionary<int, int> tmpinventory, int score)
    {
        this.Exp += score;

        var pItemMap = PlayerItems.ToDictionary(item => item.eid);
        if (tmpinventory != null)
        {
            foreach (var kvp in tmpinventory)
            {
                if (pItemMap.TryGetValue(kvp.Key, out var existingItem)) existingItem.dup_count += kvp.Value;
                else
                {
                    PlayerItems.Add(new PlayerItem
                    {
                        id = this.Id,
                        iid = 0, // 서버에서 처리
                        eid = kvp.Key,
                        type = "tmp", // 서버에서 처리
                        dup_count = kvp.Value,
                        enhance_level = 0,
                        enhance_fail_count = 0,
                        // 미사용 어트리뷰트지만 일단 0으로 초기화
                        base_atk = 0,
                        base_hp = 0,
                        base_armor = 0
                    });
                }
            }
        }
        pDataManager.SaveAndFetch(() => OnPlayerDataUpdated?.Invoke()); // 서버에서 처리하는 로직이 있으므로 저장 및 갱신 & 이벤트 알림
    }

    public EnhanceLogDto MakeLogData(CollectionState state, bool success, int level_before, int level_after)
    {
        var item = PlayerItems.FirstOrDefault(i => i.eid == state.collectionId);
        EnhanceLogDto log = new EnhanceLogDto
        {
            iid = item.iid,
            level_before = level_before,
            level_after = level_after,
            success = success
        };
        return log;
    }
}
