using UnityEngine;

public class MeleeHitbox : MonoBehaviour
{
	private PlayerCombat combat;

	private void Awake()
	{
		combat = GetComponentInParent<PlayerCombat>();
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (!combat.isMeleeAttacking)
			return;
		
		if (collision.CompareTag("Enemy"))
		{
			var dummy = collision.GetComponent<enemyCombat>();
			if (dummy)
				dummy.OnHit(5);
		}
	}
}
