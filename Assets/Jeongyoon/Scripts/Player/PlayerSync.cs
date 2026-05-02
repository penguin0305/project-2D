using Unity.Netcode;
using UnityEngine;

public class PlayerSync : NetworkBehaviour
{
	// 기존: Player.cs에 있던 isFacingLeft
	// public NetworkVariable<bool> isFacingLeft = new NetworkVariable<bool>(
	//     false,
	//     NetworkVariableReadPermission.Everyone,
	//     NetworkVariableWritePermission.Owner
	// );
	public NetworkVariable<bool> isFacingLeft = new NetworkVariable<bool>(
		false,
		NetworkVariableReadPermission.Everyone,
		NetworkVariableWritePermission.Owner
	);

	// 기존: 없었음 (PlayerAnimator에서 로컬 입력만 읽었음)
	// float moveInputX = Mathf.Abs(player.Input.Move.x);
	public NetworkVariable<float> moveX = new NetworkVariable<float>(
		0f,
		NetworkVariableReadPermission.Everyone,
		NetworkVariableWritePermission.Owner
	);

	// 기존: 없었음 (PlayerMotor.IsGrounded를 직접 읽었음)
	// animator.SetBool("IsGrounded", player.Motor.IsGrounded);
	public NetworkVariable<bool> isGrounded = new NetworkVariable<bool>(
		false,
		NetworkVariableReadPermission.Everyone,
		NetworkVariableWritePermission.Owner
	);

	// 기존: PlayerStatus.cs의 currentHealthNet
	// public NetworkVariable<int> currentHealthNet = new NetworkVariable<int>(
	//     0,
	//     NetworkVariableReadPermission.Everyone,
	//     NetworkVariableWritePermission.Server
	// );
	public NetworkVariable<int> health = new NetworkVariable<int>(
		0,
		NetworkVariableReadPermission.Everyone,
		NetworkVariableWritePermission.Server
	);

	private Player player;

	private void Awake()
	{
		player = GetComponent<Player>();
	}

	private void LateUpdate()
	{
		if (!IsOwner) return;

		float x = player.Input.Move.x;

		// 기존: Player.cs LateUpdate에서 처리
		// if (moveX > 0f) isFacingLeft.Value = false;
		// else if (moveX < 0f) isFacingLeft.Value = true;
		if (x > 0f)
			isFacingLeft.Value = false;
		else if (x < 0f)
			isFacingLeft.Value = true;

		moveX.Value = x;
		isGrounded.Value = player.Motor.IsGrounded;
	}

	// PlayerStatus.ApplyBonus에서 위임받아 클라이언트에 보너스 동기화
	[Rpc(SendTo.ClientsAndHost)]
	public void SyncBonusRpc(int maxHP, int meleeATK, int rangeATK, int armor, float speed, float critRate, float critDamage)
	{
		player.Status.ApplySyncedBonus(maxHP, meleeATK, rangeATK, armor, speed, critRate, critDamage);
	}

	// 기존: 없었음 (Owner만 로컬에서 애니메이션 실행)
	// animator.DoMeleeAttack(); → Owner 제외 모든 클라이언트에 전파
	[Rpc(SendTo.NotOwner)]
	public void MeleeAttackAnimRpc()
	{
		player.Animator.DoMeleeAttack();
	}

	// 기존: 없었음 (Owner만 로컬에서 애니메이션 실행)
	// animator.DoRangeAttack(); → Owner 제외 모든 클라이언트에 전파
	[Rpc(SendTo.NotOwner)]
	public void RangeAttackAnimRpc()
	{
		player.Animator.DoRangeAttack();
	}

	// 모든 클라이언트에 데미지 팝업 표시
	[Rpc(SendTo.ClientsAndHost)]
	public void ShowFloatingDamageRpc(int value, Vector3 position, int popupType)
	{
		if (FloatingDamageManager.Instance != null)
			FloatingDamageManager.Instance.Show(value, position, (FloatingDamageType)popupType);
	}
}