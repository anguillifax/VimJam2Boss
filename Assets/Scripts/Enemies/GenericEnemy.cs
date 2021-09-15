using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GenericEnemy : MonoBehaviour
{

	public bool Dead = false;

	protected MovingEntity entity;
	protected GrapplePoint grapplePoint;

	private void Start()
	{
		entity = GetComponent<MovingEntity>();
		grapplePoint = GetComponent<GrapplePoint>();
	}

	/*
	public void Update()
	{
		ExtendableUpdate();
	}

	public virtual void ExtendableUpdate()
	{
		Navigate();
		TryAttack();
	}

	public abstract void Navigate();

	public abstract void TryAttack();*/

	public void Die(float knockback, Vector3 knockbackDirection)
	{
		entity.TakeKnockback(knockback, knockbackDirection, 0);
		grapplePoint.Enabled = false;
		Dead = true;
	}
}
