using System;
using UnityEngine;
using UnityEngine.Rendering;

public class Damageable : MonoBehaviour
{
	[SerializeField]
	private Team Team = Team.Obstacle;
	[SerializeField]
	private int Hits = 1;

	public event EventHandler<TankBase> OnDeath;
	public bool isInvulnerable = false;

	public void SetIsInvulnerable(bool isInvulnerable)
	{
		this.isInvulnerable = isInvulnerable;
	}

	public Team GetTeam() => Team;

	public void ApplyDamage(int damage, TankBase source)
	{
		Hits = Math.Max(0, Hits - damage);
		if (Hits == 0)
		{
			OnDeath?.Invoke(this, source);
			Destroy(gameObject);
		}
	}

	public bool CanBeDamaged(Team team)
	{
		return !Team.Equals(team) && !isInvulnerable;
	}
}
