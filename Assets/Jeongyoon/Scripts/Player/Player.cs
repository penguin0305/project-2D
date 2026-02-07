using UnityEngine;

public sealed class Player : MonoBehaviour
{
	[Header("Modules")]
	public PlayerInputState Input { get; private set; }
	public PlayerMotor Motor { get; private set; }
	public PlayerOverlapSensor OverlapSensor { get; private set; }
	public PlayerCombat Combat { get; private set; }
	public PlayerInteraction Interaction { get; private set; }
	public PlayerAudio Audio { get; private set; }
	public PlayerBombPlacer BombPlacer { get; private set;}

	private bool isJumpLatched;

	private IPlayerState currentState;
	
	public IPlayerState Grounded { get; private set; }
	public IPlayerState Airborne { get; private set; }
	public IPlayerState Climb { get; private set; }
	public IPlayerState Stunned { get; private set; }
	public IPlayerState Dead { get; private set; }

private void Reset()
	{
		Input = GetComponent<PlayerInputState>();
		Motor = GetComponent<PlayerMotor>();
		OverlapSensor = GetComponent<PlayerOverlapSensor>();
		Combat = GetComponent<PlayerCombat>();
		Interaction = GetComponentInChildren<PlayerInteraction>();
		Audio = GetComponent<PlayerAudio>();
		BombPlacer = GetComponent<PlayerBombPlacer>();
	}

	private void Awake()
	{
		if (!Input)
			Input = GetComponent<PlayerInputState>();
		if (!Motor)
			Motor = GetComponent<PlayerMotor>();
		if (!OverlapSensor)
			OverlapSensor = GetComponent<PlayerOverlapSensor>();
		if (!Combat)
			Combat = GetComponent<PlayerCombat>();
		if (!Interaction)
			Interaction = GetComponentInChildren<PlayerInteraction>();
		if (!Audio)
			Audio = GetComponent<PlayerAudio>();
		if (!BombPlacer)
			BombPlacer = GetComponent<PlayerBombPlacer>();

		Grounded = new PlayerGroundState();
		Airborne = new PlayerAirborneState();
		Climb = new PlayerClimbState();
		Stunned = new PlayerStunnedState();
		Dead = new PlayerDeadState();

		ChangeState(Grounded);
	}

	private void Update()
	{
		/*
		if (Input.ConsumeJumpPressed())
			isJumpLatched = true;
		if (Input.ConsumeAttackPressed())
			Combat.TryMeleeAttack();
		if (Input.ConsumeInteractPressed())
			Interaction.TryInteract();
		if (Input.ConsumeUseItem1Pressed())
			BombPlacer.TryPlaceBomb();
		if (Input.ConsumeUseItem2Pressed())
			Debug.Log("Item 2 Used (Not implemented)");
			*/
		currentState?.Tick(this);
	}

	private void FixedUpdate()
	{
		/*
		Movement.RequestMove(Input.Move);
		Movement.RequestDash(Input.DashHeld);

		if (isJumpLatched)
		{
			Movement.RequestJump();
			isJumpLatched = false;
		}
		*/
		Motor.DetectGrounded();
		currentState?.FixedTick(this);
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
}
