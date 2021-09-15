using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Handles death & reset calls/events
public class ResettableObject : MonoBehaviour
{

	public bool Dead { get; private set; }

	// This object is the parent in the heirarchy under which all visuals related to the entity are stored.
	protected GameObject VisualsRoot;

	public virtual void ResetObj ()
	{
		Dead = false;
	}

	public virtual void Die()
	{
		Dead = true;
	}

	private void OnEnable()
	{
		GameController.Singleton.OnReset.AddListener(ResetObj);
	}

	private void OnDisable()
	{
		GameController.Singleton.OnReset.RemoveListener(ResetObj);
	}
}
