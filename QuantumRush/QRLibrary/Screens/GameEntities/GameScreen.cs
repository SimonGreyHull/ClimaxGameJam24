﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using QRLibrary.Shapes;

namespace QRLibrary.Screens.GameEntities
{
	internal class GameScreen : IScreen
	{
		private float _SecondsLeft;

		private SpriteFont _font;
		private SpriteBatch _batch;
		private ShapeBatcher _shapeBatcher;

		private Vector2 _mouseInWorldSpace;

		private Terrain _terrain = Terrain.Instance();
		private Camera _camera = new();
		private Player _player = new();
		private SoundEffectInstance _music;

		public Camera Camera { get { return _camera; } }
		public Player Player { get { return _player; } }
		internal GameScreen()
		{
			_SecondsLeft = 15;
			QuantumRush game = QuantumRush.Instance();
			_font = game.Content.Load<SpriteFont>("Font");
			_batch = new SpriteBatch(game.GraphicsDevice);
			_shapeBatcher = new ShapeBatcher(); 
			_music = game.SoundManager.GetLoopableSoundEffectInstance("music");
			_music.Play();
			_terrain.SetPlayer(_player);
		}

		public void Draw(float pSeconds)
		{
			Game game = QuantumRush.Instance();
			game.GraphicsDevice.Clear(Color.HotPink);

			_shapeBatcher.Begin(_camera);

			for (int i = 0; i < Terrain.TERRAIN_COLS; i++)
			{
				for (int j = 0; j < Terrain.TERRAIN_ROWS; j++)
				{
					_shapeBatcher.DrawTriangle(_terrain.CellData[i, j].T1, _terrain.CellData[i, j].Colour);
					_shapeBatcher.DrawTriangle(_terrain.CellData[i, j].T2, _terrain.CellData[i, j].Colour);
					//_shapeBatcher.DrawCircle(_terrain.CellData[i, j].Centre, 2, 3, 1, Color.Pink);
				}
			}

			for(int i = 0; i < _terrain._bulletCount; i++)
			{
				_shapeBatcher.DrawFilledCircle(_terrain._bullets[i].Circle.Position, _terrain._bullets[i].Circle.Radius, 8, Color.DarkViolet);
			}

			for (int i = 0; i < _terrain._enemyCount; i++)
			{
				_shapeBatcher.DrawFilledCircle(_terrain._enemies[i].Circle.Position, _terrain._enemies[i].Circle.Radius, 8, Color.Black);
			}


			// _shapeBatcher.DrawFilledCircle(_player.Circle.Position, _player.Circle.Radius, 32, Color.DarkViolet);

			_shapeBatcher.DrawTriangle(_player.Triangle, Color.DarkViolet);

			_shapeBatcher.DrawFilledCircle(_mouseInWorldSpace, 2, 3, Color.DarkViolet);

			//Vector2 p1 = _player.Position;
			//(TerrainCellData[], int) indices = _terrain.GetSurroundingCells(p1);

			//for(int i = 0; i < indices.Item2; i++)
			//{
			//	_shapeBatcher.DrawLine(p1, indices.Item1[i].Centre, 2, Color.Purple);
			//}

			//for(int i = 0; i < _terrain._enemyCount; i++)
			//{
			//	_shapeBatcher.DrawLine(_terrain._enemies[i].Circle.Position, _player.Position, 2, Color.Red);
			//}

			_shapeBatcher.End();

			_batch.Begin();

			for(int i = 0; i < _terrain._enemySpawners.Length; i++)
			{
				Vector2 positionInWorldSpace = _terrain._enemySpawners[i]._position;
				Vector2 positionInScreenSpace = _camera.ScreenSpaceFromWorldSpace(positionInWorldSpace);
				string time = _terrain._enemySpawners[i]._timeTillSpawn.ToString("0.0");
				_batch.DrawString(_font, time, positionInScreenSpace - _font.MeasureString(time) / 2, Color.Black);
				_batch.DrawString(_font, time, (positionInScreenSpace - _font.MeasureString(time) / 2) + new Vector2(2, 2), Color.White);
			}

			_batch.DrawString(_font, $"Score {_player.Score}", new Vector2(10, 10), Color.Black);
			_batch.DrawString(_font, $"Score {_player.Score}", new Vector2(12, 12), Color.White);
			_batch.End();
		}

		public void Update(float pSeconds)
		{
			_player.movement = Player.Movement.NONE;
			_player.strafe = Player.Strafe.NONE;

			if(Keyboard.GetState().IsKeyDown(Keys.W) && !Keyboard.GetState().IsKeyDown(Keys.S))
			{
				_player.movement = Player.Movement.FORWARD;
			}
			else if(Keyboard.GetState().IsKeyDown(Keys.S) && !Keyboard.GetState().IsKeyDown(Keys.W))
			{
				_player.movement = Player.Movement.BACKWARD;
			}

			if (Keyboard.GetState().IsKeyDown(Keys.A) && !Keyboard.GetState().IsKeyDown(Keys.D))
			{
				_player.strafe = Player.Strafe.LEFT;
			}
			else if (Keyboard.GetState().IsKeyDown(Keys.D) && !Keyboard.GetState().IsKeyDown(Keys.A))
			{
				_player.strafe = Player.Strafe.RIGHT;
			}

			if(Mouse.GetState().LeftButton == ButtonState.Pressed)
			{
				if(_player.CanFire)
				{
					SoundEffectInstance bulletFire = QuantumRush.Instance().SoundManager.GetSoundEffectInstance("shoot");
					bulletFire.Play();
					_terrain.AddBullet(_player.GetBullet());
				}
			}

			_mouseInWorldSpace = _camera.ScreenSpaceToWorldSpace(Mouse.GetState().Position);

			Vector2 heading = _mouseInWorldSpace - _player.Position;
			heading.Normalize();
			_player.Update(pSeconds, heading);
			
			if(_terrain.CheckPlayerCollision(_player))
			{
				_player.PreviousPosition();
			}

			for(int i = 0; i < _terrain._enemySpawners.Length; i++)
			{
				if(_terrain._enemySpawners[i].PlayerCollision(_player))
				{
					//_terrain.UpdateDistanceToLowestSpawner();
				}
			}

			_terrain.UpdateEnemies(pSeconds);
			_terrain.UpdateBullets(pSeconds);

			if (!_player.IsAlive)
			{
				_music.Stop();
				QuantumRush game = QuantumRush.Instance();
				game.ReplaceScreen(new GameOverScreen(_player.Score));
			}

			_camera.LookAt(_player.Position, _player.Heading);
		}
	}
}
