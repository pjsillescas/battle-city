using System.Threading.Tasks;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport.Relay;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Relay;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RelayManager : MonoBehaviour
{
	public int maxConnections = 4;

	async void Start()
	{
		await UnityServices.InitializeAsync();

		if (!AuthenticationService.Instance.IsSignedIn)
		{
			await AuthenticationService.Instance.SignInAnonymouslyAsync();
			Debug.Log("Signed in as: " + AuthenticationService.Instance.PlayerId);
		}
	}

	public async Task<string> StartHost()
	{
		var allocation = await RelayService.Instance.CreateAllocationAsync(maxConnections - 1);
		var joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);

		var server = allocation.RelayServer;

		var relayServerData = new RelayServerData(
			host: server.IpV4,
			port: (ushort)server.Port,
			allocationId: allocation.AllocationIdBytes,
			connectionData: allocation.ConnectionData,
			hostConnectionData: allocation.ConnectionData, // host connects to self
			key: allocation.Key,
			isSecure: true
		);

		NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);
		NetworkManager.Singleton.StartHost();
		NetworkManager.Singleton.SceneManager.LoadScene("MultiplayerGameScene", LoadSceneMode.Single);

		return joinCode;
	}

	public async void JoinGame(string joinCode)
	{
		var joinAllocation = await RelayService.Instance.JoinAllocationAsync(joinCode);
		var server = joinAllocation.RelayServer;

		var relayServerData = new RelayServerData(
			host: server.IpV4,
			port: (ushort)server.Port,
			allocationId: joinAllocation.AllocationIdBytes,
			connectionData: joinAllocation.ConnectionData,
			hostConnectionData: joinAllocation.HostConnectionData,
			key: joinAllocation.Key,
			isSecure: true
		);

		NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);
		NetworkManager.Singleton.StartClient();
	}
}
