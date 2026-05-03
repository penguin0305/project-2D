using UnityEngine;
using System.Collections.Generic;
using Unity.Netcode;
public class bossTargetRangeController : NetworkBehaviour
{
    public List<Transform> playersInRange = new List<Transform>();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!IsServer) return;
        if (collision.CompareTag("Player"))
        {
            if (!playersInRange.Contains(collision.transform))
                playersInRange.Add(collision.transform);
        }
    }//플레이어와trigger충돌하면 인식리스트에 넣음

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!IsServer) return;
        if (collision.CompareTag("Player"))
        {
            playersInRange.Remove(collision.transform);
        }
    }//플레이어가 trigger밖으로 나가면 인식리스트에서 뺌
}
