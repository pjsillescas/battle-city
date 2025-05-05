using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
	private const float SHOOTING_THRESHOLD = 0.8f;

	private NavMeshAgent agent;
	private TankEnemy tank;

	private void SetNewDestination()
	{
		var destination = transform.position + 10 * new Vector3(Random.Range(0f, 1f), 0, Random.Range(0f, 1f));
		Debug.Log($"going to ({destination.x},{destination.y},{destination.z})");

		agent.SetDestination(destination);
	}
	
	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
	{
		agent = GetComponent<NavMeshAgent>();
		tank = GetComponent<TankEnemy>();

		agent.speed = tank.GetSpeed();

		SetNewDestination();
	}

	// Update is called once per frame
	void Update()
	{
		if (agent.remainingDistance <= agent.stoppingDistance)
		{
			SetNewDestination();
		}

		if (Random.Range(0f, 1f) >= SHOOTING_THRESHOLD)
		{
			tank.LaunchMissile();
		}
	}
}
