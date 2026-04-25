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

     private PlayerStatus localPlayerStatus;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }


    private void Update()
    {
        if (timerText != null && NetworkHistoryManager.Instance != null)
        {
            UpdateTimerUI(NetworkHistoryManager.Instance.playTime.Value);
        }
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
        if (localPlayerStatus != null)
        {
            localPlayerStatus.currentHealthNet.OnValueChanged -= (oldValue, newValue) => { UpdateHP(newValue); };
        }
    }}