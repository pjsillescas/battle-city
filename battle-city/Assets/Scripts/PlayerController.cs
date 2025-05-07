using System.Xml.Schema;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	[SerializeField]
	private Tank ControlledTank;

	private InputActions actions;

	private const float XMin = 0f;
	private const float XMax = 25f;
	
	private const float ZMin = 0f;
	private const float ZMax = 25f;

	void Awake()
	{
		actions = new InputActions();
		actions.Enable();
	}

	// Update is called once per frame
	void Update()
	{
		if(ControlledTank == null)
		{
			return;
		}

		var input = actions.Player.Move.ReadValue<Vector2>();

		if (input != null)
		{
			//Debug.Log($"({input.x},{input.y})");
			ControlledTank.Move(input);
		}
		var position = ControlledTank.transform.position;
		position.x = Mathf.Clamp(position.x, XMin, XMax);
		position.z = Mathf.Clamp(position.z, ZMin, ZMax);
		ControlledTank.transform.position = position;

		if (actions.Player.Attack.WasPressedThisFrame())
		{
			ControlledTank.LaunchMissile();
		}
	}

	public void SetPawnTank(Tank tank)
	{
		ControlledTank = tank;
	}
}
