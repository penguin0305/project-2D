using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

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

	public NetworkVariable<bool> isFacingLeft = new NetworkVariable<bool>(
		false,
		NetworkVariableReadPermission.Everyone,
		NetworkVariableWritePermission.Owner
	);

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
	}

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
		else
		{
			var items = PlayerSession.Instance?.PlayerItems;
			if (items != null)
			{
				var itemsNet = new PlayerItemNetwork[items.Count];
				for (int i = 0; i < items.Count; i++)
				{
					itemsNet[i] = new PlayerItemNetwork
					{
						eid = items[i].eid,
						enhance_level = items[i].enhance_level,
						dup_count = items[i].dup_count,
						enhance_fail_count = items[i].enhance_fail_count
					};
				}
				SubmitItemsServerRpc(itemsNet);
			}
		}
	}

	[Rpc(SendTo.Server)]
	private void SubmitItemsServerRpc(PlayerItemNetwork[] items)
	{
		var bonus = BonusStatCalculator.Calculate(items);
		Status.ApplyBonus(bonus);
		Status.ChangeHealth(Status.MaxHealth);
	}

	public override void OnNetworkDespawn()
	{
	}

	private void Update()
	{
		currentState?.Tick(this);
	}

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

	public void TakeDamage(int damage, float stunDuration, bool knockback)
	{
		if (IsServer)
			ApplyDamage(damage, stunDuration, knockback);
		else
			TakeDamageServerRpc(damage, stunDuration, knockback);
	}

	[Rpc(SendTo.Server, InvokePermission = RpcInvokePermission.Everyone)]
	public void TakeDamageServerRpc(int damage, float stunDuration, bool knockback)
	{
		ApplyDamage(damage, stunDuration, knockback);
	}

	private void ApplyDamage(int damage, float stunDuration, bool knockback)
	{
		if (currentState == Dead || currentState == Stunned)
			return;

		int finalDamage = Mathf.Max(1, damage - Status.Armor);
		Status.ChangeHealth(-finalDamage);
		OnCheckHP?.Invoke(Status.CurrentHealth);

		DamageAnimClientRpc();

		if (Status.CurrentHealth <= 0)
		{
			OnDeath?.Invoke();
			SetDeadClientRpc();
			return;
		}

		if (stunDuration > 0f)
			SetStunnedClientRpc(stunDuration);

		if (knockback)
		{
			float dir = Motor.IsFacingRight ? -1f : 1f;
			KnockbackClientRpc(dir);
		}
	}

	[Rpc(SendTo.ClientsAndHost)]
	private void DamageAnimClientRpc()
	{
		Animator?.DoDamageAnim();
	}

	[Rpc(SendTo.ClientsAndHost)]
	private void SetDeadClientRpc()
	{
		Status.ChangeHealth(-Status.CurrentHealth);
		ChangeState(Dead);
	}

	[Rpc(SendTo.ClientsAndHost)]
	private void SetStunnedClientRpc(float stunDuration)
	{
		((PlayerStunnedState)Stunned).SetDuration(stunDuration);
		ChangeState(Stunned);
	}

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