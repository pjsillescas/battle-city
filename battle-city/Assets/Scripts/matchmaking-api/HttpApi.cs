using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class HttpApi : MonoBehaviour
{
	protected IEnumerator PostRequest<T, U>(string uri, U data, Action<T> action, Dictionary<string, string> headers)
	{
		Debug.Log($"POST {uri}");
		string jsonData = JsonUtility.ToJson(data);
		byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonData);

		var request = new UnityWebRequest(uri, "POST")
		{
			uploadHandler = new UploadHandlerRaw(bodyRaw),
			downloadHandler = new DownloadHandlerBuffer()
		};
		request.SetRequestHeader("Content-Type", "application/json");

		headers.Keys.ToList().ForEach(key => { request.SetRequestHeader(key, headers[key]); });

		yield return request.SendWebRequest();

		if (request.result == UnityWebRequest.Result.ConnectionError ||
			request.result == UnityWebRequest.Result.ProtocolError)
		{
			Debug.LogError(request.error);
		}
		else
		{
			string json = request.downloadHandler.text;

			try
			{
				var result = JsonUtility.FromJson<T>(json);

				if (result != null)
				{
					action?.Invoke(result);
				}
				else
				{
					Debug.LogWarning(string.Format("Failed to deserialize JSON ({0}).", json));
				}
			}
			catch (System.Exception e)
			{
				Debug.LogError("Deserialization error: " + e.Message + $" with json '{json}'");
			}
		}

		yield return null;
	}

	protected IEnumerator PostRequest<T>(string uri, Action<T> action, Dictionary<string, string> headers)
	{
		Debug.Log($"POST {uri}");
		var request = new UnityWebRequest(uri, "POST")
		{
			uploadHandler = new UploadHandlerRaw(null),
			downloadHandler = new DownloadHandlerBuffer()
		};
		request.SetRequestHeader("Content-Type", "application/json");
		headers.Keys.ToList().ForEach(key => { request.SetRequestHeader(key, headers[key]); });

		yield return request.SendWebRequest();

		if (request.result == UnityWebRequest.Result.ConnectionError ||
			request.result == UnityWebRequest.Result.ProtocolError)
		{
			Debug.LogError(request.error);
		}
		else
		{
			try
			{
				string json = request.downloadHandler.text;
				var result = JsonUtility.FromJson<T>(json);

				if (result != null)
				{
					action?.Invoke(result);
				}
				else
				{
					Debug.LogWarning(string.Format("Failed to deserialize JSON ({0}).", json));
				}
			}
			catch (System.Exception e)
			{
				Debug.LogError("Deserialization error: " + e.Message);
			}
		}

		yield return null;
	}

	protected IEnumerator PutRequest<T,U>(string uri, U data, Action<T> action, Dictionary<string, string> headers)
	{
		Debug.Log($"PUT {uri}");
		string jsonData = JsonUtility.ToJson(data);
		byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonData);

		var request = new UnityWebRequest(uri, "PUT")
		{
			uploadHandler = new UploadHandlerRaw(bodyRaw),
			downloadHandler = new DownloadHandlerBuffer()
		};
		request.SetRequestHeader("Content-Type", "application/json");
		headers.Keys.ToList().ForEach(key => { request.SetRequestHeader(key, headers[key]); });


		yield return request.SendWebRequest();

		if (request.result == UnityWebRequest.Result.ConnectionError ||
			request.result == UnityWebRequest.Result.ProtocolError)
		{
			Debug.LogError(request.error);
		}
		else
		{
			string json = request.downloadHandler.text;

			try
			{
				var result = JsonUtility.FromJson<T>(json);

				if (result != null)
				{
					action?.Invoke(result);
				}
				else
				{
					Debug.LogWarning(string.Format("Failed to deserialize JSON ({0}).", json));
				}
			}
			catch (System.Exception e)
			{
				Debug.LogError("Deserialization error: " + e.Message);
			}
		}
		
		yield return null;
	}

	protected IEnumerator PutRequest<T>(string uri, Action<T> action, Dictionary<string, string> headers)
	{
		Debug.Log($"PUT {uri}");
		var request = new UnityWebRequest(uri, "PUT")
		{
			uploadHandler = new UploadHandlerRaw(null),
			downloadHandler = new DownloadHandlerBuffer()
		};
		request.SetRequestHeader("Content-Type", "application/json");
		headers.Keys.ToList().ForEach(key => { request.SetRequestHeader(key, headers[key]); });

		yield return request.SendWebRequest();

		if (request.result == UnityWebRequest.Result.ConnectionError ||
			request.result == UnityWebRequest.Result.ProtocolError)
		{
			Debug.LogError(request.error);
		}
		else
		{
			string json = request.downloadHandler.text;

			try
			{
				var result = JsonUtility.FromJson<T>(json);

				if (result != null)
				{
					action?.Invoke(result);
				}
				else
				{
					Debug.LogWarning(string.Format("Failed to deserialize JSON ({0}).", json));
				}
			}
			catch (System.Exception e)
			{
				Debug.LogError("Deserialization error: " + e.Message);
			}
		}
		
		yield return null;
	}
	protected IEnumerator GetRequestList<T>(string uri, Action<T> action, Dictionary<string, string> headers)
	{
		Debug.Log($"GET list {uri}");
		using (UnityWebRequest request = UnityWebRequest.Get(uri))
		{
			headers.Keys.ToList().ForEach(key => { request.SetRequestHeader(key, headers[key]); });
			yield return request.SendWebRequest();

			if (request.result == UnityWebRequest.Result.ConnectionError ||
				request.result == UnityWebRequest.Result.ProtocolError)
			{
				Debug.LogError(request.error);
			}
			else
			{
				var json = "{ \"elements\": " + request.downloadHandler.text + " }";
				
				var result = JsonUtility.FromJson<T>(json);

				if (result != null)
				{
					action?.Invoke(result);
				}
				else
				{
					Debug.LogWarning(string.Format("Failed to deserialize JSON ({0}).", json));
				}
			}
		}
		
		yield return null;
	}

	protected IEnumerator GetRequest<T>(string uri, Action<T> action, Dictionary<string, string> headers)
	{
		Debug.Log($"GET {uri}");
		using UnityWebRequest request = UnityWebRequest.Get(uri);
		headers.Keys.ToList().ForEach(key => { request.SetRequestHeader(key, headers[key]); });
		yield return request.SendWebRequest();

		if (request.result == UnityWebRequest.Result.ConnectionError ||
			request.result == UnityWebRequest.Result.ProtocolError)
		{
			Debug.LogError(request.error);
		}
		else
		{
			Debug.Log($"GET {request.downloadHandler.text}");
			var json = request.downloadHandler.text;
			var result = JsonUtility.FromJson<T>(json);

			if (result != null)
			{
				action?.Invoke(result);
			}
			else
			{
				Debug.LogWarning(string.Format("Failed to deserialize JSON ({0}).", json));
			}
		}
		
		yield return null;
	}
}
