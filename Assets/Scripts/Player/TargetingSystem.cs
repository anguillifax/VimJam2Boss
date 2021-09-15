using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetingSystem : MonoBehaviour
{

	public float DetectionRadius;
	public LayerMask TargetMask;

	private Camera mainCam;
	private Collider[] spherecastResults = new Collider[5];
	private GrapplePoint[] targetResults = new GrapplePoint[5];
	private Vector3 cameraOffset;

	private void Start()
	{
		mainCam = Camera.main;
		cameraOffset = mainCam.transform.position;
		cameraOffset.x = 0;
		cameraOffset.z = 0;
	}

	// Uses a spherecast to find all PhysicsEntities within a radius of the cursor
	// Once it does so, it removes all entities that do not have a GrapplePoint attached
	// Finally, it sorts them by distance and chooses the closest one
	// Breaks if any targets are not at y position 0
	public GrapplePoint GetTarget ()
	{
		Vector3 cursorWorldPosition = mainCam.ScreenToWorldPoint(Input.mousePosition);
		int resultCount = Physics.OverlapSphereNonAlloc(cursorWorldPosition - cameraOffset, DetectionRadius, spherecastResults, TargetMask);

		// Check if there are any targets found
		if (resultCount < 1)
		{
			//print("No targets found");
			return null;
		}

		// Create an array of all the grappleable targets
		int grappleableResultCount = 0;

		for (int i = 0; i < resultCount; i++)
		{
			if (spherecastResults[i].GetComponent<GrapplePoint>() && spherecastResults[i].GetComponent<GrapplePoint>().Enabled)
			{
				targetResults[grappleableResultCount] = spherecastResults[i].GetComponent<GrapplePoint>();
				grappleableResultCount++;
			}
		}

		// Check if there are any grappleable targets found
		if (grappleableResultCount < 1)
		{
			//print("No grappleable targets found");
			return null;
		}

		GrapplePoint result = targetResults[0];

		// Pick the closest grappleable target if there are multiple
		if (grappleableResultCount > 1)
		{
			float closestDist = Mathf.Infinity;
			for (int i = 0; i < grappleableResultCount; i++)
			{
				float dist = (cursorWorldPosition - targetResults[i].transform.position).magnitude;
				if (dist < closestDist)
				{
					result = targetResults[i];
					closestDist = dist;
				}
			}
		}

		//print("Nearest target: " + result.gameObject.name);
		return result;
	}
}
