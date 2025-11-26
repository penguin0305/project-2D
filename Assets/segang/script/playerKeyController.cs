using UnityEngine;

public class playerKeyController : MonoBehaviour
{
    public int ownedKey;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void getKey()
    {
        ownedKey++;//먹으면 소유한 키 1개 추가
        Debug.Log("getKey");
    }
    public void removeKey()
    {
        ownedKey--;//소모시 소유한 키 1개 감소-
    }
    public int returnOwnedKey()
    {
        return ownedKey;
    }
}
