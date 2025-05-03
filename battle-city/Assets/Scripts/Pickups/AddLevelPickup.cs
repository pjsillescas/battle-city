using UnityEngine;

public class AddLevelPickup : Pickup
{
	protected override void Apply(Tank tank)
	{
		tank.AddTankLevel();
	}
}
