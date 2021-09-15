using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// A physics entity that moves based on provided input
public class MovingEntity : PhysicsEntity
{

	public float StunnedSpeedMultiplier = 0.4f;
	public float StunResistance = 0f;
	public float StunRecoverSpeed = 3f;

	// Internal velocity. Object is *trying* to move
	public Vector3 MovementDirection = Vector3.zero;
	public float MovementSpeed;

	protected float adjustedMovementSpeed = 0;
	protected float stunTime = 0;

	protected override void ExtendableFixedUpdate()
	{
		SimulateStun();
		base.ExtendableFixedUpdate();
	}

	protected override void ApplyPhysics()
	{
		rb.velocity = latentVelocity + (MovementDirection * adjustedMovementSpeed);
	}

	protected virtual void SimulateStun()
	{
		adjustedMovementSpeed = MovementSpeed * (StunnedSpeedMultiplier + (Mathf.Clamp01 (1 - stunTime) * (1 - StunnedSpeedMultiplier)));
		stunTime = Mathf.Max (stunTime - (Time.fixedDeltaTime * StunRecoverSpeed), 0);
	}

	public void Stun(float time)
	{
		stunTime = time * (1f - StunResistance);
	}

	public override void Die()
	{
		base.Die();
		MovementDirection = Vector3.zero;
	}

	public override void ResetPhysics()
	{
		base.ResetPhysics();
		MovementDirection = Vector3.zero;
		stunTime = 0;
	}

	public override void ResetObj()
	{
		base.ResetObj();
		MovementDirection = Vector3.zero;
		stunTime = 0;
	}
}
