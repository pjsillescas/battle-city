using UnityEngine;

public class GameManager : MonoBehaviour
{
	[SerializeField]
	private LevelFileLoader LevelLoader;
	[SerializeField]
	private GameConfiguration Configuration;
	[SerializeField]
	private bool loadDebugLevel = false;

	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
	{
		if (loadDebugLevel)
		{
			LevelLoader.LoadLevel(1);
		}
		else
		{
			LevelLoader.LoadLevel(Configuration.levelTiles);
		}
	}

	// Update is called once per frame
	void Update()
	{

	}
}
