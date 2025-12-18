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
        SceneManager.LoadScene("EndScene");
    }
}