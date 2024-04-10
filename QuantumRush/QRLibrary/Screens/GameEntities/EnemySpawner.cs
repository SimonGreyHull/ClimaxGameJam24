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

		private bool _playerAlreadyInsideSoIgnore = false;

		public EnemySpawner(TerrainCellData cell, float spawnInterval, float firstSpawn)
		{
			_cell = cell;
			_position = _cell.Centre;
			_spawnInterval = spawnInterval;
			_timeTillSpawn = firstSpawn;
		}

		public void ResetWithOffset(float offset)
		{
			_timeTillSpawn = offset;
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

		public bool PlayerCollision(Player player)
		{
			if (_cell.T1.IntersectsCircle(player.Circle) ||
				_cell.T2.IntersectsCircle(player.Circle))
			{
				if (_playerAlreadyInsideSoIgnore)
				{
					return false;
				}
				else
				{
					_playerAlreadyInsideSoIgnore = true;
					player.AddScore((int) (_spawnInterval - _timeTillSpawn));
					_timeTillSpawn = _spawnInterval;
					return true;
				}
			}

			_playerAlreadyInsideSoIgnore = false;
			return false;
		}
	}
}
