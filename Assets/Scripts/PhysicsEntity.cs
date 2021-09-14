using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Object or creature that interacts with the physics system
[RequireComponent(typeof(Rigidbody))]
public class PhysicsEntity : ResettableObject
{

	public PhysicsData physicsData;

	// External velocity. Object is *influenced* to move
	protected Vector3 latentVelocity = Vector3.zero;
	protected Rigidbody rb;
	// Percentage of knockback ignored. Ranges from 0 to 1
	protected float currentKnockbackResistance = 0f;

	private Vector3 resetPosition;
	private Quaternion resetRotation;

	private void Start()
	{
		rb = GetComponent<Rigidbody>();
		resetPosition = transform.position;
		resetRotation = transform.rotation;
	}


	void FixedUpdate()
	{
		ExtendableFixedUpdate();
	}

	protected virtual void ExtendableFixedUpdate()
	{
		if (!Dead)
		{
			SimulateDrag();
			ApplyPhysics();
		}
	}

	public virtual void TakeKnockback(float magnitude, Vector3 origin)
	{
		Vector3 direction = transform.position - origin;
		direction.y = 0;
		float falloff = physicsData.KnockbackFalloff.Evaluate(direction.magnitude);
		direction = direction.normalized;
		latentVelocity += direction * magnitude * falloff * (1 - currentKnockbackResistance) * (1 - physicsData.KnockbackResistance);
	}

	public virtual void TakeKnockback(float magnitude, Vector3 direction, float simulatedFalloffDist)
	{
		Vector3 adjustedDirection = direction;
		adjustedDirection.y = 0;
		float falloff = physicsData.KnockbackFalloff.Evaluate(simulatedFalloffDist);
		adjustedDirection = adjustedDirection.normalized;
		latentVelocity += adjustedDirection * magnitude * falloff * (1 - currentKnockbackResistance) * (1 - physicsData.KnockbackResistance);
	}

	protected virtual void ApplyPhysics()
	{
		rb.velocity = latentVelocity;
	}

	protected virtual void SimulateDrag()
	{
		latentVelocity *= 1 - (physicsData.Drag * Time.fixedDeltaTime);
	}

	public virtual void ResetPhysics()
	{
		latentVelocity = Vector3.zero;
		rb.velocity = Vector3.zero;
	}

	public override void Die()
	{
		base.Die();
		currentKnockbackResistance = 1;
	}

	public override void ResetObj()
	{
		base.ResetObj();
		ResetPhysics();
		currentKnockbackResistance = 0;
		transform.position = resetPosition;
		transform.rotation = resetRotation;
	}
}
