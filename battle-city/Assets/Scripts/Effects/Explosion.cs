using UnityEngine;

public class Explosion : MonoBehaviour
{
	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
	{
		//var particles = GetComponentsInChildren<ParticleSystem>();
		//Debug.Log("boom");
		FindFirstObjectByType<AudioManager>().PlayExplosion();
	}

	// Update is called once per frame
	void Update()
	{

	}
}
