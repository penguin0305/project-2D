using UnityEngine;
using System.Collections.Generic;
using NUnit.Framework;

[System.Serializable]
public class PlayerData
{
    public string id;
    public string username;
    public int level;
    public int exp;
    public int currency;
    public List<PlayerItem> playeritems;
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
    public double base_atk;
    public double base_hp;
    public double base_armor;
}
