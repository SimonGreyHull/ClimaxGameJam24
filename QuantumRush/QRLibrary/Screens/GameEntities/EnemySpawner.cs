using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

using System.Text;

namespace QRLibrary.Screens.GameEntities
{
	internal class EnemySpawner
	{
		public float _timeTillSpawn { get; private set; }
		private float _spawnInterval;
		public Vector2 _position { get; private set; }

		private TerrainCellData _cell;

		public EnemySpawner(TerrainCellData cell, float spawnInterval, float firstSpawn)
		{
			_cell = cell;
			_position = _cell.Centre;
			_spawnInterval = spawnInterval;
			_timeTillSpawn = firstSpawn;
		}

		public void Update(float seconds)
		{
			_timeTillSpawn -= seconds;

			if (_timeTillSpawn < 0 )
			{
				Terrain.Instance().AddEnemy(new Enemy(_position));
				_timeTillSpawn += _spawnInterval;
			}
		}

		public void PlayerCollision(Player player)
		{
			if(_cell.T1.IntersectsCircle(player.Circle)||
				_cell.T2.IntersectsCircle(player.Circle))
			{
				_timeTillSpawn = _spawnInterval;
			}
		}
	}
}
