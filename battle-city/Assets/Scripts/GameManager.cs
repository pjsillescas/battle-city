using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	[SerializeField]
	private LevelFileLoader LevelLoader;
	[SerializeField]
	private GameConfiguration Configuration;
	[SerializeField]
	private bool loadDebugLevel = false;
	[SerializeField]
	private SpawnPoint Player1SpawnPoint;
	[SerializeField]
	private GameObject Player1Prefab;
	[SerializeField]
	private SpawnPoint Player2SpawnPoint;
	[SerializeField]
	private GameObject Player2Prefab;
	[SerializeField]
	private PlayerController PlayerController1;
	[SerializeField]
	private PlayerController PlayerController2;
	[SerializeField]
	private List<EnemySpawner> EnemySpawners;
	[SerializeField]
	private GameObject BasicTankPrefab;
	[SerializeField]
	private GameObject StrikeTankPrefab;
	[SerializeField]
	private GameObject MediumTankPrefab;
	[SerializeField]
	private GameObject HeavyTankPrefab;

	private static GameManager instance = null;

	public static GameManager GetInstance() => instance;

	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
	{
		if (instance != null)
		{
			Debug.LogError("Game Manager duplicated");
			return;
		}

		instance = this;

		EnemySpawners = new();

		LevelFileLoader.OnLevelLoaded += OnLevelLoaded;

		if (!loadDebugLevel)
		{
			LevelLoader.LoadLevel(new LevelObject() { tiles = Configuration.levelTiles, tanks = Configuration.tanks });
		}
		else
		{
			OnLevelLoaded(null, null);
		}
	}

	public void RegisterPlayer1SpawnPoint(SpawnPoint playerSpawnPoint)
	{
		Player1SpawnPoint = playerSpawnPoint;
	}

	public void RegisterPlayer2SpawnPoint(SpawnPoint playerSpawnPoint)
	{
		Player2SpawnPoint = playerSpawnPoint;
	}
	public void RegisterEnemySpawner(EnemySpawner enemySpawner)
	{
		EnemySpawners.Add(enemySpawner);
	}

	private void OnLevelLoaded(object sender, LevelObject levelObject)
	{
		Debug.Log("level loaded");
		// NavigationController::GetInstance().RebuildNavMesh();
		NavigationController.GetInstance().RebuildNavMesh();

		if (Player1SpawnPoint != null && Player1Prefab != null && PlayerController1)
		{
			var player1Pawn = Instantiate(Player1Prefab, Player1SpawnPoint.transform.position, Player1SpawnPoint.transform.rotation);
			PlayerController1.SetPawnTank(player1Pawn.GetComponent<Tank>());
		}
		
		if (Player2SpawnPoint != null && Player2Prefab != null && PlayerController2)
		{
			var player2Pawn = Instantiate(Player2Prefab, Player2SpawnPoint.transform.position, Player2SpawnPoint.transform.rotation);
			PlayerController2.SetPawnTank(player2Pawn.GetComponent<Tank>());
		}

		Debug.Log($"basic: {levelObject.tanks.basic}");
		Debug.Log($"strike: {levelObject.tanks.strike}");
		Debug.Log($"medium: {levelObject.tanks.medium}");
		Debug.Log($"heavy: {levelObject.tanks.heavy}");

		var tanks = new List<TankEnemy>();

		if (EnemySpawners.Count == 0)
		{
			return;
		}
		
		FillTanks(BasicTankPrefab, levelObject.tanks.basic, ref tanks);
		FillTanks(StrikeTankPrefab, levelObject.tanks.strike, ref tanks);
		FillTanks(MediumTankPrefab, levelObject.tanks.medium, ref tanks);
		FillTanks(HeavyTankPrefab, levelObject.tanks.heavy, ref tanks);

		tanks.FisherYatesShuffle();
		
		int k = 0;
		var loads = new List<TankEnemy>[EnemySpawners.Count];
		for(int i = 0; i< EnemySpawners.Count;i++)
		{
			loads[i] = new();
		}
		foreach (var tank in tanks)
		{
			loads[k].Add(tank);
			k = (k + 1) % EnemySpawners.Count;
		}

		for (k = 0; k < EnemySpawners.Count; k++)
		{
			EnemySpawners[k].LoadTanks(loads[k]);
		}
	}

	private void FillTanks(GameObject prefab, int numTanks, ref List<TankEnemy> tanks)
	{
		for (int k = 0; k < numTanks; k++)
		{
			var tank = Instantiate(prefab, Vector3.zero, Quaternion.identity);
			tank.SetActive(false);
			tanks.Add(tank.GetComponent<TankEnemy>());
		}
	}

	// Update is called once per frame
	void Update()
	{

	}
}
