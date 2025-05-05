using System.Data.Common;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshAgentTest : MonoBehaviour
{
    private NavMeshAgent agent;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
		
        var destination = transform.position + 10 * new Vector3(Random.Range(0f, 1f), 0, Random.Range(0f, 1f));
		Debug.Log($"going to ({destination.x},{destination.y},{destination.z})");

		agent.SetDestination(destination);
	}

	// Update is called once per frame
	void Update()
    {
        if(agent.remainingDistance <= agent.stoppingDistance)
        {
			var destination = transform.position + 10 * new Vector3(Random.Range(0f, 1f), 0, Random.Range(0f, 1f));
			Debug.Log($"going to ({destination.x},{destination.y},{destination.z})");

			agent.SetDestination(destination);
		}
	}
}
