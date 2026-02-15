using UnityEngine;

public sealed class Player : MonoBehaviour
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
	public PlayerBombPlacer BombPlacer { get; private set;}

	[Header("States")]
	private IPlayerState currentState;
	
	public IPlayerState Grounded { get; private set; }
	public IPlayerState Airborne { get; private set; }
	public IPlayerState Climb { get; private set; }
	public IPlayerState Stunned { get; private set; }
	public IPlayerState Dead { get; private set; }

	public event System.Action OnDeath;
	public event System.Action<int> OnCheckHP;


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
		BombPlacer = GetComponent<PlayerBombPlacer>();
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

	private void Update()
	{
		currentState?.Tick(this);
	}

	private void FixedUpdate()
	{
		Motor.DetectGrounded();
		currentState?.FixedTick(this);
	}
	public void ApplyDamage(int damage, float stunDuration, bool knockback)
	{
		if (currentState == Dead || currentState == Stunned)
			return;
		
		Status.ChangeHealth(-damage);
		OnCheckHP?.Invoke(Status.CurrentHealth);
		HistoryManager.Instance.UpdateHP(Status.CurrentHealth);

		Animator.DoDamageAnim();

		if (Status.CurrentHealth <= 0)
		{
			ChangeState(Dead);
			OnDeath?.Invoke();
			return;
		}

		if (stunDuration > 0f)
		{
			((PlayerStunnedState)Stunned).SetDuration(stunDuration);
			ChangeState(Stunned);
		}

		if (knockback)
		{
			float dir;

			dir = Motor.IsFacingRight ? -1f : 1f;
			Motor.ApplyImpulse(new Vector2(dir * 4f, 2f));
		}
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
		if (!BombPlacer)
			BombPlacer = GetComponent<PlayerBombPlacer>();
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
