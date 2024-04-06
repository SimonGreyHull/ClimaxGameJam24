using Microsoft.Xna.Framework;
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

		private Terrain _terrain = new();
		private Camera _camera = new();
		private Player _player = new();

		public Camera Camera { get { return _camera; } }
		public Player Player { get { return _player; } }
		internal GameScreen()
		{
			_SecondsLeft = 15;
			QuantumRush game = QuantumRush.Instance();
			_font = game.Content.Load<SpriteFont>("Font");
			_batch = new SpriteBatch(game.GraphicsDevice);
			_shapeBatcher = new ShapeBatcher();
		}

		public void Draw(float pSeconds)
		{
			Game game = QuantumRush.Instance();
			game.GraphicsDevice.Clear(Color.Turquoise);

			_shapeBatcher.Begin(_camera);

			for (int i = 0; i < Terrain.TERRAIN_COLS; i++)
			{
				for (int j = 0; j < Terrain.TERRAIN_ROWS; j++)
				{
					_shapeBatcher.DrawTriangle(_terrain._triangles[2 * i, 2 * j], _terrain.Colours[i, j]);
					_shapeBatcher.DrawTriangle(_terrain._triangles[2 * i + 1, 2 * j + 1], _terrain.Colours[i, j]);
				}
			}

			for(int i = 0; i < _terrain._bulletCount; i++)
			{
				_shapeBatcher.DrawCircle(_terrain._bullets[i].Circle.Position, _terrain._bullets[i].Circle.Radius, 8, 2, Color.Gold);
			}

			_shapeBatcher.DrawCircle(_player.Circle.Position, _player.Circle.Radius, 16, 2, Color.Red);

			_shapeBatcher.DrawArrow(_player.Position, _mouseInWorldSpace - _player.Position, 2, 3, Color.White);

			_shapeBatcher.End();

			_batch.Begin();
			_batch.DrawString(_font, "GamePlay Screen", new Vector2(100, 50), Color.Black);
			_batch.DrawString(_font, $"{_SecondsLeft.ToString("0.0")} Seconds Remaining", new Vector2(100, 100), Color.Black);
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

			if (_terrain.ReachedTarget(_player.Position))
			{
				_SecondsLeft += 3;
				_terrain.ChangeTarget();
			}

			//_terrain.UpdateMouse(_mouseInWorldSpace);

			_terrain.UpdateBullets(pSeconds);

			_SecondsLeft -= pSeconds;

			if (_SecondsLeft <= 0.0f)
			{
				QuantumRush game = QuantumRush.Instance();
			//	game.ReplaceScreen(new GameOverScreen());
			}

			_camera.LookAt(_player.Position, _player.Heading);
		}
	}
}
