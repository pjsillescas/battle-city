using Unity.Netcode;
using UnityEngine;

public class SingletonEnsurer : MonoBehaviour
{
	public void Awake()
	{
		var managers = FindObjectsByType<NetworkManager>();
		if (managers.Length > 1)
		{
			Debug.LogWarning("Duplicate NetworkManager found. Destroying this one.");
			Destroy(gameObject);
			return;
		}

		DontDestroyOnLoad(gameObject);
	}

	private void Start()
	{
		DontDestroyOnLoad(NetworkManager.Singleton.gameObject);
	}
}
