using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelDiagramWidget : MonoBehaviour
{
	[SerializeField]
	private Button ButtonPlay;
	[SerializeField]
	private Button ButtonBack;
	[SerializeField]
	private Button ButtonReset;
	[SerializeField]
	private TMP_InputField BasicTanksInputField;
	[SerializeField]
	private TMP_InputField StrikeTanksInputField;
	[SerializeField]
	private TMP_InputField MediumTanksInputField;
	[SerializeField]
	private TMP_InputField HeavyTanksInputField;

	[SerializeField]
	private GameObject MainMenuWidget;
	[SerializeField]
	private GameConfiguration Configuration;

	private static LevelDiagramWidget instance = null;

	private TileType selectedTileType;
	private ButtonTileTypeSelect selectedButton;

	public static LevelDiagramWidget GetInstance() => instance;

	public TileType GetSelectedTileType() => selectedTileType;

	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
	{
		if (instance != null && instance.gameObject != null)
		{
			Debug.LogError("LevelDiagram duplicated!");
			return;
		}

		instance = this;
		ButtonTileTypeSelect.OnTileTypeSelect += OnTileTypeSelect;
		ButtonPlay.onClick.AddListener(ButtonPlayClick);
		ButtonBack.onClick.AddListener(ButtonBackClick);
		ButtonReset.onClick.AddListener(ButtonResetClick);
	}

	private void ButtonResetClick()
	{
		LevelDiagram.GetInstance().ResetLevel();
	}

	private void ButtonBackClick()
	{
		gameObject.SetActive(false);
		MainMenuWidget.SetActive(true);
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
			TileType.Player1Spawn => 7,
			TileType.Player2Spawn => 8,
			TileType.EnemySpawn => 9,
			_ => 0,
		};
	}

	private void DebugLevel(List<List<TileType>> level)
	{
		var level2 = level.Select(row => row.Select(TileTypeToInt).ToList()).ToList();

		var str = string.Join("\n", level2.Select(row => string.Join("", row.ToArray())).ToList());

		Debug.Log(str);

	}

	private int ParseInt(string strInt)
	{
		try
		{
			return int.Parse(strInt);
		}
		catch (Exception)
		{
			return 0;
		}
	}

	private void ButtonPlayClick()
	{
		var levelTiles = LevelDiagram.GetInstance().GetLevelDiagram();
		var tanks = new Tanks() {
			basic = ParseInt(BasicTanksInputField.text),
			strike = ParseInt(StrikeTanksInputField.text),
			medium = ParseInt(MediumTanksInputField.text),
			heavy = ParseInt(HeavyTanksInputField.text),
		};
		Configuration.SetLevelTiles(levelTiles);
		Configuration.SetTanks(tanks);
		DebugLevel(levelTiles);
		//SceneManager.LoadScene("GameScene",LoadSceneMode.Single);
	}

	private void OnTileTypeSelect(object sender, TileType tileType)
	{
		if (this.selectedButton != null)
		{
			selectedButton.Deselect();
		}

		selectedTileType = tileType;
		Debug.Log($"Selected tiletype {selectedTileType}");
		selectedButton = sender as ButtonTileTypeSelect;

		selectedButton.Select();
	}

	// Update is called once per frame
	void Update()
	{

	}
}
