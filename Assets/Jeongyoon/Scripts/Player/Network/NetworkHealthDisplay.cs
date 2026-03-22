using TMPro;
using Unity.Netcode;
using UnityEngine;

public class NetworkHealthDisplay : MonoBehaviour
{
	[SerializeField] private TextMeshProUGUI healthText;

	private NetworkPlayer networkPlayer;

	private void Awake()
	{
		networkPlayer = GetComponentInParent<NetworkPlayer>();
	}

	private void OnEnable()
	{
		networkPlayer.CurrentHealth.OnValueChanged += OnHealthChanged;
		UpdateText(networkPlayer.CurrentHealth.Value);
	}

	private void OnDisable()
	{
		networkPlayer.CurrentHealth.OnValueChanged -= OnHealthChanged;
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
}
