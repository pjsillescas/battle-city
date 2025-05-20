using JetBrains.Annotations;
using System;
using UnityEngine;

public class TankBase : MonoBehaviour
{
	private Team tankTeam;
	private float speed;
	private float inputThreshold;
	private int maxMissilesLaunched;
	private ParticleSystem particles;
	private Damageable damageable;

	public void SetIsInvulnerable(bool isInvulnerable)
	{
		if (isInvulnerable)
		{
			StartParticles();
		}
		else
		{
			StopParticles();
		}
		
		if(damageable == null)
		{
			damageable = GetComponent<Damageable>();
		}

		damageable.SetIsInvulnerable(isInvulnerable);
	}

	public void SetSpeed(float speed)
	{
		this.speed = speed;
	}

	public void SetTeam(Team team)
	{
		this.tankTeam = team;
	}

	public void SetDamage(float damage)
	{
		//this.damage = damage;
	}

	public float GetSpeed() => speed;
	
	public void SetInputThreshold(float inputThreshold)
	{
		this.inputThreshold = inputThreshold;
	}

	public float GetInputThreshold() => inputThreshold;

	public void SetMaxMissilesLaunched(int maxMissilesLaunched)
	{
		this.maxMissilesLaunched = maxMissilesLaunched;
	}

	public int GetMaxMissilesLaunched() => maxMissilesLaunched;
	
	public void Move(Vector2 input)
	{
		var input3D = new Vector3(input.x, 0, input.y);

		if (input3D.sqrMagnitude > inputThreshold)
		{
			var rotation = new Quaternion();
			rotation.SetLookRotation(input3D, Vector3.up);
			transform.rotation = rotation;

			var displacement = speed * Time.deltaTime * input3D;
			transform.position += displacement;
		}
	}

	protected void ShootMissile(Transform shootingPoint, GameObject MissilePrefab)
	{
		if (maxMissilesLaunched == 0)
		{
			return;
		}

		maxMissilesLaunched--;

		var missileObj = Instantiate(MissilePrefab, shootingPoint.position, shootingPoint.rotation);
		var missile = missileObj.GetComponent<Missile>();
		missile.SetShooter(this);
		missile.SetTeam(tankTeam);
		missile.OnMissileDestroy += OnMissileDestroy;
	}

	public void OnMissileDestroy(object sender, EventArgs args)
	{
		maxMissilesLaunched++;
	}

	protected void SetParticles(ParticleSystem particles)
	{
		this.particles = particles;
	}

	public void StartParticles()
	{
		if (particles != null)
		{
			particles.Play();
		}
	}

	public void StopParticles()
	{
		if (particles != null)
		{
			particles.Stop();
		}
	}

	public virtual void Deactivate()
	{
		//particles.Play();
		SetIsInvulnerable(true);
	}

	public virtual void Activate()
	{
		//particles.Stop();
		SetIsInvulnerable(false);
	}
}
