using UnityEngine;

public class WallExplosion : MonoBehaviour
{
	private const int cubesPerAxisX = 4;
	private const int cubesPerAxisY = 4;
	private const int cubesPerAxisZ = 4;
	private const float delay = 0.2f;
	private const float force = 300f;
	private const float radius = 5f;

	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
	{
		Invoke(nameof(Main), delay);
	}

	private void Main()
	{
		for(var x=0;x<cubesPerAxisX;x++)
		{
			for (var y = 0; y < cubesPerAxisY; y++)
			{
				for (var z = 0; z < cubesPerAxisZ; z++)
				{
					CreateCube(new Vector3(x,y,z));
				}
			}
		}
	}

	private void CreateCube(Vector3 coordinates)
	{
		var cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
		//Destroy(cube.GetComponent<Collider>()); // destroy collider
		Renderer rd = cube.GetComponent<Renderer>();
		rd.material = GetComponent<Renderer>().material;
		float scaleFactor = 100f;
		cube.transform.localScale = new Vector3(
			transform.localScale.x / (cubesPerAxisX * scaleFactor),
			transform.localScale.y / (cubesPerAxisY * scaleFactor),
			transform.localScale.z / (cubesPerAxisZ * scaleFactor));

		var firstCube = transform.position - (transform.rotation * transform.localScale / 2 / scaleFactor + transform.rotation * cube.transform.localScale / 2);
		cube.transform.position = firstCube + transform.rotation * Vector3.Scale(coordinates, cube.transform.localScale);// /scaleFactor;

		var rigidBody = cube.AddComponent<Rigidbody>();
		cube.layer = LayerMask.NameToLayer("Debris");
		//rigidBody.mass = 10f;
		rigidBody.AddExplosionForce(force, transform.position, radius);

		Destroy(cube, 1f);
	}
}
