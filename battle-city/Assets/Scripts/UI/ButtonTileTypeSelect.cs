using System;
using UnityEngine;
using UnityEngine.UI;

public class ButtonTileTypeSelect : MonoBehaviour
{
	public static event EventHandler<TileType> OnTileTypeSelect;

    [SerializeField]
    private TileType tileType;

    [SerializeField]
    private Color SelectedColor;
	[SerializeField]
	private Color DeselectedColor;

	private Image backgroundImage;

	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
    {
        GetComponent<Button>().onClick.AddListener(ButtonSelect);
		backgroundImage = GetComponent<Image>();

	}

    private void ButtonSelect()
    {
        OnTileTypeSelect?.Invoke(this, tileType);
    }

	public void Select()
	{
        backgroundImage.color = SelectedColor;
	}

	public void Deselect()
	{
		backgroundImage.color = DeselectedColor;
	}

	// Update is called once per frame
	void Update()
    {
        
    }
}
