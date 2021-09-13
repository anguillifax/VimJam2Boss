using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameController : MonoBehaviour
{

	public static GameController Singleton;

	public UnityEvent OnReset;

	private void Awake()
	{
		if (!Singleton)
		{
			Singleton = this;
			DontDestroyOnLoad(gameObject);
		} else
		{
			Destroy(gameObject);
		}
	}

	/*private void Start()
	{
		OnReset.Invoke();
	}*/
}
