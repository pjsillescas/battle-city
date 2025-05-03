using System;
using UnityEngine;

public class Damageable : MonoBehaviour
{
	[SerializeField]
	private int Shield;

	public event EventHandler<TankBase> OnDeath;

	public void ApplyDamage(int damage, TankBase source)
	{
		if (damage >= Shield)
		{
			OnDeath?.Invoke(this, source);
			Destroy(gameObject);
		}
	}
}
