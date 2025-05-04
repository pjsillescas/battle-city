using UnityEngine;

public class TankEnemy : TankBase
{
	private const float INPUT_THRESHOLD = 0.1f;

	[SerializeField]
	private float Speed = 5.0f;
	[SerializeField]
	private Transform ShootTransform;
	[SerializeField]
	private GameObject MissilePrefab;
	[SerializeField]
	private int ShootLimit;

	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
	{
		SetSpeed(Speed);
		SetInputThreshold(INPUT_THRESHOLD);
		SetMaxMissilesLaunched(ShootLimit);
	}

	public void LaunchMissile()
	{
		ShootMissile(ShootTransform, MissilePrefab);
	}

	// Update is called once per frame
	void Update()
	{

	}
}
