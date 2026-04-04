/*
using UnityEngine;

public class MeleeHitbox : MonoBehaviour
{
	private Player player;

	private void Awake()
	{
		player = GetComponentInParent<Player>();
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (!player.Combat.isMeleeAttacking)
			return;
		
		if (collision.CompareTag("Enemy"))
		{
			var dummy = collision.GetComponent<enemyCombat>();
			if (dummy)
				dummy.OnHit(player.Status.MeleeATK, transform);//�÷��̾� ���ݰ� �ݴ�������� �˹�
		}
	}
}
*/