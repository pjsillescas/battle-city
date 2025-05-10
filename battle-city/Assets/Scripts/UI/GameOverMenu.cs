using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverMenu : MonoBehaviour
{
	[SerializeField]
	private Button MainMenuButton;
	[SerializeField]
	private GameObject WidgetPanel;

	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
	{
		MainMenuButton.onClick.AddListener(MainMenuButtonClick);

		GameManager.OnLevelStart += OnLevelStart;
		GameManager.OnGameOver += OnGameOver;
		GameManager.OnLevelComplete += OnGameOver;
	}

	private void OnLevelStart(object sender, EventArgs args)
	{
		//Time.timeScale = 1f;
		WidgetPanel.SetActive(false);
	}

	private void OnGameOver(object sender, EventArgs args)
	{
		//Time.timeScale = 0f;
		WidgetPanel.SetActive(true);
	}

	private void MainMenuButtonClick()
	{
		SceneManager.LoadScene("MainMenuScene");
	}

	// Update is called once per frame
	void Update()
	{

	}
}
