using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public string id;
    public string username;
    public int level;
    public int exp;
    public int currency;
    public List<PlayerItem> items;
}

[System.Serializable]
public class PlayerItem
{
    public string id; // playerID
    public int iid; // instanceID
    public int eid; // 본체
    public string type;
    public int dup_count; // 본체
    public int enhance_level;
    public int enhance_fail_count;
    public double base_atk = 0;
    public double base_hp = 0;
    public double base_armor = 0;
}

[System.Serializable]
public class EnhanceLogDto
{
    public int iid;
    public int level_before;
    public int level_after;
    public bool success;
}
