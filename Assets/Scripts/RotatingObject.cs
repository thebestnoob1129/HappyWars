using UnityEngine;

public class RotatingObject : GameLevelObject
{
	public Vector3 AngularVelocity { get; set; }

	public override void GameUpdate()
	{
		transform.Rotate(AngularVelocity * Time.deltaTime);
	}
}