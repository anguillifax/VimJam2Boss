using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAppearance : MonoBehaviour
{

	public float DeflectedShieldKnockbackStrength;
	public float DeflectedShieldAngle = 75f;

	public GameObject VisualsRoot;
	public GameObject HeldShield;
	public GameObject EmbeddedShield;
	public GameObject DeflectedShield;

	private PlayerAvatar player;

	private void Start()
	{
		player = GetComponent<PlayerAvatar>();
	}

	public void ThrowGrapple(bool shieldEmbedded, GrapplePoint shieldTarget)
	{
		HeldShield.SetActive(false);

		if (shieldEmbedded)
		{
			EmbeddedShield.SetActive(true);
			EmbeddedShield.transform.parent = shieldTarget.transform;
			EmbeddedShield.transform.localPosition = Vector3.zero;
		} else
		{
			DeflectedShield.SetActive(true);
			DeflectedShield.transform.position = shieldTarget.transform.position;
			DeflectedShield.GetComponent<PhysicsEntity>().TakeKnockback(DeflectedShieldKnockbackStrength, Quaternion.AngleAxis(DeflectedShieldAngle, Vector3.up) * (player.transform.position - shieldTarget.transform.position).normalized, 0);
		}
	}

	public void ReturnGrapple ()
	{
		EmbeddedShield.transform.parent = null;
		EmbeddedShield.SetActive(false);
		DeflectedShield.SetActive(false);
		HeldShield.SetActive(true);
	}
}
