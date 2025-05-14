using UnityEngine;

public class Base : MonoBehaviour
{
	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
	{
		GetComponent<Damageable>().OnDeath += OnDeath;
	}

	private void OnDeath(object sender, TankBase tank)
	{
		GameManager.GetInstance().GameOver();
	}
}
