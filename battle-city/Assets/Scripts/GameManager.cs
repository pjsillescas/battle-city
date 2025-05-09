using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;

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

	private List<Vector3> navigablePoints;
	private int currentEnemySpawner;
	private int enemiesRemaining;

	private static GameManager instance = null;

	public static GameManager GetInstance() => instance;

	public List<Vector3> GetNavigablePoints() => navigablePoints;

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
		currentEnemySpawner = 0;
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
			player1Pawn.GetComponent<Damageable>().OnDeath += Player1Respawn;
			PlayerController1.SetPawnTank(player1Pawn.GetComponent<Tank>());
		}
		
		if (Player2SpawnPoint != null && Player2Prefab != null && PlayerController2)
		{
			var player2Pawn = Instantiate(Player2Prefab, Player2SpawnPoint.transform.position, Player2SpawnPoint.transform.rotation);
			player2Pawn.GetComponent<Damageable>().OnDeath += Player2Respawn;
			PlayerController2.SetPawnTank(player2Pawn.GetComponent<Tank>());
		}

		SetNavigablePoints(levelObject);
		
		PreloadEnemyTanks(levelObject);

		PickupManager.GetInstance().Initialize(navigablePoints);
	}
	private void SetNavigablePoints(LevelObject levelObject)
	{
		var floorTiles = new List<TileType>() {
			TileType.Floor,
			TileType.SlipperyFloor,
			TileType.Player1Spawn,
			TileType.Player2Spawn,
			TileType.EnemySpawn,
		};

		navigablePoints = new();

		float z = levelObject.tiles.Count - 1f;
		levelObject.tiles.ForEach(row => {
			float x = 0;
			row.ForEach(tile => {
				if(floorTiles.Contains(tile))
				{
					navigablePoints.Add(new Vector3(x,0,z));
				}

				x = x + 1f;
			});

			z = z - 1f;
		});
	}

	private void PreloadEnemyTanks(LevelObject levelObject)
	{
		Debug.Log($"basic: {levelObject.tanks.basic}");
		Debug.Log($"strike: {levelObject.tanks.strike}");
		Debug.Log($"medium: {levelObject.tanks.medium}");
		Debug.Log($"heavy: {levelObject.tanks.heavy}");

		enemiesRemaining = levelObject.tanks.basic + levelObject.tanks.strike +
			levelObject.tanks.medium + levelObject.tanks.heavy;
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
		for (int i = 0; i < EnemySpawners.Count; i++)
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
			EnemySpawners[k].DeployEnemy();
		}
	}

	private void Player1Respawn(object sender, TankBase tank)
	{
		var player1Pawn = Instantiate(Player1Prefab, Player1SpawnPoint.transform.position, Player1SpawnPoint.transform.rotation);
		player1Pawn.GetComponent<Damageable>().OnDeath += Player1Respawn;
		PlayerController1.SetPawnTank(player1Pawn.GetComponent<Tank>());
	}

	private void Player2Respawn(object sender, TankBase tank)
	{
		var player2Pawn = Instantiate(Player2Prefab, Player2SpawnPoint.transform.position, Player2SpawnPoint.transform.rotation);
		player2Pawn.GetComponent<Damageable>().OnDeath += Player2Respawn;
		PlayerController2.SetPawnTank(player2Pawn.GetComponent<Tank>());
	}

	private void FillTanks(GameObject prefab, int numTanks, ref List<TankEnemy> tanks)
	{
		for (int k = 0; k < numTanks; k++)
		{
			var tank = Instantiate(prefab, Vector3.zero, Quaternion.identity);
			tank.SetActive(false);
			tank.GetComponent<Damageable>().OnDeath += OnEnemyDeath;
			tanks.Add(tank.GetComponent<TankEnemy>());
		}
	}

	private void OnEnemyDeath(object sender, TankBase tank)
	{
		enemiesRemaining--;
		if (enemiesRemaining == 0)
		{
			CompleteLevel();
		}
		else
		{
			DeployEnemy();
		}
	}

	private void DeployEnemy()
	{
		int numExploredSpawners = 0;
		bool enemySpawned = false;

		while(!enemySpawned && numExploredSpawners < EnemySpawners.Count)
		{
			var currentSpawner = EnemySpawners[currentEnemySpawner];
			if (currentSpawner.HasTanks())
			{
				currentSpawner.DeployEnemy();
				enemySpawned = true;
			}

			numExploredSpawners++;
			currentEnemySpawner = (currentEnemySpawner + 1) % EnemySpawners.Count;
		}
	}

	// Update is called once per frame
	void Update()
	{

	}

	private void CompleteLevel()
	{
		SceneManager.LoadScene("MainMenuScene");
	}
}
