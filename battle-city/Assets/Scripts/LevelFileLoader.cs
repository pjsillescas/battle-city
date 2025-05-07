using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine;
using Unity.AI.Navigation;

public class LevelFileLoader : MonoBehaviour
{
	[SerializeField]
	private GameObject FloorPrefab;
    [SerializeField]
    private GameObject RiverPrefab;
    [SerializeField]
    private GameObject BrickWallPrefab;
    [SerializeField]
    private GameObject SteelWallPrefab;
    [SerializeField]
    private GameObject SlipperyFloorPrefab;
    [SerializeField]
    private GameObject TreeCoverPrefab;
    [SerializeField]
    private GameObject BasePrefab;
	[SerializeField]
	private GameObject PlayerTankPrefab;
	[SerializeField]
	private GameObject PlayerSpawnPoint;
	[SerializeField]
	private GameObject EnemySpawnerPrefab;
	[SerializeField]
	private NavMeshSurface NavigationSurface;

	public static EventHandler<LevelObject> OnLevelLoaded;

	private static LevelFileLoader instance = null;

	private int CurrentLevel;
	//private List<List<int>> tiles;
	private PlayerController playerController;

	[Serializable]
	public class JsonLevelObject
	{
		public List<string> lines;
	}

	//public List<List<int>> GetLevel() => tiles;
	public static LevelFileLoader GetInstance() => instance;

	private readonly GameObject[,] levelObjects = new GameObject[26, 26];

	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
	{
		if (instance != null)
		{
			Debug.LogError("LevelFileLoader duplicated");
		}

		instance = this;
		var playerControllers = Resources.FindObjectsOfTypeAll<PlayerController>();
		playerController = playerControllers.FirstOrDefault();

		if (playerController == null)
		{
			Debug.LogError("Player controller not found");
		}
	}

	public void LoadLevel(int level)
	{
		if (CurrentLevel != level /*|| tiles == null*/)
		{
			CurrentLevel = level;
			var fileName = $"Levels/Level-{level:D2}.json";
			var jsonLoader = GetComponent<JsonLoader>();
			jsonLoader.Load(fileName, OnJsonLoad2);
		}
		else
		{
			//OnLevelLoaded?.Invoke(this, tiles);
		}
	}

	public void LoadLevel(LevelObject levelObject)
	{
		BuildLevel(levelObject);
	}

	private GameObject InstantiateLevelObject(GameObject prefab, float x, float z, Vector3 translation)
	{

		var gameObject = Instantiate(prefab, new Vector3(x, 0, z), new Quaternion(0, 0f, 0f, 0f));
		gameObject.transform.Translate(translation);
		gameObject.transform.Rotate(-90, 0, 0);
		gameObject.transform.parent = NavigationSurface.transform;
		return gameObject;
	}

	private Dictionary<TileType, int> serialTileTypes = new () {
		{ TileType.Floor, 0 },
		{ TileType.BrickWall, 1 },
		{ TileType.SteelWall, 2 },
		{ TileType.SlipperyFloor, 3 },
		{ TileType.Base, 4 },
		{ TileType.TreeCover, 5 },
		{ TileType.River, 6 },
		{ TileType.Player1Spawn, 7 },
		{ TileType.Player2Spawn, 8 },
		{ TileType.EnemySpawn, 9 },
	};
	
	private int TileTypeToInt(TileType tileType)
	{
		return tileType switch
		{
			TileType.Floor => 0,
			TileType.BrickWall => 1,
			TileType.SteelWall => 2,
			TileType.SlipperyFloor => 3,
			TileType.Base => 4,
			TileType.TreeCover => 5,
			TileType.River => 6,
			TileType.Player1Spawn => 7,
			TileType.Player2Spawn => 8,
			TileType.EnemySpawn => 9,
			_ => 0,
		};
	}

