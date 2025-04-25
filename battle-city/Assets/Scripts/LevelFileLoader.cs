using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine;

public class LevelFileLoader : MonoBehaviour
{
	public static EventHandler<List<List<int>>> OnLevelLoaded;

	private static LevelFileLoader instance = null;

	public int CurrentLevel;
	private List<List<int>> tiles;

	[Serializable]
	public class JsonLevelObject
	{
		public List<string> lines;
	}

	public List<List<int>> GetLevel() => tiles;
	public static LevelFileLoader GetInstance() => instance;

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
			var fileName = $"Levels/Level-{level}.txt";
			var jsonLoader = GetComponent<JsonLoader>();
			jsonLoader.Load(fileName, OnJsonLoad);
		}
		else
		{
			OnLevelLoaded?.Invoke(this, tiles);
		}
	}

	private void OnJsonLoad(string json)
	{
		var levelLines = JsonUtility.FromJson<JsonLevelObject>(json);

		tiles = levelLines.lines.Select(line => line.Split("").Select(c => c[0] - '0').ToList()).ToList();
		
		OnLevelLoaded?.Invoke(this, tiles);
	}
}
