using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameConfiguration", menuName = "Data/GameConfiguration")]
public class GameConfiguration : ScriptableObject
{
	public List<List<TileType>> levelTiles;
	public Tanks tanks;
	public bool enableMusic;
	public NumPlayers numPlayers;

	public void SetEnableMusic(bool enableMusic)
	{
		this.enableMusic = enableMusic;
	}

	public bool GetEnableMusic() => enableMusic;
	public void SetNumPlayers(NumPlayers numPlayers)
	{
		this.numPlayers = numPlayers;
	}

	public NumPlayers GetNumPlayers() => numPlayers;

	public void SetTanks(Tanks tanks)
	{
		this.tanks = tanks;
	}

	public void SetLevelTiles(List<List<TileType>> levelTiles)
	{
		this.levelTiles = levelTiles;
	}
}
