using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using Unity.VisualScripting;

public class Tank : TankBase
{
    private const float Speed = 5.0f;
    private const float INPUT_THRESHOLD = 0.1f;

    [SerializeField]
    private GameObject TankLevel1;
	[SerializeField]
	private Transform ShootTransform1;
	[SerializeField]
	private GameObject TankLevel2;
	[SerializeField]
	private Transform ShootTransform2;
	[SerializeField]
	private GameObject TankLevel3;
	[SerializeField]
	private Transform ShootTransform3;
	[SerializeField]
	private GameObject TankLevel4;
	[SerializeField]
	private Transform ShootTransform4;
	[SerializeField]
    private GameObject MissilePrefab;
    [SerializeField]
    private int ShootLimit1;
	[SerializeField]
	private int ShootLimit2;
	[SerializeField]
	private int ShootLimit3;
	[SerializeField]
	private int ShootLimit4;

	private int tankLevel = 0;
    private List<GameObject> tankLevels;
    private List<Transform> shootingPoints;
    private List<int> shootLimits;
	private Team team;

	
    // Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
    {
        tankLevels = new() { TankLevel1, TankLevel2, TankLevel3, TankLevel4, };
		tankLevels = tankLevels.Where(tank => tank != null).ToList();

		shootingPoints = new() { ShootTransform1, ShootTransform2, ShootTransform3, ShootTransform4, };
		shootingPoints = shootingPoints.Where(point => point != null).ToList();

		shootLimits = new() { ShootLimit1, ShootLimit2, ShootLimit3, ShootLimit4, };
		shootLimits  = shootLimits.Where(limit => limit >= 0).ToList();

		team = GetComponent<Damageable>().GetTeam();
		SetTankLevel(0);
	}

	public void SetTankLevel(int level)
    {
        if (level < ((tankLevels == null) ? 0 : tankLevels?.Count))
        {
			tankLevels.ForEach(tank => tank.SetActive(false));
			
			tankLevel = level;
            tankLevels[level].SetActive(true);

			SetTeam(team);
			SetSpeed(Speed);
			SetInputThreshold(INPUT_THRESHOLD);
			SetMaxMissilesLaunched(shootLimits[level]);
        }
    }

	public void AddTankLevel()
	{
		SetTankLevel(tankLevel + 1);
	}

    public void LaunchMissile()
    {
		ShootMissile(shootingPoints[tankLevel],MissilePrefab);

		//SetTankLevel((tankLevel + 1) %  tankLevels.Count);
	}

	public void StartShield()
	{
		;
	}

	// Update is called once per frame
	void Update()
    {
        
    }
}
