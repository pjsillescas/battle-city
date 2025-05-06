using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameConfiguration", menuName = "Data/GameConfiguration")]
public class GameConfiguration : ScriptableObject
{
	public List<List<TileType>> levelTiles;
	public Tanks tanks;

	public void SetTanks(Tanks tanks)
	{
		this.tanks = tanks;
	}

	public void SetLevelTiles(List<List<TileType>> levelTiles)
	{
		this.levelTiles = levelTiles;
	}
}
