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

	private int gameId;
	private Action<MatchmakingApi.GameExtendedDTO> action;

	private void Start()
	{
		JoinButton.onClick.AddListener(JoinButtonClick);
	}

	public void Load(MatchmakingApi.GameDTO game, Action<MatchmakingApi.GameExtendedDTO> action)
	{
		GameTitleText.text = game.name;
		gameId = game.id;
		//this.api = api;
		this.action = action;
	}

	public void JoinButtonClick()
	{
		var api = FindAnyObjectByType<MatchmakingApi>();
		api.JoinGame(gameId, action);
	}
}
