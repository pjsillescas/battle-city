using UnityEngine;

public class StopWatchPickup : Pickup
{
	protected override void Apply(Tank tank)
	{
		GetGameManager().StopWatch();
	}
}
