using UnityEngine;

public class ShieldPickup : Pickup
{
	protected override void Apply(Tank tank)
	{
		tank.StartShield();
	}
}
