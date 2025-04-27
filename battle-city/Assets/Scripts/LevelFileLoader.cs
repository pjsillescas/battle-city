using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine;

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

    public static EventHandler<List<List<int>>> OnLevelLoaded;

	private static LevelFileLoader instance = null;

	private int CurrentLevel;
	private List<List<int>> tiles;

	[Serializable]
	public class JsonLevelObject
	{
		public List<string> lines;
	}

	public List<List<int>> GetLevel() => tiles;
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
	}

	public void LoadLevel(int level)
	{
		if (CurrentLevel != level || tiles == null)
		{
			CurrentLevel = level;
			var fileName = $"Levels/Level-{level:D2}.json";
			var jsonLoader = GetComponent<JsonLoader>();
			jsonLoader.Load(fileName, OnJsonLoad2);
		}
		else
		{
			OnLevelLoaded?.Invoke(this, tiles);
		}
	}

	public void LoadLevel(List<List<TileType>> tiles)
	{
		BuildLevel(tiles);
	}

	private GameObject InstantiateLevelObject(GameObject prefab, float x, float z, Vector3 translation)
	{

		var gameObject = Instantiate(prefab, new Vector3(x, 0, z), new Quaternion(0, 0f, 0f, 0f));

		gameObject.transform.Translate(translation);
		gameObject.transform.Rotate(-90, 0, 0);
		return gameObject;
	}

	private void OnJsonLoad(string json)
	{
		const float TILE_WIDTH = 1.0f;
        const float TILE_HEIGHT = 1.0f;
        var levelLines = JsonUtility.FromJson<JsonLevelObject>(json);

		tiles = levelLines.lines.Select(line => new List<char>(line.ToCharArray()).Select(c => c - '0').ToList()).ToList();

		float x = 0;
		float z = 0;
        int nx = 0;
        int nz = 0;
        tiles.ForEach(row => {
			x = 0;
			nx = 0;
			row.ForEach(col => {
				var translation = (col == 4) ? new Vector3(0.5f, 0, 0.5f) : Vector3.zero;
				var prefab = col switch
				{
					// 0 => FloorPrefab,
					1 => BrickWallPrefab,
					2 => SteelWallPrefab,
					3 => SlipperyFloorPrefab,
					4 => BasePrefab,
					5 => RiverPrefab,
					6 => TreeCoverPrefab,
					_ => FloorPrefab,
				};


				/*
				var gameObject = Instantiate(prefab, new Vector3(x, 0, z), new Quaternion(0, 0f, 0f, 0f));

                gameObject.transform.Translate(translation);
                gameObject.transform.Rotate(-90, 0, 0);
				*/

				var gameObject = InstantiateLevelObject(prefab, x, z, translation);
				if(col == 6)
				{
					InstantiateLevelObject(FloorPrefab, x, z, translation);
				}

                levelObjects[nx, nz] = gameObject;
				x += TILE_HEIGHT;
				nx++;
            });
			
			z += TILE_WIDTH;
			nz++;
        });

		OnLevelLoaded?.Invoke(this, tiles);
    }

	private Dictionary<TileType, int> serialTileTypes = new () {
		{ TileType.Floor, 0 },
		{ TileType.BrickWall, 1 },
		{ TileType.SteelWall, 2 },
		{ TileType.SlipperyFloor, 3 },
		{ TileType.Base, 4 },
		{ TileType.TreeCover, 5 },
		{ TileType.River, 6 },
		{ TileType.PlayerSpawn, 7 },
		{ TileType.EnemySpawn, 8 },
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
			TileType.PlayerSpawn => 7,
			TileType.EnemySpawn => 8,
			_ => 0,
		};
	}

	private void OnJsonLoad2(string json)
	{
		const float TILE_WIDTH = 1.0f;
		const float TILE_HEIGHT = 1.0f;
		var levelLines = JsonUtility.FromJson<JsonLevelObject>(json);

		tiles = levelLines.lines.Select(line => new List<char>(line.ToCharArray()).Select(c => c - '0').ToList()).ToList();

		float x = 0;
		float z = 0;
		int nx = 0;
		int nz = 0;
		tiles.ForEach(row => {
			x = 0;
			nx = 0;
			row.ForEach(col => {
				var translation = (col == 4) ? new Vector3(0.5f, 0, 0.5f) : Vector3.zero;
				TileType val = TileType.Floor;

				if(serialTileTypes.ContainsValue(col))
				{
					val = serialTileTypes.FirstOrDefault(x => x.Value == col).Key;
				}

			});
		});

		OnLevelLoaded?.Invoke(this, tiles);
	}

	private void BuildLevel(List<List<TileType>> tiles)
	{
		const float TILE_WIDTH = 1.0f;
		const float TILE_HEIGHT = 1.0f;

		float x = 0;
		float z = 0;
		int nx = 0;
		int nz = 0;
		tiles.ForEach(row => {
			x = 0;
			nx = 0;
			row.ForEach(col => {
				var translation = (col == TileType.Base) ? new Vector3(0.5f, 0, 0.5f) : Vector3.zero;
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
				if (col == TileType.TreeCover)
				{
					InstantiateLevelObject(FloorPrefab, x, z, translation);
				}

				levelObjects[nx, nz] = gameObject;
				x += TILE_HEIGHT;
				nx++;
			});

			z += TILE_WIDTH;
			nz++;
		});

		// OnLevelLoaded?.Invoke(this, tiles);
	}
}
