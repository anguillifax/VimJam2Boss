using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grunt : GenericEnemy
{

	public float MovementSpeed;

	private void Update()
	{
		if (!Dead)
		{
			entity.MovementDirection = (PlayerAvatar.Instance.transform.position - transform.position).normalized;
			entity.MovementSpeed = MovementSpeed;
		} else
		{
			entity.MovementSpeed = 0;
		}
	}

}
