using UnityEngine;
using System.Collections.Generic;
using Unity.Netcode;
public class bossTargetSelector : NetworkBehaviour
{
    [Header("Target")]
    public bossTargetRangeController targetRange;
    public Transform target;
    public bool HasTarget()
    {
        if (!IsServer) return false;
        return targetRange.playersInRange.Count > 0;
    }

    public void SelectRandomTarget()
    {
        if (!IsServer) return;
        var list = targetRange.playersInRange;
        list.RemoveAll(p => p == null);

        if (list.Count == 0)
        {
            target = null;
            return;
        }

        target = list[Random.Range(0, list.Count)];
        Debug.Log(target+"À¸·Î Å¸°Ù¼³Á¤µÊ");
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
