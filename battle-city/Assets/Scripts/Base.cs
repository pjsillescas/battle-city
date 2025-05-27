using UnityEngine;

public class Base : MonoBehaviour
{
	private GameManager gameManager;

	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
	{
		GetComponent<Damageable>().OnDeath += OnDeath;
		gameManager = FindFirstObjectByType<GameManager>();
	}

	private void OnDeath(object sender, TankBase tank)
	{
		gameManager.GameOver();
	}
}
