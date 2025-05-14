using System.Collections;
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

	private bool isStoppedWatch;


	private void Awake()
	{
		isStoppedWatch = false;
	}

	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
	{
		SetSpeed(isStoppedWatch ? 0 : Speed);
		SetTeam(GetComponent<Damageable>().GetTeam());
		SetInputThreshold(INPUT_THRESHOLD);
		SetMaxMissilesLaunched(ShootLimit);
	}

	public void LaunchMissile()
	{
		if (!isStoppedWatch)
		{
			ShootMissile(ShootTransform, MissilePrefab);
		}
	}

	public void Stop()
	{
		isStoppedWatch = true;
		SetSpeed(0);
	}

	public void Play()
	{
		isStoppedWatch = false;
		SetSpeed(Speed);
	}

	// Update is called once per frame
	void Update()
	{

	}
}
