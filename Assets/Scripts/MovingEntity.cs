using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// A physics entity that moves based on provided input
public class MovingEntity : PhysicsEntity
{

	public float MovementSpeed;

	// Internal velocity. Object is *trying* to move
	public Vector3 MovementDirection = Vector3.zero;

	void FixedUpdate()
	{
		if (!Dead)
		{
			ApplyPhysics();
		}
	}

	protected override void ApplyPhysics()
	{
		rb.velocity = latentVelocity + MovementDirection;
	}

	public override void Die()
	{
		base.Die();
		MovementDirection = Vector3.zero;
	}

	public override void Reset()
	{
		base.Reset();
		MovementDirection = Vector3.zero;
	}
}
