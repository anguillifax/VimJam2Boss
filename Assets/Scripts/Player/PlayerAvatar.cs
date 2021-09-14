using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Handles state machine
public class PlayerAvatar : MonoBehaviour
{
	public enum State
	{
		Movement,
		Dash,
		Charge,
		Dying,
		Inactive // Used for cutscenes and start menu
	}

	public bool DashEnabled = false;

	[Header("State Machine")]
	public State currentState = State.Movement;
	public bool HoldingShield;
	public bool CanThrowShield;

	[Header("Timings")]
	public float ChargeLength;
	public float DashLength;
	public float ShieldThrowInputPauseTime;
	public float ShieldReturnInputPauseTime;

	[Header("Values")]
	public float DashSpeed;
	public float MovementSpeed;
	public AnimationCurve ChargeFollowThroughSpeedMap;

	[Header("Input")]
	public Vector3 MovementInput = Vector3.zero;
	public bool DashInputImpulse = false;
	public bool AttackInputImpulse = false;

	private MovingEntity entity;
	private PlayerCombat combat;
	private float movementInputPauseTimer = 0;
	private float shieldInputPauseTimer = 0;
	private float chargeSpeedCache = 0;

	private void Start()
	{
		entity = GetComponent<MovingEntity>();
		combat = GetComponent<PlayerCombat>();
	}

	private void Update()
	{
		switch (currentState)
		{
			case State.Movement:

				if (AttackInputImpulse && shieldInputPauseTimer <= 0)
				{
					if (combat.EmbeddedTarget != null)
					{
						currentState = State.Charge;

						entity.ResetPhysics();
						entity.MovementDirection = (combat.EmbeddedTarget.transform.position - transform.position).normalized;
						chargeSpeedCache = (combat.EmbeddedTarget.transform.position - transform.position).magnitude / ChargeLength;
						entity.MovementSpeed = chargeSpeedCache;

						combat.ReturnShield();

						shieldInputPauseTimer = ChargeLength;
						movementInputPauseTimer = ChargeLength;

						break;
					}
					else
					{
						combat.TryThrowShield();
						shieldInputPauseTimer = ShieldThrowInputPauseTime;
					}
				}
				else if (DashInputImpulse && DashEnabled)
				{
					currentState = State.Dash;
					movementInputPauseTimer = DashLength;
					entity.MovementSpeed = DashSpeed;
					// TODO: Check if the following lines are balanced features
					entity.ResetPhysics();
					break;
				}

				entity.MovementDirection = MovementInput;
				entity.MovementSpeed = MovementSpeed;

				break;
			case State.Dash:
				if (movementInputPauseTimer <= 0)
				{
					currentState = State.Movement;
					entity.MovementSpeed = MovementSpeed;
					break;
				}
				entity.MovementSpeed = DashSpeed;
				break;
			case State.Charge:
				if (movementInputPauseTimer <= 0)
				{
					currentState = State.Movement;
					entity.MovementSpeed = MovementSpeed;
					entity.TakeKnockback(ChargeFollowThroughSpeedMap.Evaluate (chargeSpeedCache), entity.MovementDirection, 0);
					break;
				}
				entity.MovementSpeed = chargeSpeedCache;
				break;
			default:
				return;
		}

		shieldInputPauseTimer = Mathf.Max (0, shieldInputPauseTimer - Time.deltaTime);
		movementInputPauseTimer = Mathf.Max (0, movementInputPauseTimer - Time.deltaTime);
	}

	public void Die()
	{
		currentState = State.Dying;
	}

	private void OnDisable()
	{
		entity.MovementDirection = Vector3.zero;
	}

}
