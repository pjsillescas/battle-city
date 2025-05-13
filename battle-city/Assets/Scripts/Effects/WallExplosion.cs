using UnityEngine;

public class WallExplosion : MonoBehaviour
{
	private const int cubesPerAxis = 8;
	private const float delay = 1f;
	private const float force = 300f;
	private const float radius = 2f;

	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
	{
		Invoke("Main", delay);
	}

	private void Main()
	{
		for(var x=0;x<cubesPerAxis;x++)
		{
			for (var y = 0; y < cubesPerAxis; y++)
			{
				for (var z = 0; z < cubesPerAxis; z++)
				{
					CreateCube(new Vector3(x,y,z));
				}

			}

		}
	}

	private void CreateCube(Vector3 coordinates)
	{
		var cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
		Renderer rd = cube.GetComponent<Renderer>();
		rd.material = GetComponent<Renderer>().material;
		cube.transform.localScale = transform.localScale / cubesPerAxis;

		var firstCube = transform.position - transform.localScale / 2 + cube.transform.localScale / 2;
		cube.transform.position = firstCube + Vector3.Scale(coordinates, cube.transform.localScale);

		var rigidBody = cube.AddComponent<Rigidbody>();
		rigidBody.AddExplosionForce(force, transform.position, radius);
	}
}
