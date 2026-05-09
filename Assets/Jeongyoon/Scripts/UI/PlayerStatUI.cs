using UnityEngine;
using TMPro;

public class PlayerStatUI : MonoBehaviour
{
    [Header("Text Fields")]
    [SerializeField] private TextMeshProUGUI hpText;
    [SerializeField] private TextMeshProUGUI meleeATKText;
    [SerializeField] private TextMeshProUGUI rangeATKText;
    [SerializeField] private TextMeshProUGUI armorText;
    [SerializeField] private TextMeshProUGUI speedText;
    [SerializeField] private TextMeshProUGUI critRateText;
    [SerializeField] private TextMeshProUGUI critDamageText;

    [Header("Panel")]
    [SerializeField] private GameObject panel;

    private Player player;

    private void Awake()
    {
        panel.SetActive(false);
    }

    private void Update()
    {
        if (player == null)
        {
            foreach (var p in FindObjectsByType<Player>(FindObjectsSortMode.None))
            {
                if (p.IsOwner)
                {
                    player = p;
                    break;
                }
            }
        }

        if (player == null)
            return;

        if (player.Input.ConsumeToggleStatPressed())
            Toggle();

        if (panel.activeSelf)
            RefreshStats();
    }

    private void RefreshStats()
    {
        var s = player.Status;

        hpText.text = $"HP: {s.CurrentHealth} / {s.MaxHealth}";
        meleeATKText.text = $"MeleeATK: {s.MeleeATK}";
        rangeATKText.text = $"RangeATK: {s.RangeATK}";
        armorText.text = $"Armor: {s.Armor}";
        speedText.text = $"Speed: {s.Speed}";
        critRateText.text = $"CritRate: {s.CritRate * 100f:F1}%";
        critDamageText.text = $"CritDamage: {s.CritDamage * 100f:F0}%";
    }

    public void Toggle()
    {
        panel.SetActive(!panel.activeSelf);
    }
}