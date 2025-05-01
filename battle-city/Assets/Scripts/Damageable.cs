using UnityEngine;

public class Damageable : MonoBehaviour
{
	[SerializeField]
	private int Shield;

	public void ApplyDamage(int damage)
	{
		if (damage >= Shield)
		{
			Destroy(gameObject);
		}
	}
}
