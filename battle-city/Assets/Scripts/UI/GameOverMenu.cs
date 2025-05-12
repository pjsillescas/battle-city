using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverMenu : MonoBehaviour
{
	[SerializeField]
	private Button MainMenuButton;
	[SerializeField]
	private GameObject WidgetPanel;
	[SerializeField]
	private TextMeshProUGUI TitleText;

	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
	{
		MainMenuButton.onClick.AddListener(MainMenuButtonClick);

		GameManager.GetInstance().OnLevelStart += OnLevelStart;
		GameManager.GetInstance().OnGameOver += OnGameOver;
		GameManager.GetInstance().OnLevelComplete += OnLevelComplete;
	}

	private void OnDestroy()
	{
		
		GameManager.GetInstance().OnLevelStart -= OnLevelStart;
		GameManager.GetInstance().OnGameOver -= OnGameOver;
		GameManager.GetInstance().OnLevelComplete -= OnLevelComplete;

	}

	private void OnLevelStart(object sender, EventArgs args)
	{
		Time.timeScale = 1f;
		WidgetPanel.SetActive(false);
	}

	private void OnGameOver(object sender, EventArgs args)
	{
		Time.timeScale = 0f;
		TitleText.text = "Game Over";
		WidgetPanel.SetActive(true);
		
		var audioManager = FindFirstObjectByType<AudioManager>();
		audioManager.StopBackground();
		audioManager.PlayLoseFanfare();
	}

	private void OnLevelComplete(object sender, EventArgs args)
	{
		Time.timeScale = 0f;
		TitleText.text = "Level Complete!!";
		WidgetPanel.SetActive(true);
		var audioManager = FindFirstObjectByType<AudioManager>();
		audioManager.StopBackground();
		audioManager.PlayWinFanfare();
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
