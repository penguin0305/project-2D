using TMPro;
using UnityEngine;

public class PlayerHealthDisplay : MonoBehaviour
{
	[SerializeField] private TextMeshProUGUI healthText;

	private Player player;

	private void Awake()
	{
		player = GetComponentInParent<Player>();
	}

	private void OnEnable()
	{
		player.Sync.health.OnValueChanged += OnHealthChanged;
		UpdateText(player.Sync.health.Value);
	}

	private void OnDisable()
	{
		player.Sync.health.OnValueChanged -= OnHealthChanged;
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
		if (Camera.main != null)
			transform.forward = Camera.main.transform.forward;
	}

	public void ForceSync()
	{
		UpdateText(player.Sync.health.Value);
	}
}