using UnityEngine;
using UnityEngine.SceneManagement;

public class HistoryManager : MonoBehaviour
{
    public static HistoryManager Instance;

    public float playTime;
    public int DefeatCount;
    public int currentHP;
    public int coinCount = 0;
    public int tempcoinCount = 0;
    public bool isGameActive = true;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (isGameActive)
        {
            playTime += Time.deltaTime;
        }
    }

    public void AddDefeatCount()
    {
        DefeatCount++;
    }

    public void UpdateHP(int hp)
    {
        currentHP = hp;
    }

    public void AddcoinCount()
    {
        coinCount++;
        tempcoinCount++;
    }

    public void GameClear()
    {
        isGameActive = false;
    }

    public void ResetData()
    {
        playTime = 0f;
        DefeatCount = 0;
        currentHP = 20;
        coinCount = 0;
        tempcoinCount = 0;
        isGameActive = true;
        Debug.Log("History Data Reset");
    }
}