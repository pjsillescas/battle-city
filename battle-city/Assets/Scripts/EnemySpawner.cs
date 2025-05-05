using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
	private List<TankEnemy> tanks;
	
	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
	{
		tanks = new();
		
	}

	private IEnumerator WaitCooldown()
	{
		while (tanks.Count > 0)
		{
			yield return new WaitForSeconds(10f);

			Debug.Log("spawn enemy");
			var tank = tanks[0];
			tanks.RemoveAt(0);
			tank.transform.position = transform.position;
			tank.gameObject.SetActive(true);
		}

		yield return null;

	}

	public void LoadTanks(List<TankEnemy> tanks)
	{
		this.tanks = tanks;
		StartCoroutine(WaitCooldown());
	}

	// Update is called once per frame
	void Update()
	{

	}
}