	private void OnJsonLoad2(string json)
	{
		var levelLines = JsonUtility.FromJson<JsonLevelObject>(json);
		var tiles = levelLines.lines.Select(line => new List<char>(line.ToCharArray()).Select(c => c - '0').Select(col => serialTileTypes.FirstOrDefault(x => x.Value == col)).ToList()).ToList();

		/*
		tiles.ForEach(row => {
			row.ForEach(col => {
				TileType val = TileType.Floor;

				if(serialTileTypes.ContainsValue(col))
				{
					val = serialTileTypes.FirstOrDefault(x => x.Value == col).Key;
				}

			});
		});
		*/
	}

	private void BuildLevel(LevelObject levelObject)
	{
		List<TileType> tilesWithFloor = new() { TileType.TreeCover, TileType.BrickWall, TileType.SteelWall };
		const float TILE_WIDTH = 1.0f;
		const float TILE_HEIGHT = 1.0f;

		int nx = 0;
		int nz = levelObject.tiles.Count - 1;
		float x = 0;
		float z = nz * TILE_HEIGHT;
		levelObject.tiles.ForEach(row => {
			x = 0;
			nx = 0;
			row.ForEach(col => {
				var translation = (col == TileType.Base) ? new Vector3(0.5f * TILE_WIDTH, 0, -0.5f * TILE_HEIGHT) : Vector3.zero;
				var prefab = col switch
				{
					// 0 => FloorPrefab,
					TileType.BrickWall => BrickWallPrefab,
					TileType.SteelWall => SteelWallPrefab,
					TileType.SlipperyFloor => SlipperyFloorPrefab,
					TileType.Base => BasePrefab,
					TileType.River => RiverPrefab,
					TileType.TreeCover => TreeCoverPrefab,
					_ => FloorPrefab,
				};


				/*
				var gameObject = Instantiate(prefab, new Vector3(x, 0, z), new Quaternion(0, 0f, 0f, 0f));

                gameObject.transform.Translate(translation);
                gameObject.transform.Rotate(-90, 0, 0);
				*/

				var gameObject = InstantiateLevelObject(prefab, x, z, translation);
				if (tilesWithFloor.Contains(col))
				{
					InstantiateLevelObject(FloorPrefab, x, z, translation);
				}

				levelObjects[nx, nz] = gameObject;
				nx++;
				x = nx * TILE_WIDTH;
			});

			nz--;
			z = TILE_HEIGHT * nz;
		});

		// Place tanks
		nx = 0;
		nz = levelObject.tiles.Count - 1;
		x = 0;
		z = nz * TILE_HEIGHT;
		levelObject.tiles.ForEach(row => {
			nx = 0;
			x = 0;
			row.ForEach(col => {

				switch(col)
				{
					case TileType.Player1Spawn:
						var spawnPosition1 = new Vector3(x, 0, z + 1.5f * TILE_HEIGHT);
						var spawnPoint = Instantiate(PlayerSpawnPoint, spawnPosition1, Quaternion.identity);
						GameManager.GetInstance().RegisterPlayer1SpawnPoint(spawnPoint.GetComponent<SpawnPoint>());
						break;
					case TileType.Player2Spawn:
						var spawnPosition2 = new Vector3(x, 0, z + 1.5f * TILE_HEIGHT);
						var spawnPoint2 = Instantiate(PlayerSpawnPoint, spawnPosition2, Quaternion.identity);
						GameManager.GetInstance().RegisterPlayer2SpawnPoint(spawnPoint2.GetComponent<SpawnPoint>());
						break;
					case TileType.EnemySpawn:
						//var enemySpawnerPosition = new Vector3(x, 0, z - 1.5f * TILE_HEIGHT);
						var enemySpawnerPosition = new Vector3(x, 0, z - 0.5f * TILE_HEIGHT);
						var enemySpawner = Instantiate(EnemySpawnerPrefab, enemySpawnerPosition, Quaternion.identity);
						GameManager.GetInstance().RegisterEnemySpawner(enemySpawner.GetComponent<EnemySpawner>());
						break;
					default:
						break;
				}

				nx++;
				x = nx * TILE_WIDTH;
			});

			nz--;
			z = TILE_HEIGHT * nz;
		});

		OnLevelLoaded?.Invoke(this, levelObject);
	}
}
