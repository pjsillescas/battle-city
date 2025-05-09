using System;
using System.Collections.Generic;
using UnityEngine;

public class RemainingTanksWidget : MonoBehaviour
{

	[SerializeField]
	private GameObject TokenPrefab;
	
	private int numEnemies;

	private List<GameObject> tokens;
	void Awake()
	{
		tokens = new();
	}

	// Start is called once before the first execution of Update after the MonoBehaviour is created
	private void Start()
	{
		GameManager.OnEnemiesSet += OnEnemiesSet;
		GameManager.OnEnemyKilled += OnEnemyKilled;
	}

	private void OnEnemiesSet(object sender, int numEnemies)
	{
		if (TokenPrefab != null)
		{
			tokens.Clear();
			for (int k = 0; k < numEnemies; k++)
			{
				var token = Instantiate(TokenPrefab);
				token.transform.parent = transform;
				tokens.Add(token);
			}
		}
	}

	private void OnEnemyKilled(object sender, EventArgs args)
	{
		if (tokens.Count > 0)
		{
			var k = tokens.Count - 1;
			var token = tokens[k];
			tokens.RemoveAt(k);
			Destroy(token);
		}
	}
}
