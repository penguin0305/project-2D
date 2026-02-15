using UnityEngine;

public class scoreItemController : MonoBehaviour
{
    public int itemScore;
    public itemData data;
    GameObject stageManager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        stageManager = GameObject.FindWithTag("StageManager");
        data.itemQuantity = 0;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.CompareTag("Player"))
        {
            stageManager.GetComponent<StageManager>().GetItem(data);
            //data.itemQuantity++;//>>나중에 데이터를 복사본으로 받아서 따로 관리 필요
            //Debug.Log(data.itemQuantity);

            //inventory UI 추가
            InventoryManager.Instance.AddItem(data);
            
            Destroy(gameObject);
        }
    }
}
