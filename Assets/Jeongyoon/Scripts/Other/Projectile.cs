using UnityEngine;

public class Projectile : MonoBehaviour
{
	[SerializeField] private float speed = 8f;
	[SerializeField] private float lifetime = 2f;
	[SerializeField] private float visualOffset = -45f;
	private int damage;
	private Vector2 direction;
	public void Setup(int damage, Vector2 direction)
	{
		this.damage = damage;
		this.direction = direction;

		float angle = Mathf.Atan2(this.direction.y, this.direction.x) * Mathf.Rad2Deg;
		transform.rotation = Quaternion.Euler(0, 0, angle + visualOffset);

		Destroy(gameObject, lifetime);
	}

	void Update()
	{
		transform.Translate(Vector2.right * speed * Time.deltaTime, Space.World);
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag("Enemy"))
		{
			var dummy = collision.GetComponent<enemyCombat>();
			if (dummy)
			{
				dummy.OnHit(damage, transform);
				Destroy(gameObject);
			}
		}
	}
}
