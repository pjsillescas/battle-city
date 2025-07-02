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

	private MatchmakingApi api;

	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
	{
		api = FindFirstObjectByType<MatchmakingApi>();
		ButtonCreateGame.onClick.AddListener(ButtonCreateGameClick);
		ButtonRefreshGames.onClick.AddListener(ButtonRefreshGamesClick);
		ButtonBack.onClick.AddListener(ButtonBackClick);
	}

	private void CreateGame()
	{
		var joinCode = "abc";
		api.AddGame(joinCode, OnGameCreated);
	}
	private void ButtonCreateGameClick()
	{
		CreateGame();
	}
	private void ButtonRefreshGamesClick()
	{
		CreateGame();
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
		api = FindFirstObjectByType<MatchmakingApi>();
		api.GetGames(OnGamesReceived);
	}

	private void OnGamesReceived(MatchmakingApi.GameListWrapper wrapper)
	{
		wrapper.elements.ForEach(game => {
			var row = Instantiate(GameRowPrefab, Content).GetComponent<GameRow>();
			row.Load(game, api, OnGameJoined);
		});
	}

	private void OnGameJoined(MatchmakingApi.GameExtendedDTO gameDto)
	{
		Debug.Log($"joined to game with code {gameDto.joinCode}");
	}
}
