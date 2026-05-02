using UnityEngine;

public sealed class LocalPlayer : MonoBehaviour
{
	[Header("Modules")]
	public PlayerInputState Input { get; private set; }
	public PlayerMotor Motor { get; private set; }
	public PlayerOverlapSensor OverlapSensor { get; private set; }
	public PlayerInteraction Interaction { get; private set; }
	public PlayerAudio Audio { get; private set; }
	public LocalPlayerAnimator Animator { get; private set; }

	[Header("States")]
	private ILocalPlayerState currentState;

	public ILocalPlayerState Grounded { get; private set; }
	public ILocalPlayerState Airborne { get; private set; }
	public ILocalPlayerState Climb { get; private set; }

	private void Awake()
	{
		InitializeModules();
		InitializeStates();
	}

	private void Start()
	{
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

	public void ChangeState(ILocalPlayerState newState)
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
		Input = GetComponent<PlayerInputState>();
		Motor = GetComponent<PlayerMotor>();
		OverlapSensor = GetComponent<PlayerOverlapSensor>();
		Interaction = GetComponentInChildren<PlayerInteraction>();
		Audio = GetComponent<PlayerAudio>();
		Animator = GetComponent<LocalPlayerAnimator>();
	}

	private void InitializeStates()
	{
		Grounded = new LocalPlayerGroundState();
		Airborne = new LocalPlayerAirborneState();
		Climb = new LocalPlayerClimbState();
	}
}
