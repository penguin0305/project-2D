using UnityEngine;

public class PlayerEvents : MonoBehaviour
{
	public event System.Action OnDeath;
	public event System.Action<int> OnCheckHP;
}
