using System.Collections.Generic;
using NUnit.Framework;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
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
	[SerializeField]
	private GameObject FullMainMenuWidget;
	[SerializeField]
	private TMP_Dropdown LevelDropdown;
	[SerializeField]
	private GameConfiguration Configuration;
	[SerializeField]
	private AudioManager AudioManager;

	private NumPlayers numPlayers;


	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
    {
        OnePlayerButton.onClick.AddListener(OnePlayerClick);
		TwoPlayerButton.onClick.AddListener(TwoPlayerClick);
		ConstructionButton.onClick.AddListener(ConstructionClick);
		AudioManager.PlayMainMenuBackground();
	}

	private void OnFinishLoadLevel(LevelObject levelObject)
	{
		Configuration.SetLevelTiles(levelObject.tiles);
		Configuration.SetTanks(levelObject.tanks);
		Configuration.SetNumPlayers(numPlayers);

		SceneManager.LoadScene("GameScene");
	}

	private void OnePlayerClick()
	{
		numPlayers = NumPlayers.SinglePlayer;
		var selectedLevel = int.Parse(LevelDropdown.options[LevelDropdown.value].text);
		LevelLoader.GetInstance().LoadLevel(selectedLevel, OnFinishLoadLevel);
	}

	private void TwoPlayerClick()
	{
		numPlayers = NumPlayers.TwoPlayers;
		var selectedLevel = int.Parse(LevelDropdown.options[LevelDropdown.value].text);
		LevelLoader.GetInstance().LoadLevel(selectedLevel, OnFinishLoadLevel);
	}

	private void ConstructionClick()
	{
		ConstructionWidget.SetActive(true);
		FullMainMenuWidget.SetActive(false);
	}
	
	// Update is called once per frame
	void Update()
    {
        
    }
}
