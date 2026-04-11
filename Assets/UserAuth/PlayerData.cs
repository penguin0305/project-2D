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
    public string id;
    public int iid; // 인스턴스아이디
    public int eid;
    public string type;
    public int dup_count;
    public int enhance_level;
    public int enhance_fail_count;
    public double base_atk;
    public double base_hp;
    public double base_armor;
}
