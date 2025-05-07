using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
	private List<TankEnemy> tanks;
	
	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Awake()
	{
		tanks = new();
		
	}

	private IEnumerator WaitCooldown()
	{
		while (tanks.Count > 0)
		{
			yield return new WaitForSeconds(10f);
			DeployEnemy();
		}

		yield return null;

	}

	public void DeployEnemy()
	{
		if (tanks == null || tanks.Count == 0)
		{
			return;
		}
		
		var tank = tanks[0];
		tanks.RemoveAt(0);
		tank.transform.position = transform.position;
		tank.gameObject.SetActive(true);
	}
	public void LoadTanks(List<TankEnemy> tanks)
	{
		tanks.Clear();
		tanks.ForEach(this.tanks.Add);
		StartCoroutine(WaitCooldown());
	}

	// Update is called once per frame
	void Update()
	{

	}
}
