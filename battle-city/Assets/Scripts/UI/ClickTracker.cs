using UnityEngine;

public class ClickTracker : MonoBehaviour
{
	public bool IsClickHeld { get; private set; }

	private InputActions inputActions;

	void Awake()
	{
		inputActions = new InputActions();
	}

	void OnEnable()
	{
		inputActions.UI.Click.started += ctx => { IsClickHeld = true; Debug.Log("holding click"); };
		inputActions.UI.Click.canceled += ctx => { IsClickHeld = false; Debug.Log("click released"); };
		inputActions.Enable();
	}

	void OnDisable()
	{
		inputActions.Disable();
	}
}
