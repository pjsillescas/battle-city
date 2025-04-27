using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelDiagramRow : MonoBehaviour
{
	[SerializeField]
	private List<LevelDiagramCell> RowCells;
	
	private List<TileType> TileTypes;

	private Dictionary<TileType, Color> tileColors;

	public void Initialize()
	{
		
		TileTypes.Clear();

		var defaultTileType = TileType.Floor;
		for (var k = 0; k < RowCells.Count; k++)
		{
			TileTypes.Add(defaultTileType);
			RowCells[k].SetTileType(defaultTileType);
		}
	}

	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
	{
		tileColors = new()
		{
			{ TileType.Floor, Color.black },
			{ TileType.BrickWall, Color.red},
			{ TileType.SteelWall, Color.gray},
			{ TileType.Base, Color.yellow},
			{ TileType.River, Color.cyan},
			{ TileType.TreeCover, Color.green },
			{ TileType.SlipperyFloor, Color.white },
		};

		TileTypes = new();
		RowCells = new List<LevelDiagramCell>(GetComponentsInChildren<LevelDiagramCell>());
		RowCells.Sort(CellCompare);

		RowCells.ForEach(image => {
			//image
		});

		Initialize();
	}

	private int CellCompare(LevelDiagramCell cell1, LevelDiagramCell cell2)
	{
		var index1 = GetCellIndex(cell1);
		var index2 = GetCellIndex(cell2);
		return (index1 == index2) ? 0 : (index1 < index2 ? -1 : 1);
	}

	private int GetCellIndex(LevelDiagramCell cell)
	{
		var cellName = cell.gameObject.name;
		var index = int.Parse(new List<string>(cellName.Split("_")).LastOrDefault());

		return index;
	}

	public List<TileType> GetRow()
	{
		return RowCells.Select(cell => cell.GetTileType()).ToList();
	}



	// Update is called once per frame
	void Update()
	{

	}
}
