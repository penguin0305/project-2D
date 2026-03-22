using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class NetworkPlayer : NetworkBehaviour
{
	private SpriteRenderer spriteRenderer;
	private PlayerInputState inputState;

	private NetworkVariable<bool> isFacingLeft = new NetworkVariable<bool>(
		false,
		NetworkVariableReadPermission.Everyone,
		NetworkVariableWritePermission.Owner
	);

	public NetworkVariable<int> CurrentHealth = new NetworkVariable<int>(
		20,
		NetworkVariableReadPermission.Everyone,
		NetworkVariableWritePermission.Server
	);

	private void Awake()
	{
		spriteRenderer = GetComponent<SpriteRenderer>();
		inputState = GetComponent<PlayerInputState>();
	}

	public override void OnNetworkSpawn()
	{
		isFacingLeft.OnValueChanged += OnFacingChanged;

		if (!IsOwner)
		{
			// 입력 차단
			var playerInput = GetComponent<PlayerInput>();
			if (playerInput != null)
				playerInput.enabled = false;

			// 카메라 / 오디오 비활성화 (비-오너에게는 불필요)
			foreach (var cam in GetComponentsInChildren<Camera>())
				cam.enabled = false;

			foreach (var listener in GetComponentsInChildren<AudioListener>())
				listener.enabled = false;
		}
	}

	public override void OnNetworkDespawn()
	{
		isFacingLeft.OnValueChanged -= OnFacingChanged;
	}

	private void LateUpdate()
	{
		if (!IsOwner) return;

		float moveX = inputState.Move.x;
		if (moveX > 0f)
			isFacingLeft.Value = false;
		else if (moveX < 0f)
			isFacingLeft.Value = true;
	}

	private void OnFacingChanged(bool oldValue, bool newValue)
	{
		if (spriteRenderer != null)
			spriteRenderer.flipX = newValue;
	}

	[ServerRpc(RequireOwnership = false)]
	public void TakeDamageServerRpc(int damage, float stunDuration, bool knockback)
	{
		var player = GetComponent<Player>();
		try { player.ApplyDamage(damage, stunDuration, knockback); }
		catch { }
		CurrentHealth.Value = player.Status.CurrentHealth;
		DamageAnimClientRpc();

		if (player.Status.CurrentHealth <= 0)
			SetDeadClientRpc();
	}

	[ClientRpc]
	private void DamageAnimClientRpc()
	{
		GetComponent<Player>().Animator?.DoDamageAnim();
	}

	[ClientRpc]
	private void SetDeadClientRpc()
	{
		var player = GetComponent<Player>();
		player.Status.ChangeHealth(-player.Status.CurrentHealth); // 로컬 체력도 0으로
		player.ChangeState(player.Dead);
	}
}
