using UnityEngine;
using TMPro;
using System.Collections;
using Unity.Netcode;

public class NetworkPlayerUI : MonoBehaviour
{
    public static NetworkPlayerUI Instance;

    public TextMeshProUGUI HP;
    //public TextMeshProUGUI skillText;스킬추가

    //private PlayerSkill playerSkill;스킬추가

    [SerializeField] private TextMeshProUGUI timerText;

    private Player player;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    private void Update()
    {
        // 스폰 이후 한 번만 찾아서 콜백 등록
        if (player == null)
        {
            foreach (var p in FindObjectsByType<Player>(FindObjectsSortMode.None))
            {
                if (p.IsOwner)
                {
                    player = p;
                    player.Sync.health.OnValueChanged += OnChangeHealth;
                    UpdateHP(player.Sync.health.Value);
                    break;
                }
            }
        }

        if (timerText != null && NetworkHistoryManager.Instance != null)
        {
            UpdateTimerUI(NetworkHistoryManager.Instance.playTime.Value);
        }
    }

    private void OnChangeHealth(int oldValue, int newValue)
    {
        UpdateHP(newValue);
    }

    private void UpdateTimerUI(float Seconds)
    {
        int minutes = Mathf.FloorToInt(Seconds / 60);
        int seconds = Mathf.FloorToInt(Seconds % 60);

        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void UpdateHP(int currentHp)
    {
        HP.text = $"{currentHp}";
    }

    /*-스킬추가
    void UpdateSkillUI(int currentSkill)
    {

        skillText.text = $"{currentSkill}";
    }
    */

    private void OnDestroy()
    {
        if (player != null)
        {
            player.Sync.health.OnValueChanged -= OnChangeHealth;
        }
    }
}