using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Processes input and passes it off to the player
public class PlayerController : MonoBehaviour
{

	private Player player;

	private void Start()
	{
		player = GetComponent<Player>();
	}

	private void Update()
	{
		// == Movement ==
		player.MovementInput = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

		// == Combat ==
		
	}

	private void OnDisable()
	{
		player.MovementInput = Vector3.zero;
	}
}
