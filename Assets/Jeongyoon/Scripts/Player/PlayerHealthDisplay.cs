using TMPro;
using UnityEngine;

public class PlayerHealthDisplay : MonoBehaviour
{
	[SerializeField] private TextMeshProUGUI healthText;

	private Player player;

	private void Awake()
	{
		// NetworkHealthDisplay: networkPlayer = GetComponentInParent<NetworkPlayer>();
		player = GetComponentInParent<Player>();
	}

	private void OnEnable()
	{
		// NetworkHealthDisplay: networkPlayer.CurrentHealth.OnValueChanged += OnHealthChanged;
		player.NetworkHealth.OnValueChanged += OnHealthChanged;
		UpdateText(player.NetworkHealth.Value);
	}

	private void OnDisable()
	{
		// NetworkHealthDisplay: networkPlayer.CurrentHealth.OnValueChanged -= OnHealthChanged;
		player.NetworkHealth.OnValueChanged -= OnHealthChanged;
	}

	private void OnHealthChanged(int oldValue, int newValue)
	{
		UpdateText(newValue);
	}

	private void UpdateText(int hp)
	{
		if (healthText != null)
			healthText.text = $"HP {hp}";
	}

	private void LateUpdate()
	{
		// 항상 카메라를 향하도록
		if (Camera.main != null)
			transform.forward = Camera.main.transform.forward;
	}

	public void ForceSync()
	{
		UpdateText(player.NetworkHealth.Value);
	}
}
