using UnityEngine;

public abstract class Pickup : MonoBehaviour
{
	protected abstract void Apply(Tank tank);
	private GameManager gameManager;

	private void Start()
	{
		gameManager = FindFirstObjectByType<GameManager>();
	}

	protected GameManager GetGameManager() => gameManager;

	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.TryGetComponent(out Tank tank))
		{
			Apply(tank);
			Destroy(gameObject, 0.2f);
		}
	}
}
