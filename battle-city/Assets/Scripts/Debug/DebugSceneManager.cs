using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugSceneManager : MonoBehaviour
{
	[SerializeField]
	private List<GameObject> TankPrefabs;
	[SerializeField]
	private EnemySpawner EnemySpawn;

	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
	{
		NavigationController.GetInstance().RebuildNavMesh();
		Debug.Log("navmesh built");
		StartCoroutine(LoadTanks());
	}

	private IEnumerator LoadTanks()
	{
		yield return new WaitForSeconds(10);
		Debug.Log("loading tanks");
		var tanks = new List<TankEnemy>();

		for (int i = 0; i < 20; i++)
		{
			var tank = Instantiate(TankPrefabs[Random.Range(0, TankPrefabs.Count)], Vector3.zero, Quaternion.identity);
			tank.SetActive(false);
			tanks.Add(tank.GetComponent<TankEnemy>());
		}

		EnemySpawn.LoadTanks(tanks);
		
		yield return null;
	}

	// Update is called once per frame
	void Update()
	{

	}
}
