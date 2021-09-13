using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetingSystem : MonoBehaviour
{

	public float DetectionRadius;
	public LayerMask TargetMask;

	private Camera mainCam;
	private Collider[] targetResults;

	private void Start()
	{
		mainCam = Camera.main;
	}

	// Uses a spherecast to find the nearest PhysicsEntity within a radius of the cursor
	// Breaks if any targets are not located on a 2D plane
	public PhysicsEntity GetTarget ()
	{
		Vector3 cursorWorldPosition = mainCam.ScreenToWorldPoint(Input.mousePosition);
		if (Physics.OverlapSphereNonAlloc(cursorWorldPosition, DetectionRadius, targetResults, TargetMask) < 1)
		{
			return null;
		}

		Collider result = targetResults[0];

		// Pick the closest target if there are multiple
		if (targetResults.Length > 1)
		{
			float closestDist = Mathf.Infinity;
			foreach (Collider col in targetResults)
			{
				float dist = (cursorWorldPosition - col.transform.position).magnitude;
				if (dist < closestDist)
				{
					result = col;
					closestDist = dist;
				}
			}
		}

		return result.GetComponent<PhysicsEntity>();
	}
}
