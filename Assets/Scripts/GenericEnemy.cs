using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ResettableObject))]
public class GenericEnemy : MonoBehaviour
{
	[Header("Attributes")]
	public bool Embeddable = true;
	public bool Tankable = true;
	public bool Bonkable = true;
	public bool Brawny = false;
	public bool Chunky = false;
	public bool Deflecting = false;
}
