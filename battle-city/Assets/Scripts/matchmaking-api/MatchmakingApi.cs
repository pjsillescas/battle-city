
using System;
using System.Collections.Generic;

public class MatchmakingApi: HttpApi
{
	private const string URL_BASE = "http://localhost:8080/api";

	private string token = null;

	[Serializable]
	public class AuthDataDTO
	{
		public string token;
	}

	[Serializable]
	public class LoginDataDTO
	{
		public string username;
		public string password;
	}

	[Serializable]
	public class PlayerDTO
	{
		public int id;
		public string username;
	}

	[Serializable]
	public class GameDTO
	{
		public int id;

		public string name;

		public DateTime creationDate;

		public PlayerDTO host;
		public PlayerDTO guest;
	}

	[Serializable]
	public class GameExtendedDTO: GameDTO
	{
		public string joinCode;
	}

	[Serializable]
	public class ErrorDTO
	{
		public DateTime timestamp;
		public string message;
	}

	[Serializable]
	public class GameListWrapper
	{
		public List<GameDTO> elements;
	}

	[Serializable]
	public class GameInputDTO
	{
		public string joinCode;
	}

	private void ExtractToken(AuthDataDTO data)
	{
		token = data.token;
	}

	public void Login(string login, string password, Action<string> onLogin)
	{
		var url = URL_BASE + "/auth/login";
		var loginData = new LoginDataDTO() { username = login, password = password };
		StartCoroutine(PostRequest<AuthDataDTO, LoginDataDTO>(url, loginData, (data) => {
			token = data.token;
			onLogin(token);
		}, new()));
	}

	public void Signup(string login, string password, Action<PlayerDTO> action)
	{
		var url = URL_BASE + "/auth/signup";
		var loginData = new LoginDataDTO() { username = login, password = password };
		StartCoroutine(PutRequest(url, loginData, action, new()));
	}

	private Dictionary<string, string> GetAuthenticationHeaders()
	{
		return new () { { "Authentication", string.Format("Bearer {0}", token) } };
	}
	public void GetGames(Action<GameListWrapper> action)
	{
		var url = URL_BASE + "/game";
		StartCoroutine(GetRequestList(url, action, GetAuthenticationHeaders()));
	}
	public void GetGame(int gameId, Action<GameDTO> action)
	{
		var url = URL_BASE + string.Format("/game/{0}", gameId);
		StartCoroutine(GetRequest(url, action, GetAuthenticationHeaders()));
	}

	public void AddGame(string joinCode, Action<GameDTO> action)
	{
		var url = URL_BASE + "/game";
		StartCoroutine(PutRequest(url, new GameInputDTO() { joinCode = joinCode }, action, GetAuthenticationHeaders()));
	}
	public void JoinGame(int gameId, Action<GameExtendedDTO> action)
	{
		var url = URL_BASE + string.Format("/game/{0}/join", gameId);
		StartCoroutine(PostRequest(url, action, GetAuthenticationHeaders()));
	}

	public void LeaveGame(int gameId, Action<GameDTO> action)
	{
		var url = URL_BASE + string.Format("/game/{0}/leave", gameId);
		StartCoroutine(PostRequest(url, action, GetAuthenticationHeaders()));
	}

}
