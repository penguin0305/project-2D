using UnityEngine;

public class treasureBoxController : MonoBehaviour
{
    private bool isOpened = false;
    public GameObject player;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (isOpened) return;

        // 플레이어가 트리거 안에 있고 F키를 누르면
        if (collision.CompareTag("Player") && Input.GetKeyDown(KeyCode.F)&& player.GetComponent<playerKeyController>().returnOwnedKey()>0)
        {
            player.GetComponent<playerKeyController>().removeKey();
            OpenChest();
        }
    }

    void OpenChest()
    {
        isOpened = true;
        Debug.Log("상자 열림!");
        GetComponent<itemDropController>().DropItems();
        Destroy(this.gameObject);
        // 열리는 애니메이션, 스프라이트 변경, 아이템 드롭 등 추가 가능
    }
}
