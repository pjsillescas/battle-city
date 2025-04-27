using UnityEngine;
using UnityEngine.UI;

public class MainMenuWidget : MonoBehaviour
{
    [SerializeField]
    private Button OnePlayerButton;
	[SerializeField]
	private Button TwoPlayerButton;
	[SerializeField]
	private Button ConstructionButton;
	[SerializeField]
	private GameObject ConstructionWidget;


	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
    {
        OnePlayerButton.onClick.AddListener(OnePlayerClick);
		TwoPlayerButton.onClick.AddListener(TwoPlayerClick);
		ConstructionButton.onClick.AddListener(ConstructionClick);
	}

	private void OnePlayerClick()
	{
		;
	}

	private void TwoPlayerClick()
	{
		;
	}

	private void ConstructionClick()
	{
		;
	}
	// Update is called once per frame
	void Update()
    {
        
    }
}
