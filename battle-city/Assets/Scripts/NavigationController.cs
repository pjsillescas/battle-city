using Unity.AI.Navigation;
using UnityEngine;

public class NavigationController : MonoBehaviour
{
	[SerializeField]
	private NavMeshSurface surface;

	private static NavigationController instance = null;

	public static NavigationController GetInstance() => instance;
	
	private void Awake()
	{
		if (instance != null)
		{
			Debug.LogError("Navigation duplicated");
			return;
		}

		instance = this;
	}

	public void RebuildNavMesh()
	{
		surface.BuildNavMesh();
	}

}
