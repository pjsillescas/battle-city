using System.Collections.Generic;
using System;
using UnityEngine;
using System.Linq;
using System.Threading.Tasks;

public class LevelLoader : MonoBehaviour
{
	public static EventHandler<List<List<int>>> OnLevelLoaded;

	private static LevelLoader instance = null;

	Action<LevelObject> onFinishLoad;

	
	private readonly Dictionary<TileType, int> serialTileTypes = new() {
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
	
	public static LevelLoader GetInstance() => instance;

	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
	{
		if (instance != null)
		{
			Debug.LogError("LevelLoader duplicated");
		}

		instance = this;
	}

	public void LoadLevel(int level, Action<LevelObject> onFinishLoad)
	{
		this.onFinishLoad = onFinishLoad;
		var fileName = $"Levels/Level-{level:D2}.json";
		var jsonLoader = GetComponent<JsonLoader>();
		jsonLoader.Load(fileName, OnJsonLoad);
	}

	private void OnJsonLoad(string json)
	{
		var jsonLevelObject = JsonUtility.FromJson<JsonLevelObject>(json);
		var tiles = jsonLevelObject.lines.Select(line => new List<char>(line.ToCharArray()).Select(c => c - '0').Select(col => serialTileTypes.FirstOrDefault(x => x.Value == col).Key).ToList()).ToList();
		onFinishLoad(new LevelObject() { tiles = tiles, tanks = jsonLevelObject.tanks });
	}
}
