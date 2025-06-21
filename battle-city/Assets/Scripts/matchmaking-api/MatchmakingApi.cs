
using System;
using System.Collections;
using System.Text;
using UnityEngine.Networking;
using UnityEngine;

public class MatchmakingApi: MonoBehaviour
{
	private string urlBase = "http://localhost:1234/api";

	[Serializable]
	public class AuthData
	{
		public string token;
	}

	[Serializable]
	public class LoginData
	{
		public string username;
		public string password;
	}

	public void Login(string login, string password, Action<string> action)
	{
		var url = urlBase + "/auth/login";
		var loginData = new LoginData() { username = login, password = password };
		StartCoroutine(PostRequest(url, loginData, action));
	}

	private IEnumerator PostRequest(string uri, LoginData loginData, Action<string> action)
	{
		//string jsonData = "{\"title\":\"foo\",\"body\":\"bar\",\"userId\":1}";
		string jsonData = JsonUtility.ToJson(loginData);
		byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonData);

		var request = new UnityWebRequest(uri, "POST")
		{
			uploadHandler = new UploadHandlerRaw(bodyRaw),
			downloadHandler = new DownloadHandlerBuffer()
		};
		request.SetRequestHeader("Content-Type", "application/json");

		yield return request.SendWebRequest();

		if (request.result == UnityWebRequest.Result.ConnectionError ||
			request.result == UnityWebRequest.Result.ProtocolError)
		{
			Debug.LogError(request.error);
		}
		else
		{
			string json = request.downloadHandler.text;

			AuthData result;
			try
			{
				result = JsonUtility.FromJson<AuthData>(json);

				if (result != null)
				{
					action?.Invoke(result.token);
				}
				else
				{
					Debug.LogWarning(string.Format("Failed to deserialize JSON ({0}).", jsonData));
				}
			}
			catch (System.Exception e)
			{
				Debug.LogError("Deserialization error: " + e.Message);
			}
		}
	}

	private IEnumerator GetRequest(string uri)
	{
		using (UnityWebRequest request = UnityWebRequest.Get(uri))
		{
			yield return request.SendWebRequest();

			if (request.result == UnityWebRequest.Result.ConnectionError ||
				request.result == UnityWebRequest.Result.ProtocolError)
			{
				Debug.LogError(request.error);
			}
			else
			{
				Debug.Log(request.downloadHandler.text);
			}
		}
	}
}
