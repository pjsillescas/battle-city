using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Tank : MonoBehaviour
{
    private float Speed = 5.0f;
    private float THRESHOLD = 0.1f;

    [SerializeField]
    private GameObject TankLevel1;
	[SerializeField]
	private Transform ShootTransform1;
	[SerializeField]
	private GameObject TankLevel2;
	[SerializeField]
	private Transform ShootTransform2;
	[SerializeField]
	private GameObject TankLevel3;
	[SerializeField]
	private Transform ShootTransform3;
	[SerializeField]
	private GameObject TankLevel4;
	[SerializeField]
	private Transform ShootTransform4;
	[SerializeField]
    private GameObject MissilePrefab;

    private int tankLevel = 0;
    private List<GameObject> tankLevels;
    private List<Transform> shootingPoints;
	
    // Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
    {
        tankLevels = new() { TankLevel1, TankLevel2, TankLevel3, TankLevel4, };
		tankLevels = tankLevels.Where(tank => tank != null).ToList();
		SetTankLevel(0);

		shootingPoints = new() { ShootTransform1, ShootTransform2, ShootTransform3, ShootTransform4, };
		shootingPoints = shootingPoints.Where(point => point != null).ToList();

	}

	public void SetTankLevel(int level)
    {
        tankLevels.ForEach(tank => tank.SetActive(false));
        if (level < tankLevels.Count)
        {
            tankLevel = level;
            tankLevels[level].SetActive(true);
        }
    }

	public void Move(Vector2 input)
	{
        var input3D = new Vector3(input.x, 0, input.y);

        if (input3D.sqrMagnitude > THRESHOLD)
        {
            var rotation = new Quaternion();
            rotation.SetLookRotation(input3D, Vector3.up);
			transform.rotation = rotation;

			var displacement = Speed * Time.deltaTime * input3D;
			transform.position += displacement;
        }
	}

    public void LaunchMissile()
    {
        Debug.Log("booom!!");
        var aTransform = shootingPoints[tankLevel];
		var missileObj = Instantiate(MissilePrefab, aTransform.position, aTransform.rotation);
        var missile = missileObj.GetComponent<Missile>();
        missile.SetDamage(1);
        //SetTankLevel((tankLevel + 1) %  tankLevels.Count);
    }

	// Update is called once per frame
	void Update()
    {
        
    }
}
