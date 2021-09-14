using UnityEngine;

[CreateAssetMenu(fileName = "PhysicsData", menuName = "ScriptableObjects/PhysicsData")]
public class PhysicsData : ScriptableObject
{
	public AnimationCurve KnockbackFalloff;
	public float KnockbackResistance;
	public float Drag;
}
