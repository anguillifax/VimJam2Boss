using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{

	public GrapplePoint EmbeddedTarget = null;

	private TargetingSystem targeting;

	private void Start()
	{
		targeting = GetComponent<TargetingSystem>();
	}

	// Returns 0 for no target found, 1 for grapple success, and 2 for target deflected grapple
	public int TryGrapple()
	{
		EmbeddedTarget = targeting.GetTarget();

		if (!EmbeddedTarget)
		{
			return 0;
		}

		if (EmbeddedTarget.Deflecting)
		{
			return 2;
		} else
		{
			return 1;
		}
	}

	public void ReturnGrapple()
	{
		EmbeddedTarget = null;
	}

	// TODO: Behavior when shield is deflected by an enemy/object
	public void DeflectShield()
	{

	}
}
