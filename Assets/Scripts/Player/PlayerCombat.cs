using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{

	public TargetingSystem targeting;
	public GrapplePoint EmbeddedTarget = null;

	private void Start()
	{
		targeting = GetComponent<TargetingSystem>();
	}

	public GrapplePoint TryThrowShield()
	{
		GrapplePoint grapplePoint = targeting.GetTarget();
		if (grapplePoint != null)
		{
			if (!grapplePoint.Deflecting)
			{
				EmbeddedTarget = grapplePoint;
			}
		}

		return grapplePoint;
	}

	public void ReturnShield()
	{
		EmbeddedTarget = null;
	}

	// TODO: Behavior when shield is deflected by an enemy/object
	public void DeflectShield()
	{

	}
}
