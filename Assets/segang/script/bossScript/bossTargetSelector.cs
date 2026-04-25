using UnityEngine;
using System.Collections.Generic;
public class bossTargetSelector : MonoBehaviour
{
    [Header("Target")]
    public bossTargetRangeController targetRange;
    public Transform target;
    public bool HasTarget()
    {
        return targetRange.playersInRange.Count > 0;
    }

    public void SelectRandomTarget()
    {
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
