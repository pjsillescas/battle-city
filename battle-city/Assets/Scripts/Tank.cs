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
	private GameObject TankLevel2;
	[SerializeField]
	private GameObject TankLevel3;
	[SerializeField]
	private GameObject TankLevel4;

    private int tankLevel = 0;
    private List<GameObject> tankLevels;
	
    // Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
    {
        tankLevels = new() { TankLevel1, TankLevel2, TankLevel3, TankLevel4, };
		tankLevels = tankLevels.Where(tank => tank != null).ToList();
		SetTankLevel(0);
    }

    private void SetTankLevel(int level)
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

        SetTankLevel((tankLevel + 1) %  tankLevels.Count);
    }

	// Update is called once per frame
	void Update()
    {
        
    }
}
