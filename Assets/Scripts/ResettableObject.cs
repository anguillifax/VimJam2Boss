using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// Handles death & reset calls/events
public class ResettableObject : MonoBehaviour
{

	public UnityEvent OnDeath;

	public bool Dead { get; private set; }

	// This object is the parent in the heirarchy under which all visuals related to the entity are stored.
	protected GameObject VisualsRoot;

	public virtual void Reset ()
	{
		Dead = false;
	}
	public virtual void Die()
	{
		Dead = true;
		if (OnDeath != null)
		{
			OnDeath.Invoke();
		}
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
