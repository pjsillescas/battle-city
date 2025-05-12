using System;
using System.Collections;
using UnityEngine;

public class Missile : MonoBehaviour
{
	private const float SPEED = 8.0f;

	[SerializeField]
	private GameObject ExplosionPrefab;

	private float lifetime = 3f;
	private Team team;

	public event EventHandler OnMissileDestroy;

	private int damage;
	private TankBase shooter;
	
	private Vector3 forward;
	private Coroutine coroutine;

	public void SetDamage(int damage)
	{
		this.damage = damage;
	}

	public void SetTeam(Team team)
	{
		this.team = team;
	}
	
	public void SetShooter(TankBase shooter)
	{
		this.shooter = shooter;
	}

	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
	{
		damage = 1;
		forward = transform.forward;
		coroutine = StartCoroutine(WaitLifetime());
	}

	private IEnumerator WaitLifetime()
	{
		yield return new WaitForSeconds(lifetime);
		Destroy(gameObject);
		yield return null;
	}

	// Update is called once per frame
	void Update()
	{
		// Debug.Log($"({transform.rotation.x}, {transform.rotation.y}, {transform.rotation.z}) => ({transform.forward.x}, {transform.forward.y}, {transform.forward.z})");
		transform.position += SPEED * Time.deltaTime * forward;
	}

	private void DestroySelf()
	{
		StopCoroutine(coroutine);
		Destroy(Instantiate(ExplosionPrefab, transform.position, Quaternion.identity), 2f);
		Destroy(gameObject);
	}
	private void OnTriggerEnter(Collider other)
	{
		if (other.TryGetComponent(out Damageable damageable))
		{
			if (damageable.CanBeDamaged(team))
			{
				damageable.ApplyDamage(damage, shooter);
				DestroySelf();
			}
		}
		else
		{
			DestroySelf();
		}
	}
	/*
	private void OnCollisionEnter(Collision collision)
	{
		if (collision.collider.TryGetComponent(out Damageable damageable))
		{
			if(damageable.CanBeDamaged(team))
			{
				damageable.ApplyDamage(damage, shooter);
				StopCoroutine(coroutine);
				Destroy(gameObject);
			}
		}
		else
		{
			StopCoroutine(coroutine);
			Destroy(gameObject);
		}
	}
	*/
	private void OnDestroy()
	{
		//StopCoroutine(coroutine);
		OnMissileDestroy?.Invoke(this, EventArgs.Empty);
	}
}
