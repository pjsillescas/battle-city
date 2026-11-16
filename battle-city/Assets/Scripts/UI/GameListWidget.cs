using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class GameListWidget : MonoBehaviour
{
	[SerializeField]
	private ScrollView GameScrollView;	
	[SerializeField]
	private Transform Content;
	[SerializeField]
	private Transform GameRowPrefab;
	[SerializeField]
	private UnityEngine.UI.Button ButtonCreateGame;
	[SerializeField]
	private UnityEngine.UI.Button ButtonRefreshGames;
	[SerializeField]
	private UnityEngine.UI.Button ButtonBack;
	[SerializeField]
	private GameObject MainMenuWidget;
	[SerializeField]
	private RelayManager RelayManager;

	//private MatchmakingApi api;

	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
	{
		//api = FindFirstObjectByType<MatchmakingApi>();
		ButtonCreateGame.onClick.AddListener(ButtonCreateGameClick);
		ButtonRefreshGames.onClick.AddListener(ButtonRefreshGamesClick);
		ButtonBack.onClick.AddListener(ButtonBackClick);
	}

	private async void CreateGame()
	{
		string joinCode = await RelayManager.StartHost();
		FindAnyObjectByType<MatchmakingApi>().AddGame(joinCode, OnGameCreated);
	}

	private void ButtonCreateGameClick()
	{
		CreateGame();
	}
	private void ButtonRefreshGamesClick()
	{
		// CreateGame();
		Fill();
	}
	private void ButtonBackClick()
	{
		MainMenuWidget.SetActive(true);
		DisableWidget();
	}

	private void OnGameCreated(MatchmakingApi.GameDTO game)
	{
		Debug.Log("game created");
	}

	public void EnableWidget()
	{
		gameObject.SetActive(true);
		Fill();
	}

	public void DisableWidget()
	{
		gameObject.SetActive(false);
	}

	private void ClearList()
	{
		var transforms = Content.GetComponentsInChildren<Transform>();
		if (transforms != null)
		{
			new List<Transform>(transforms).Where(transform => !transform.Equals(Content)).ToList()
				.ForEach(transform =>
			{
				//transform.SetParent(null);
				Destroy(transform.gameObject);
			});
		}
	}

	public void Fill()
	{
		ClearList();
		var api = FindAnyObjectByType<MatchmakingApi>();
		api.GetGames(OnGamesReceived);
	}

	private void OnGamesReceived(MatchmakingApi.GameListWrapper wrapper)
	{
		wrapper.elements.ForEach(game => {
			var row = Instantiate(GameRowPrefab, Content).GetComponent<GameRow>();
			row.Load(game, OnGameJoined);
		});
	}

	private void OnGameJoined(MatchmakingApi.GameExtendedDTO gameDto)
	{
		Debug.Log($"joined to game with code {gameDto.joinCode}");
	}
}
