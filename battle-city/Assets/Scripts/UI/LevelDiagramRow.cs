using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class LevelDiagramRow : MonoBehaviour
{
	[SerializeField]
	private List<Image> RowImages;
	
	private List<TileType> TileTypes;

	private Dictionary<TileType, Color> tileColors;

	public void Initialize()
	{
		
		TileTypes.Clear();

		var defaultTileType = TileType.Floor;
		for (var k = 0; k < RowImages.Count; k++)
		{
			TileTypes.Add(defaultTileType);
			RowImages[k].color = tileColors[defaultTileType];
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
		var images = new List<Image>(GetComponentsInChildren<Image>());
		images.Sort(ImageCompare);

		RowImages = new();
		images.ForEach(RowImages.Add);

		Initialize();
	}

	private int ImageCompare(Image image1, Image image2)
	{
		var index1 = GetImageIndex(image1);
		var index2 = GetImageIndex(image2);
		return (index1 == index2) ? 0 : (index1 < index2 ? -1 : 1);
	}

	private int GetImageIndex(Image image)
	{
		var imageName = image.gameObject.name;
		var index = int.Parse(new List<string>(imageName.Split("_")).LastOrDefault());

		return index;
	}

	// Update is called once per frame
	void Update()
	{

	}
}
