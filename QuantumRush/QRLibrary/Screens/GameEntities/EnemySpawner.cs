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

		public EnemySpawner(Vector2 position, float spawnInterval, float firstSpawn)
		{
			_position = position;
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
	}
}
