using Unity.Netcode;
using UnityEngine;

public class SingletonEnsurer : MonoBehaviour
{
	public void Awake()
	{
		var managers = FindObjectsByType<NetworkManager>(FindObjectsSortMode.None);
		if (managers.Length > 1)
		{
			Debug.LogWarning("Duplicate NetworkManager found. Destroying this one.");
			Destroy(gameObject);
			return;
		}

		DontDestroyOnLoad(gameObject);
	}
}
