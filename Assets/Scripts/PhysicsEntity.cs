using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Object or creature that interacts with the physics system
[RequireComponent(typeof(Rigidbody))]
public class PhysicsEntity : ResettableObject
{

	[Tooltip("Percentage of incoming knockback ignored")]
	[Range(0f, 1f)]
	public float BaseKnockbackResistance = 0f;

	// External velocity. Object is *influenced* to move
	protected Vector3 latentVelocity;
	protected Rigidbody rb;
	protected float currentKnockbackResistance;

	private Vector3 resetPosition;
	private Quaternion resetRotation;

	private void Start()
	{
		rb = GetComponent<Rigidbody>();
		resetPosition = transform.position;
		resetRotation = transform.rotation;
	}


	void Update()
	{
		if (!Dead)
		{
			ApplyPhysics();
		}
	}

	public virtual void TakeKnockback(float magnitude, Vector3 origin)
	{
		Vector3 direction = (transform.position - origin).normalized;
		direction.y = 0;
		latentVelocity += direction * magnitude * currentKnockbackResistance * BaseKnockbackResistance;
	}

	protected virtual void ApplyPhysics()
	{
		rb.velocity = latentVelocity;
	}

	public void ResetPhysics()
	{
		latentVelocity = Vector3.zero;
		rb.velocity = Vector3.zero;
	}

	public override void Die()
	{
		base.Die();
		currentKnockbackResistance = 1;
	}

	public override void Reset()
	{
		base.Reset();
		ResetPhysics();
		currentKnockbackResistance = 0;
		transform.position = resetPosition;
		transform.rotation = resetRotation;
	}
}
