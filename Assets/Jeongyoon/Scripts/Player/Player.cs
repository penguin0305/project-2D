// using UnityEngine;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

// public sealed class Player : MonoBehaviour
public sealed class Player : NetworkBehaviour
{
	[Header("Modules")]
	public PlayerInputState Input { get; private set; }
	public PlayerMotor Motor { get; private set; }
	public PlayerStatus Status { get; private set; }
	public PlayerOverlapSensor OverlapSensor { get; private set; }
	public PlayerCombat Combat { get; private set; }
	public PlayerInteraction Interaction { get; private set; }
	public PlayerAudio Audio { get; private set; }
	public PlayerAnimator Animator { get; private set; }
	public PlayerHealthDisplay HpDisplay { get; private set; }

	[Header("States")]
	private IPlayerState currentState;

	public IPlayerState Grounded { get; private set; }
	public IPlayerState Airborne { get; private set; }
	public IPlayerState Climb { get; private set; }
	public IPlayerState Stunned { get; private set; }
	public IPlayerState Dead { get; private set; }

	public event System.Action OnDeath;
	public event System.Action<int> OnCheckHP;

	// --- 추가: 네트워크 변수 ---
	public NetworkVariable<bool> isFacingLeft = new NetworkVariable<bool>(
		false,
		NetworkVariableReadPermission.Everyone,
		NetworkVariableWritePermission.Owner
	);

	public NetworkVariable<int> NetworkHealth = new NetworkVariable<int>(
		0,
		NetworkVariableReadPermission.Everyone,
		NetworkVariableWritePermission.Server
	);
	// ---

	private void Reset()
	{
		Input = GetComponent<PlayerInputState>();
		Motor = GetComponent<PlayerMotor>();
		Status = GetComponent<PlayerStatus>();
		OverlapSensor = GetComponent<PlayerOverlapSensor>();
		Combat = GetComponent<PlayerCombat>();
		Interaction = GetComponentInChildren<PlayerInteraction>();
		Audio = GetComponent<PlayerAudio>();
		Animator = GetComponent<PlayerAnimator>();
		HpDisplay = GetComponentInChildren<PlayerHealthDisplay>();
	}

	private void Awake()
	{
		InitializeModules();
		InitializeStates();
	}

	private void Start()
	{
		OnCheckHP?.Invoke(Status.CurrentHealth);
		ChangeState(Grounded);

		// 추가: NetworkHealth 초기값을 실제 HP와 동기화 (서버만 쓸 수 있으므로)
		if (IsServer)
			NetworkHealth.Value = Status.CurrentHealth;
	}

	// 추가: 비소유자 입력/카메라/오디오 비활성화, NetworkVariable 콜백 등록
	public override void OnNetworkSpawn()
    {
        if (!IsOwner)
        {
            var playerInput = GetComponent<PlayerInput>();
            if (playerInput != null)
                playerInput.enabled = false;

            foreach (var cam in GetComponentsInChildren<Camera>())
                cam.enabled = false;

            foreach (var listener in GetComponentsInChildren<AudioListener>())
                listener.enabled = false;
        }
        //HpDisplay.ForceSync();
    }

    public override void OnNetworkDespawn()
	{
		
	}

	private void Update()
	{
		currentState?.Tick(this);
	}

	// 추가: Owner만 방향 NetworkVariable 갱신
	private void LateUpdate()
	{
		if (!IsOwner) return;

		float moveX = Input.Move.x;
		if (moveX > 0f)
			isFacingLeft.Value = false;
		else if (moveX < 0f)
			isFacingLeft.Value = true;
	}

	private void FixedUpdate()
	{
		Motor.DetectGrounded();
		currentState?.FixedTick(this);
	}

	// 추가: 외부 진입점 — 서버면 바로 처리, 클라이언트면 ServerRpc 경유
	public void TakeDamage(int damage, float stunDuration, bool knockback)
	{
		if (IsServer)
			ApplyDamage(damage, stunDuration, knockback);
		else
			TakeDamageServerRpc(damage, stunDuration, knockback);
	}

