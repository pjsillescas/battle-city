using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameRow : MonoBehaviour
{
	[SerializeField]
	private TextMeshProUGUI GameTitleText;
	[SerializeField]
	private Button JoinButton;

	private MatchmakingApi api;
	private int gameId;
	private Action<MatchmakingApi.GameExtendedDTO> action;

	private void Start()
	{
		JoinButton.onClick.AddListener(JoinButtonClick);
	}

	public void Load(MatchmakingApi.GameDTO game, MatchmakingApi api, Action<MatchmakingApi.GameExtendedDTO> action)
	{
		GameTitleText.text = game.name;
		gameId = game.id;
		this.api = api;
		this.action = action;
	}

	public void JoinButtonClick()
	{
		api.JoinGame(gameId, action);
	}
}
