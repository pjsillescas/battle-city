using System.Collections;
using System.Xml.Schema;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	[SerializeField]
	private Tank ControlledTank;

	private InputActions actions;
	private bool isActivated;

	private const float XMin = 0f;
	private const float XMax = 25f;
	
	private const float ZMin = 0f;
	private const float ZMax = 25f;

	void Awake()
	{
		isActivated = false;
		actions = new InputActions();
		actions.Enable();
	}

	// Update is called once per frame
	void Update()
	{
		if(ControlledTank == null || !isActivated)
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
		StartCoroutine(PauseStart());
	}

	private const float START_PAUSE_COOLDOWN = 1f;

	private IEnumerator PauseStart()
	{
		Debug.Log("pause player");
		ControlledTank.Deactivate();
		isActivated = false;
		yield return new WaitForSeconds(START_PAUSE_COOLDOWN);

		ControlledTank.Activate();
		isActivated = true;
		Debug.Log("activate player");
		yield return null;
	}

}
