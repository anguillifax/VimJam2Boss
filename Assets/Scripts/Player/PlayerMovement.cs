using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

	[Header("Values")]
	public float DashSpeed;
	public float ChargeSpeed;
	public float MovementSpeed;

	public Vector3 MovementInput;

	protected MovingEntity entity;

	public bool charging = false;
	public bool dashing = false;

	private void Start()
	{
		entity = GetComponent<MovingEntity>();
	}

	private void Update()
	{
		if (!charging && !dashing)
		{
			entity.MovementDirection = MovementInput;
		}

		if (charging)
		{
			entity.MovementSpeed = ChargeSpeed;
		} else if (dashing)
		{
			entity.MovementSpeed = DashSpeed;
		}
		else
		{
			entity.MovementSpeed = MovementSpeed;
		}
	}
}