	// 추가: 누구든 서버에 데미지 요청 가능
	[Rpc(SendTo.Server, InvokePermission = RpcInvokePermission.Everyone)]
	public void TakeDamageServerRpc(int damage, float stunDuration, bool knockback)
	{
		ApplyDamage(damage, stunDuration, knockback);
	}

	// public void ApplyDamage(int damage, float stunDuration, bool knockback)
	private void ApplyDamage(int damage, float stunDuration, bool knockback)
	{
		if (currentState == Dead || currentState == Stunned)
			return;

		Status.ChangeHealth(-damage);
		NetworkHealth.Value = Status.CurrentHealth; // 추가: NetworkVariable 갱신
		OnCheckHP?.Invoke(Status.CurrentHealth);
		HistoryManager.Instance?.UpdateHP(Status.CurrentHealth);

		// Animator.DoDamageAnim();
		DamageAnimClientRpc(); // 변경: 모든 클라이언트에 애니메이션 전파

		if (Status.CurrentHealth <= 0)
		{
			// ChangeState(Dead);
			// OnDeath?.Invoke();
			OnDeath?.Invoke();
			SetDeadClientRpc(); // 변경: 모든 클라이언트에 사망 전파
			return;
		}

		if (stunDuration > 0f)
		{
			// ((PlayerStunnedState)Stunned).SetDuration(stunDuration);
			// ChangeState(Stunned);
			SetStunnedClientRpc(stunDuration); // 변경: 모든 클라이언트에 스턴 전파
		}

		if (knockback)
		{
			// float dir;
			// dir = Motor.IsFacingRight ? -1f : 1f;
			// Motor.ApplyImpulse(new Vector2(dir * 4f, 2f));
			float dir = Motor.IsFacingRight ? -1f : 1f;
			KnockbackClientRpc(dir); // 변경: 모든 클라이언트에 넉백 전파
		}
	}

	// 추가
	// [ClientRpc]
	[Rpc(SendTo.ClientsAndHost)]
	private void DamageAnimClientRpc()
	{
		Animator?.DoDamageAnim();
	}

	// 추가
	// [ClientRpc]
	[Rpc(SendTo.ClientsAndHost)]
	private void SetDeadClientRpc()
	{
		Status.ChangeHealth(-Status.CurrentHealth);
		ChangeState(Dead);
	}

	// 추가
	// [ClientRpc]
	[Rpc(SendTo.ClientsAndHost)]
	private void SetStunnedClientRpc(float stunDuration)
	{
		((PlayerStunnedState)Stunned).SetDuration(stunDuration);
		ChangeState(Stunned);
	}

	// 추가
	// [ClientRpc]
	[Rpc(SendTo.ClientsAndHost)]
	private void KnockbackClientRpc(float dir)
	{
		Motor.ApplyImpulse(new Vector2(dir * 4f, 2f));
	}

	public void ChangeState(IPlayerState newState)
	{
		if (newState == null || currentState == newState)
			return;

		Debug.Log($"State Changed: {currentState?.GetType().Name} -> {newState.GetType().Name}");
		currentState?.Exit(this);
		currentState = newState;
		currentState.Enter(this);
	}

	private void InitializeModules()
	{
		if (!Input)
			Input = GetComponent<PlayerInputState>();
		if (!Motor)
			Motor = GetComponent<PlayerMotor>();
		if (!Status)
			Status = GetComponent<PlayerStatus>();
		if (!OverlapSensor)
			OverlapSensor = GetComponent<PlayerOverlapSensor>();
		if (!Combat)
			Combat = GetComponent<PlayerCombat>();
		if (!Interaction)
			Interaction = GetComponentInChildren<PlayerInteraction>();
		if (!Audio)
			Audio = GetComponent<PlayerAudio>();
		if (!Animator)
			Animator = GetComponent<PlayerAnimator>();
		if (!HpDisplay)
			HpDisplay = GetComponentInChildren<PlayerHealthDisplay>();
	}

	private void InitializeStates()
	{
		Grounded = new PlayerGroundState();
		Airborne = new PlayerAirborneState();
		Climb = new PlayerClimbState();
		Stunned = new PlayerStunnedState();
		Dead = new PlayerDeadState();
	}
}
