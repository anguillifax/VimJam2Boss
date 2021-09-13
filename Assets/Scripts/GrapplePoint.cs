using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapplePoint : PhysicsEntity
{
    public override void Die()
	{
		Debug.LogError("Grapple points shouldn't be dying!");
	}
}
