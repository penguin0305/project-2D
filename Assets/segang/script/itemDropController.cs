using System.Collections.Generic;
using UnityEngine;

public class itemDropController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    [System.Serializable]
    public class DropItem
    {
        public GameObject itemPrefab;  // 드롭될 아이템 프리팹
        public float weight;           // 가중치
    }

    [Header("드롭될 아이템 리스트")]
    public List<DropItem> dropItems = new List<DropItem>();

    [Header("드롭 개수 설정")]
    public int minDropCount = 1;       // 최소 드롭 개수
    public int maxDropCount = 3;       // 최대 드롭 개수

    public void DropItems()
    {
        if (dropItems.Count == 0)
            return;

        int dropCount = Random.Range(minDropCount, maxDropCount + 1);//아이템 드랍개수 난수 설정

        for (int i = 0; i < dropCount; i++)
        {
            GameObject selected = GetWeightedRandomItem();

            if (selected != null)
            {
                // 약간의 랜덤 오프셋을 줘서 겹치지 않게 함
                Vector3 offset = new Vector3(
                    Random.Range(-0.5f, 0.5f),
                    Random.Range(0.0f, 0.8f),
                    0
                );

                Instantiate(selected, transform.position + offset, Quaternion.identity);
            }
        }
    }

    private GameObject GetWeightedRandomItem()
    {
        float totalWeight = 0f;

        foreach (var drop in dropItems)
            totalWeight += drop.weight;

        float randomValue = Random.Range(0, totalWeight);

        foreach (var drop in dropItems)
        {
            if (randomValue < drop.weight)
                return drop.itemPrefab;

            randomValue -= drop.weight;//랜덤 드랍될 아이템 1개 선별
        }

        return null;
    }//아이템 드랍 가중치 함수
}
