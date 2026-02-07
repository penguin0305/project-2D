using UnityEngine;

public class PlayerOverlapSensor : MonoBehaviour
{
	public bool IsOnLadder
	{
		get;
		private set;
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		CheckOverlap(collision, true);
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		CheckOverlap(collision, false);
	}

	private void CheckOverlap(Collider2D collision, bool state)
	{
		if (collision.TryGetComponent<Ladder>(out _))
			IsOnLadder = state;
	}
}
