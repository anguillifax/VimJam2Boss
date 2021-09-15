using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Handles state machine
public class PlayerAvatar : MonoBehaviour
{
	public enum PrimaryState
	{
		Movement,
		Dash,
		Charge,
		Dying,
		Inactive // Used for cutscenes and start menu
	}

	public enum GrappleState
	{
		Held,
		Embedded,
		Deflected,
		Disabled
	}

	public static PlayerAvatar Instance;

	public bool DashEnabled = false;

	[Header("State Machine")]
	public PrimaryState currentPrimaryState = PrimaryState.Movement;
	public GrappleState currentGrappleState = GrappleState.Held;

	[Header("Timings")]
	public float ChargeLength;
	public float DashLength;
	public float GrappleThrowInputCooldown;
	public float GrappleReturnInputCooldown;
	public float GrappleDeflectedInputCooldown;

	[Header("Values")]
	public float DashSpeed;
	public float MovementSpeed;
	public float KillKnockbackMagnitude;
	public AnimationCurve ChargeFollowThroughSpeedMap;

	[Header("Input")]
	public Vector3 MovementInput = Vector3.zero;
	public bool DashInputImpulse = false;
	public bool AttackInputImpulse = false;

	private MovingEntity entity;
	private PlayerCombat combat;
	private PlayerAppearance appearance;
	private float movementInputCooldown = 0;
	private float grappleInputCooldown = 0;
	private float chargeSpeedCache = 0;

	private void Start()
	{
		if (!Instance)
		{
			Instance = this;
		} else
		{
			Debug.LogError("Why are there multiple instances of the player?!");
		}

		entity = GetComponent<MovingEntity>();
		combat = GetComponent<PlayerCombat>();
		appearance = GetComponent<PlayerAppearance>();
	}

	private void Update()
	{
		switch (currentPrimaryState)
		{
			case PrimaryState.Movement:

				// == Dashing ==

				if (DashInputImpulse && DashEnabled)
				{
					currentPrimaryState = PrimaryState.Dash;
					movementInputCooldown = DashLength;
					entity.MovementSpeed = DashSpeed;
					// TODO: Check if the following lines are balanced features
					entity.ResetPhysics();
					break;
				}

				// == Grappling ==

				if (AttackInputImpulse && grappleInputCooldown <= 0)
				{

					// Grapple embedded - charge target
					if (currentGrappleState == GrappleState.Embedded)
					{
						currentPrimaryState = PrimaryState.Charge;

						entity.ResetPhysics();
						entity.MovementDirection = (combat.EmbeddedTarget.transform.position - transform.position).normalized;
						chargeSpeedCache = (combat.EmbeddedTarget.transform.position - transform.position).magnitude / ChargeLength;
						entity.MovementSpeed = chargeSpeedCache;

						grappleInputCooldown = ChargeLength;
						movementInputCooldown = ChargeLength;

						break;
					}

					// Grapple held - throw grapple
					else if (currentGrappleState == GrappleState.Held)
					{
						int grappleResult = combat.TryGrapple();
						if (grappleResult == 1)
						{
							currentGrappleState = GrappleState.Embedded;
							appearance.ThrowGrapple(true, combat.EmbeddedTarget);
							grappleInputCooldown = GrappleThrowInputCooldown;
						} else if (grappleResult == 2)
						{
							currentGrappleState = GrappleState.Deflected;
							appearance.ThrowGrapple(false, combat.EmbeddedTarget);
							grappleInputCooldown = GrappleDeflectedInputCooldown;
						}
						break;
					}

					// Grapple deflected - return grapple
					else if (currentGrappleState == GrappleState.Deflected)
					{
						combat.ReturnGrapple();
						currentGrappleState = GrappleState.Held;

						appearance.ReturnGrapple();

						grappleInputCooldown = GrappleReturnInputCooldown;
					}
				}

				entity.MovementDirection = MovementInput;
				entity.MovementSpeed = MovementSpeed;

				break;
			case PrimaryState.Dash:
				if (movementInputCooldown <= 0)
				{
					currentPrimaryState = PrimaryState.Movement;
					entity.MovementSpeed = MovementSpeed;
					break;
				}
				entity.MovementSpeed = DashSpeed;
				break;
			case PrimaryState.Charge:
				if (movementInputCooldown <= 0)
				{
					currentPrimaryState = PrimaryState.Movement;
					entity.MovementSpeed = MovementSpeed;
					entity.TakeKnockback(ChargeFollowThroughSpeedMap.Evaluate (chargeSpeedCache), entity.MovementDirection, 0);

					if (combat.EmbeddedTarget.GetComponent<GenericEnemy>())
					{
						combat.EmbeddedTarget.GetComponent<GenericEnemy>().Die(KillKnockbackMagnitude, entity.MovementDirection);
					}
					combat.ReturnGrapple();
					currentGrappleState = GrappleState.Held;

					appearance.ReturnGrapple();

					break;
				}
				entity.MovementSpeed = chargeSpeedCache;
				break;
			default:
				return;
		}

		grappleInputCooldown = Mathf.Max (0, grappleInputCooldown - Time.deltaTime);
		movementInputCooldown = Mathf.Max (0, movementInputCooldown - Time.deltaTime);
	}

	public void Die()
	{
		currentPrimaryState = PrimaryState.Dying;
	}

	private void OnDisable()
	{
		entity.MovementDirection = Vector3.zero;
	}

}
