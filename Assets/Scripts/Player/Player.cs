using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Parent class. Processes input and handles state machine
[RequireComponent(typeof(PlayerController))]
[RequireComponent(typeof(MovingEntity))]
[RequireComponent(typeof(Health))]
public class Player : MonoBehaviour
{
	public enum State
	{
		Movement,
		Dash,
		Charge,
		Dying,
		Inactive // Used for cutscenes and start menu
	}

	public State currentState = State.Movement;
	public bool HoldingShield;

	public Vector3 MovementInput;
	public Transform EmbeddedTarget;

	private MovingEntity entity;

	private void Start()
	{
		entity = GetComponent<MovingEntity>();
	}


	public void ThrowShield()
	{

	}

	public void Dash()
	{
		if (currentState == State.Movement)
		{
			currentState = State.Dash;
		}
	}

	public void Charge()
	{
		if (currentState == State.Movement)
		{
			currentState = State.Charge;
		}
	}

	// TODO: Behavior when shield is deflected by an enemy/object
	public void DeflectShield()
	{

	}

	public void Die()
	{
		currentState = State.Dying;
	}

	
	
	private void Update()
	{
		switch (currentState)
		{
			case State.Movement:

				entity.MovementDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")).normalized;
				break;
			case State.Dash:
				// Ignore movement input for a brief moment, but continue moving in the same direction
				// Disable hitbox? Might be overpowered
				break;
			case State.Charge:
				if (!EmbeddedTarget)
				{
					Debug.LogError("Tried to charge without grappling an enemy! How?!");
				}
				entity.MovementDirection = (EmbeddedTarget.position - transform.position).normalized;
				break;
			default:
				return;
		}
	}

	private void FixedUpdate()
	{
		
	}
	private void OnDisable()
	{
		entity.MovementDirection = Vector3.zero;
	}

}
