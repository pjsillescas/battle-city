using UnityEngine;

public class OneUpPickup : Pickup
{
	[SerializeField]
	private int LivesToAdd = 1;
	
	protected override void Apply(Tank tank)
	{
		GameManager.GetInstance().AddLives(LivesToAdd);
	}
}
