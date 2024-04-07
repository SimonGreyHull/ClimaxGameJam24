using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

using System.Text;

namespace QRLibrary.Screens.GameEntities
{
	internal class EnemySpawner
	{
		private float _timeTillSpawn;
		private float _spawnInterval;
		private Vector2 _position;

		public EnemySpawner(Vector2 position, float spawnInterval, float offset)
		{
			_position = position;
			_spawnInterval = spawnInterval;
			_timeTillSpawn = spawnInterval + offset;
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
	}
}
