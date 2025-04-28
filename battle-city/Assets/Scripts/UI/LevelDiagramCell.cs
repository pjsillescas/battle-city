using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelDiagramCell : MonoBehaviour
{
	private readonly Dictionary<TileType, Color> tileColors = new()
		{
			{ TileType.Floor, Color.black },
			{ TileType.BrickWall, Color.red },
			{ TileType.SteelWall, Color.gray },
			{ TileType.Base, Color.magenta },
			{ TileType.River, Color.cyan },
			{ TileType.TreeCover, Color.green },
			{ TileType.SlipperyFloor, Color.white },
			{ TileType.Player1Spawn, Color.yellow },
			{ TileType.Player2Spawn, new Color(1f,0.5f,0) },
			{ TileType.EnemySpawn, Color.blue },
		};
	
	private TileType tileType;
	private Image image;
	private Button button;

	private void Awake()
	{
		image = GetComponent<Image>();
		button = GetComponent<Button>();
		button.onClick.AddListener(ButtonClick);
	}

	private void ButtonClick()
	{
		SetTileType(LevelDiagramWidget.GetInstance().GetSelectedTileType());
	}

	public void SetTileType(TileType tileType)
    {
        this.tileType = tileType;
		image.color = tileColors[tileType];
	}

    public TileType GetTileType() => tileType;
}
