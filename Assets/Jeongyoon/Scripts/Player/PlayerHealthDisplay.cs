using TMPro;
using UnityEngine;

public class PlayerHealthDisplay : MonoBehaviour
{
	[SerializeField] private TextMeshPro healthText;
	[SerializeField] private Vector3 offset = new Vector3(0f, 1.5f, 0f);
	[SerializeField] private float fontSize = 0.5f;

	private Player player;

	private void Awake()
	{
		player = GetComponentInParent<Player>();

		if (healthText != null)
		{
			healthText.fontSize = fontSize;
			healthText.alignment = TextAlignmentOptions.Center;
		}
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
		// 플레이어 머리 위에 고정
		transform.position = player.transform.position + offset;

		// 항상 카메라를 향하도록
		if (Camera.main != null)
			transform.forward = Camera.main.transform.forward;
	}

	public void ForceSync()
	{
		UpdateText(player.Sync.health.Value);
	}
}