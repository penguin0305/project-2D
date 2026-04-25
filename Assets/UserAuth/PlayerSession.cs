using UnityEngine;
using System;
using NUnit.Framework;
using System.Collections.Generic;

public class PlayerSession : MonoBehaviour
{
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

    public void UpdateSessionData(PlayerData pData)
    {
        this.Id = pData.id;
        this.Username = pData.username;
        this.Level = pData.level;
        this.Exp = pData.exp;
        this.Currency = pData.currency;
        this.PlayerItems = pData.playeritems;

        OnPlayerDataUpdated?.Invoke();
    }

    // temp
    public void UpdateItem(int eid, int level, int exp, int failCount)
    {
        if (PlayerItems == null)
            PlayerItems = new List<PlayerItem>();

        foreach (var item in PlayerItems)
        {
            if (item.eid == eid)
            {
                item.enhance_level = level;
                item.dup_count = exp;
                item.enhance_fail_count = failCount;

                OnPlayerDataUpdated?.Invoke();
                return;
            }
        }

        PlayerItems.Add(new PlayerItem
        {
            eid = eid,
            enhance_level = level,
            dup_count = exp,
            enhance_fail_count = failCount,
            type = "collection"
        });

        OnPlayerDataUpdated?.Invoke();
    }
}
