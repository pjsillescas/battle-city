using UnityEngine;

public class Rotator : MonoBehaviour
{
	private const float RotationSpeed = 15f;

	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{
		transform.Rotate(0, RotationSpeed * Time.deltaTime, 0);
	}
}