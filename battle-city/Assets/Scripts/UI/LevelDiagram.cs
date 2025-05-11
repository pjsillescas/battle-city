using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class LevelDiagram : MonoBehaviour
{
	[SerializeField]
	private static LevelDiagram instance = null;

	public static LevelDiagram GetInstance() => instance;

    private List<LevelDiagramRow> Rows;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
		if(instance != null && instance.gameObject != null)
		{
			return;
		}

		instance = this;
		Rows = new List<LevelDiagramRow>(GetComponentsInChildren<LevelDiagramRow>());
		Rows.Sort(RowCompare);
	}

	private int RowCompare(LevelDiagramRow row1, LevelDiagramRow row2)
	{
		var index1 = GetRowIndex(row1);
		var index2 = GetRowIndex(row2);
		return (index1 == index2) ? 0 : (index1 < index2 ? -1 : 1);
	}

	private int GetRowIndex(LevelDiagramRow row)
	{
		var rowName = row.gameObject.name;
		var index = int.Parse(new List<string>(rowName.Split("_")).LastOrDefault());

		return index;
	}

	public List<List<TileType>> GetLevelDiagram()
	{
		return Rows.Select(row => row.GetRow()).ToList();
	}

	public void ResetLevel()
	{
		Rows.ForEach(row => row.Initialize());
	}


	// Update is called once per frame
	void Update()
    {
        
    }
}
