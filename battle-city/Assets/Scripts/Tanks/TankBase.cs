using System;
using UnityEngine;

public class TankBase : MonoBehaviour
{
	private float speed;
	private float inputThreshold;
	private int maxMissilesLaunched;

	public void SetSpeed(float speed)
	{
		this.speed = speed;
	}

	public float GetSpeed() => speed;
	
	public void SetInputThreshold(float inputThreshold)
	{
		this.inputThreshold = inputThreshold;
	}

	public float GetInputThreshold() => inputThreshold;

	public void SetMaxMissilesLaunched(int maxMissilesLaunched)
	{
		this.maxMissilesLaunched = maxMissilesLaunched;
	}

	public int GetMaxMissilesLaunched() => maxMissilesLaunched;
	
	public void Move(Vector2 input)
	{
		var input3D = new Vector3(input.x, 0, input.y);

		if (input3D.sqrMagnitude > inputThreshold)
		{
			var rotation = new Quaternion();
			rotation.SetLookRotation(input3D, Vector3.up);
			transform.rotation = rotation;

			var displacement = speed * Time.deltaTime * input3D;
			transform.position += displacement;
		}
	}

	protected void ShootMissile(Transform shootingPoint, GameObject MissilePrefab, int damage)
	{
		if (maxMissilesLaunched == 0)
		{
			return;
		}

		maxMissilesLaunched--;

		var missileObj = Instantiate(MissilePrefab, shootingPoint.position, shootingPoint.rotation);
		var missile = missileObj.GetComponent<Missile>();
		missile.SetDamage(damage);
		missile.OnMissileDestroy += OnMissileDestroy;
	}

	public void OnMissileDestroy(object sender, EventArgs args)
	{
		maxMissilesLaunched++;
	}
}
