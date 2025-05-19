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
	[SerializeField]
	private ParticleSystem Particles;

	private bool isStoppedWatch;
	private bool isActivated;


	private void Awake()
	{
		isStoppedWatch = false;
		isActivated = false;
		
	}

	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
	{
		SetParticles(Particles);
		SetSpeed(isStoppedWatch || !isActivated? 0 : Speed);
		SetTeam(GetComponent<Damageable>().GetTeam());
		SetInputThreshold(INPUT_THRESHOLD);
		SetMaxMissilesLaunched(ShootLimit);
	}

	public void LaunchMissile()
	{
		if (!isStoppedWatch && isActivated)
		{
			ShootMissile(ShootTransform, MissilePrefab);
		}
	}

	public void Stop()
	{
		isStoppedWatch = true;
		isActivated = false;
		SetSpeed(0);
	}

	public void Play()
	{
		isStoppedWatch = false;
		isActivated = true;
		SetSpeed(Speed);
	}

	public override void Activate()
	{
		base.Activate();
		Debug.Log("activate");
		Play();
	}

	public override void Deactivate()
	{
		base.Deactivate();
		Stop();
	}
}
