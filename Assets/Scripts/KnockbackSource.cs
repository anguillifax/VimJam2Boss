using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockbackSource : MonoBehaviour
{

	public float knockback;
	public float stunTime;

	private void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.GetComponent<PhysicsEntity>())
		{
			collision.gameObject.GetComponent<PhysicsEntity>().TakeKnockback(knockback, transform.position);
		}

		if (collision.gameObject.GetComponent<MovingEntity>())
		{
			collision.gameObject.GetComponent<MovingEntity>().Stun(stunTime);
		}
	}
}
