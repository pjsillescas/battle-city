using UnityEngine;

public class GameManager : MonoBehaviour
{
	[SerializeField]
	private LevelFileLoader LevelLoader;

	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
	{
		LevelLoader.LoadLevel(1);
	}

	// Update is called once per frame
	void Update()
	{

	}
}
