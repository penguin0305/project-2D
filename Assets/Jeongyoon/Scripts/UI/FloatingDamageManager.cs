using UnityEngine;

public class FloatingDamageManager : MonoBehaviour
{
	public static FloatingDamageManager Instance { get; private set; }

	[SerializeField] private FloatingDamage prefab;

	private void Awake()
	{
		if (Instance == null)
			Instance = this;
		else
			Destroy(gameObject);
	}

	public void Show(int value, Vector3 worldPosition, FloatingDamageType type)
	{
		var popup = Instantiate(prefab, worldPosition + Vector3.up * 0.5f, Quaternion.identity);
		popup.Init(value, type);
	}
}