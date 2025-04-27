using System;
using UnityEngine;
using UnityEngine.UI;

public class ButtonTileTypeSelect : MonoBehaviour
{
	public static event EventHandler<TileType> OnTileTypeSelect;

    [SerializeField]
    private TileType tileType;

	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
    {
        GetComponent<Button>().onClick.AddListener(ButtonSelect);
    }

    private void ButtonSelect()
    {
        OnTileTypeSelect?.Invoke(this, tileType);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
