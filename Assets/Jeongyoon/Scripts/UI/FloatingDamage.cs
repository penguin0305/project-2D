using UnityEngine;
using TMPro;

public enum FloatingDamageType
{
	Normal,
	Crit,
	Heal
}

public class FloatingDamage : MonoBehaviour
{
	[SerializeField] private TextMeshPro text;

	private float lifetime = 1f;
	private float elapsed = 0f;
	private Vector3 moveDir;
	private Color startColor;

	public void Init(int value, FloatingDamageType type)
	{
		switch (type)
		{
			case FloatingDamageType.Normal:
				text.color = Color.white;
				text.fontSize = 4f;
				text.text = value.ToString();
				moveDir = new Vector3(Random.Range(-0.3f, 0.3f), 1f, 0f);
				break;

			case FloatingDamageType.Crit:
				text.color = Color.red;
				text.fontSize = 6f;
				text.text = $"{value}";
				moveDir = new Vector3(Random.Range(-0.3f, 0.3f), 2f, 0f);
				break;

			case FloatingDamageType.Heal:
				text.color = Color.green;
				text.fontSize = 4f;
				text.text = $"+{value}";
				moveDir = new Vector3(Random.Range(-0.3f, 0.3f), 1f, 0f);
				break;
		}

		startColor = text.color;
		elapsed = 0f;
	}

	private void Update()
	{
		elapsed += Time.deltaTime;

		// 위로 떠오르기 (시간이 지날수록 느려짐)
		transform.position += moveDir * Time.deltaTime * (1f - elapsed / lifetime);

		// 페이드아웃
		float alpha = Mathf.Lerp(1f, 0f, elapsed / lifetime);
		text.color = new Color(startColor.r, startColor.g, startColor.b, alpha);

		if (elapsed >= lifetime)
			Destroy(gameObject);
	}
}