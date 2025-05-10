using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupManager : MonoBehaviour
{
	[SerializeField]
	private List<GameObject> PickupPrefabs;

	[SerializeField]
	private float SpawningCooldown = 10f;

	private List<Vector3> spawnPoints;

	private Coroutine spawnCoroutine;

	private static PickupManager instance;
	public static PickupManager GetInstance() => instance;

	private void Awake()
	{
		if(instance != null)
		{
			Debug.LogError("Pickup Manager duplicated");
			return;
		}

		instance = this;
	}

	public void Initialize(List<Vector3> spawnPoints)
	{
		this.spawnPoints = spawnPoints;

		spawnCoroutine = StartCoroutine(SpawnCoroutine());
	}

	private void OnDestroy()
	{
		if (spawnCoroutine != null)
		{
			StopCoroutine(spawnCoroutine);
		}
	}

	private T GetRandomElement<T>(List<T> list)
	{
		return list[Random.Range(0, list.Count)];
	}

	private void SpawnPickup()
	{
		//var pickup = PickupPrefabs[Random.Range(0, PickupPrefabs.Count)];
		//var position = spawnPoints[Random.Range(0, spawnPoints.Count)];

		var pickup = GetRandomElement(PickupPrefabs);
		var position = GetRandomElement(spawnPoints);

		Instantiate(pickup, position, Quaternion.identity);
	}

	private IEnumerator SpawnCoroutine()
	{
		while(true)
		{
			yield return new WaitForSeconds(SpawningCooldown);

			SpawnPickup();
		}
	}
}
