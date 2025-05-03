using System;
using UnityEngine;
using UnityEngine.Rendering;

public class Damageable : MonoBehaviour
{
	[SerializeField]
	private int Hits = 1;

	public event EventHandler<TankBase> OnDeath;

	public void ApplyDamage(int damage, TankBase source)
	{
		Hits = Math.Max(0, Hits - damage);
		if (Hits == 0)
		{
			OnDeath?.Invoke(this, source);
			Destroy(gameObject);
		}
	}
}
