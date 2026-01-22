using UnityEngine;

public sealed class Player : MonoBehaviour
{
	[Header("Modules")]
	[SerializeField] private PlayerInputState input;
	[SerializeField] private PlayerMovement movement;
	[SerializeField] private PlayerCombat combat;
	[SerializeField] private PlayerInteraction interaction;
	[SerializeField] private PlayerBombPlacer bombplacer;

	private bool isJumpLatched;

	private void Reset()
	{
		input = GetComponent<PlayerInputState>();
		movement = GetComponent<PlayerMovement>();
		combat = GetComponent<PlayerCombat>();
		interaction = GetComponentInChildren<PlayerInteraction>();
		bombplacer = GetComponent<PlayerBombPlacer>();
	}

	private void Awake()
	{
		if (!input)
			input = GetComponent<PlayerInputState>();
		if (!movement)
			movement = GetComponent<PlayerMovement>();
		if (!combat)
			combat = GetComponent<PlayerCombat>();
		if (!interaction)
			interaction = GetComponentInChildren<PlayerInteraction>();
		if (!bombplacer)
			bombplacer = GetComponent<PlayerBombPlacer>();
	}

	private void Update()
	{
		if (input.ConsumeJumpPressed())
			isJumpLatched = true;
		if (input.ConsumeAttackPressed())
			combat.TryMeleeAttack();
		if (input.ConsumeInteractPressed())
			interaction.TryInteract();
		if (input.ConsumeUseItem1Pressed())
			bombplacer.TryPlaceBomb();
		if (input.ConsumeUseItem2Pressed())
			Debug.Log("Item 2 Used (Not implemented)");
	}

	private void FixedUpdate()
	{
		movement.RequestMove(input.Move);
		movement.RequestDash(input.DashHeld);

		if (isJumpLatched)
		{
			movement.RequestJump();
			isJumpLatched = false;
		}
	}
}
