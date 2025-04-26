using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameConfiguration", menuName = "Data/GameConfiguration")]
public class GameConfiguration : ScriptableObject
{
	public List<List<TileType>> tileType;
}
