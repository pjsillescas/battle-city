using UnityEngine;

public class PlayerController : MonoBehaviour
{
	[SerializeField]
	private Tank ControlledTank;

	private InputActions actions;

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

		if(actions.Player.Attack.WasPressedThisFrame())
		{
			ControlledTank.LaunchMissile();
		}
	}

	public void SetPawnTank(Tank tank)
	{
		ControlledTank = tank;
	}
}
