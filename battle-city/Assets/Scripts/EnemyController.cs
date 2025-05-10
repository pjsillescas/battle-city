using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
	private const int SHOOTING_THRESHOLD = 95;

	//[SerializeField]
	private const float ShootCooldownSeconds = 3f;

	private NavMeshAgent agent;
	private TankEnemy tank;
	private List<Vector3> navigablePoints;

	private bool canShoot;

	private void SetNewDestination()
	{
		//var destination = transform.position + 10 * new Vector3(Random.Range(0f, 1f), 0, Random.Range(0f, 1f));
		var k = Random.Range(0, navigablePoints.Count);
		var destination = navigablePoints[k];
		Debug.Log($"going to ({destination.x},{destination.y},{destination.z})");

		agent.SetDestination(destination);
	}
	
	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
	{
		agent = GetComponent<NavMeshAgent>();
		tank = GetComponent<TankEnemy>();

		agent.speed = tank.GetSpeed();
		canShoot = true;

		navigablePoints = GameManager.GetInstance().GetNavigablePoints();

		SetNewDestination();
	}

	// Update is called once per frame
	void Update()
	{
		if (agent.remainingDistance <= agent.stoppingDistance)
		{
			SetNewDestination();
		}

		if (canShoot)
		{
			var rng = Random.Range(0, 99);
			//Debug.Log($"rng: {rng}");
			if (rng >= SHOOTING_THRESHOLD)
			{
				//Debug.Log($"shoot {rng}");

				tank.LaunchMissile();
				
				canShoot = false;
				StartCoroutine(ShootCooldown());
			}
		}
	}

	private IEnumerator ShootCooldown()
	{
		yield return new WaitForSeconds(ShootCooldownSeconds);

		canShoot = true;

		yield return null;
	}

}
