using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
	public static event EventHandler<int> OnEnemiesSet;
	public static event EventHandler OnEnemyKilled;
	public static event EventHandler<int> OnPlayerLivesChanged;
	public static event EventHandler OnGameOver;
	public static event EventHandler OnLevelComplete;
	public static event EventHandler OnLevelStart;

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
	private int playerLives = 3;

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
		Debug.Log("inicializando enemyspawners");
		currentEnemySpawner = 0;
		LevelFileLoader.OnLevelLoaded += OnLevelLoaded;

		LevelLoader.LoadLevel(new LevelObject() { tiles = Configuration.levelTiles, tanks = Configuration.tanks });
		/*
		if (!loadDebugLevel)
		{
			LevelLoader.LoadLevel(new LevelObject() { tiles = Configuration.levelTiles, tanks = Configuration.tanks });
		}
		else
		{
			OnLevelLoaded(null, null);
		}
		*/
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
		Debug.Log($"added {enemySpawner.name} there are {EnemySpawners.Count} enemy spawners");
	}

	private void OnLevelLoaded(object sender, LevelObject levelObject)
	{
		Debug.Log("level loaded");
		NavigationController.GetInstance().RebuildNavMesh();

		if (Player1SpawnPoint != null && Player1Prefab != null && PlayerController1)
		{
			OnPlayerLivesChanged?.Invoke(this, playerLives);
			
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

		//OnLevelStart?.Invoke(this, EventArgs.Empty);
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

		if (EnemySpawners.Count == 0)
		{
			return;
		}

		var tanks = new List<TankEnemy>();
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
			if (EnemySpawners[k] != null)
			{
				EnemySpawners[k].LoadTanks(loads[k]);
				EnemySpawners[k].DeployEnemy();
			}
			else
			{
				Debug.Log($"el enemy spawner {k} es null");
			}
		}

		OnEnemiesSet?.Invoke(this, enemiesRemaining);
	}

	private void Player1Respawn(object sender, TankBase tank)
	{
		playerLives--;
		if (playerLives > 0)
		{
			OnPlayerLivesChanged?.Invoke(this, playerLives);

			var player1Pawn = Instantiate(Player1Prefab, Player1SpawnPoint.transform.position, Player1SpawnPoint.transform.rotation);
			player1Pawn.GetComponent<Damageable>().OnDeath += Player1Respawn;
			PlayerController1.SetPawnTank(player1Pawn.GetComponent<Tank>());
		}
		else
		{
			GameOver();
		}
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
		OnEnemyKilled?.Invoke(this, EventArgs.Empty);
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
	private void GameOver()
	{
		SceneManager.LoadScene("MainMenuScene");
		OnGameOver?.Invoke(this, EventArgs.Empty);
	}

	private void CompleteLevel()
	{
		OnLevelComplete?.Invoke(this, EventArgs.Empty);
	}
}
