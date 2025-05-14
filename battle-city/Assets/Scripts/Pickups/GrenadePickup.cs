using UnityEngine;

public class GrenadePickup : Pickup
{
	protected override void Apply(Tank tank)
	{
		GameManager.GetInstance().KillAllEnemies();
	}
}
