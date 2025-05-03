using NUnit.Framework;
using System.Collections.Generic;
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

		LevelFileLoader.OnLevelLoaded += OnLevelLoaded;

		if (!loadDebugLevel)
		{
			LevelLoader.LoadLevel(Configuration.levelTiles);
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

	private void OnLevelLoaded(object sender, List<List<TileType>> list)
	{
		Debug.Log("level loaded");

		if(Player1SpawnPoint != null && Player1Prefab != null && PlayerController1)
		{
			var player1Pawn = Instantiate(Player1Prefab, Player1SpawnPoint.transform.position, Player1SpawnPoint.transform.rotation);
			PlayerController1.SetPawnTank(player1Pawn.GetComponent<Tank>());
		}
		
		if (Player2SpawnPoint != null && Player2Prefab != null && PlayerController2)
		{
			var player2Pawn = Instantiate(Player2Prefab, Player2SpawnPoint.transform.position, Player2SpawnPoint.transform.rotation);
			PlayerController2.SetPawnTank(player2Pawn.GetComponent<Tank>());
		}
	}

	// Update is called once per frame
	void Update()
	{

	}
}
