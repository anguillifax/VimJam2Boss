using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ResettableObject))]
public class Health : MonoBehaviour
{

	[Tooltip("Percentage of incoming damage ignored")]
	[Range(0f, 1f)]
	public float DamageResistance = 0f;

	[SerializeField]
	float maxValue;
	float value;
	ResettableObject entity;

	private void Awake()
	{
		entity = GetComponent<ResettableObject>();
	}

	public void AdjustValue (float adjustment)
	{
		// Taken damage while dead - ignore
		if (entity.Dead)
		{
			return;
		}

		value += adjustment * (1f - DamageResistance);

		if (value <= 0 && entity)
		{
			entity.Die();
		}
	}

	public void Reset()
	{
		value = maxValue;
	}

	private void OnEnable()
	{
		GameController.Singleton.OnReset.AddListener(Reset);
	}

	private void OnDisable()
	{
		GameController.Singleton.OnReset.RemoveListener(Reset);
	}
}
