using System;
using System.Collections.Generic;
using UnityEngine;

public class RemainingTanksWidget : MonoBehaviour
{

	[SerializeField]
	private GameObject TokenPrefab;
	
	private List<GameObject> tokens;
	void Awake()
	{
		tokens = new();
	}

	// Start is called once before the first execution of Update after the MonoBehaviour is created
	private void Start()
	{
		Debug.Log("remainingtankswidget start");
		GameManager.GetInstance().OnEnemiesSet -= OnEnemiesSet;
		GameManager.GetInstance().OnEnemyKilled -= OnEnemyKilled;

		GameManager.GetInstance().OnEnemiesSet += OnEnemiesSet;
		GameManager.GetInstance().OnEnemyKilled += OnEnemyKilled;
	}

	private void OnDestroy()
	{
		GameManager.GetInstance().OnEnemiesSet -= OnEnemiesSet;
		GameManager.GetInstance().OnEnemyKilled -= OnEnemyKilled;

		for(var k = 0; k<  transform.childCount; k++)
		{
			var child = transform.GetChild(k);
			Destroy(child);
		}
	}

	private void OnEnemiesSet(object sender, int numEnemies)
	{
		if (TokenPrefab != null)
		{
			Debug.Log($"instantiating {numEnemies} enemy tokens");
			tokens.Clear();
			for (int k = 0; k < numEnemies; k++)
			{
				var token = Instantiate(TokenPrefab);
				token.transform.SetParent(transform);
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
