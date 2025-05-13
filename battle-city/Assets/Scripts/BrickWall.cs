using UnityEngine;

public class BrickWall : MonoBehaviour
{
    [SerializeField]
    private GameObject DestructionWallPrefab;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
		GetComponent<Damageable>().OnDeath += OnDeath;
	}

	private void OnDeath(object sender, TankBase tank)
	{
		Destroy(Instantiate(DestructionWallPrefab, transform.position, Quaternion.identity), 2f);
	}
}
