using NUnit.Framework;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class LevelDiagramWidget : MonoBehaviour
{
	[SerializeField]
	private Button ButtonPlay;

	private static LevelDiagramWidget instance = null;

	private TileType selectedTileType;
	public static LevelDiagramWidget GetInstance() => instance;

	public TileType GetSelectedTileType() => selectedTileType;

	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
	{
		if (instance != null)
		{
			Debug.LogError("LevelDiagram duplicated!");
			return;
		}

		instance = this;
		ButtonTileTypeSelect.OnTileTypeSelect += OnTileTypeSelect;
		ButtonPlay.onClick.AddListener(ButtonPlayClick);
	}

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
			_ => 0,
		};
	}

	private void ButtonPlayClick()
	{
		var level = LevelDiagram.GetInstance().GetLevelDiagram();

		var level2 = level.Select(row => row.Select(TileTypeToInt).ToList()).ToList();

		var str = string.Join("\n", level2.Select(row => string.Join("",row.ToArray())).ToList());

		Debug.Log(str);
	}

	private void OnTileTypeSelect(object sender, TileType tileType)
	{
		selectedTileType = tileType;
		Debug.Log($"Selected tiletype {selectedTileType}");
	}

	// Update is called once per frame
	void Update()
	{

	}
}
