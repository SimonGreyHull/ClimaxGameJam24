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
					_shapeBatcher.DrawTriangle(_terrain.Vertices[i, j], _terrain.Vertices[i + 1, j + 1], _terrain.Vertices[i + 1, j], _terrain.Colours[i,j]);
					_shapeBatcher.DrawTriangle(_terrain.Vertices[i, j], _terrain.Vertices[i, j + 1], _terrain.Vertices[i + 1, j + 1], _terrain.Colours[i, j]);
				}
			}

			_shapeBatcher.DrawCircle(_player.Position, 10, 16, 2, Color.Red);

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

			Point point = new Point();

			_mouseInWorldSpace = _camera.ScreenSpaceToWorldSpace(Mouse.GetState().Position);

			Vector2 heading = _mouseInWorldSpace - _player.Position;
			heading.Normalize();
			_player.Update(pSeconds, heading);

			_SecondsLeft -= pSeconds;

			if (_SecondsLeft <= 0.0f)
			{
				QuantumRush game = QuantumRush.Instance();
				//	game.ReplaceScreen(new GameOverScreen());
			}

			//float dx = 0f, dy = 0f, rot = 0f, change = 0.25f, scale = 1f;
			//if (Keyboard.GetState().IsKeyDown(Keys.A))
			//{
			//	dx -= change;
			//}
			//if (Keyboard.GetState().IsKeyDown(Keys.D))
			//{
			//	dx += change;
			//}
			//if (Keyboard.GetState().IsKeyDown(Keys.W))
			//{
			//	dy -= change;
			//}
			//if (Keyboard.GetState().IsKeyDown(Keys.S))
			//{
			//	dy += change;
			//}
			//if (Keyboard.GetState().IsKeyDown(Keys.Q))
			//{
			//	rot += change;
			//}
			//if (Keyboard.GetState().IsKeyDown(Keys.E))
			//{
			//	rot -= change;
			//}
			//if (Keyboard.GetState().IsKeyDown(Keys.R))
			//{
			//	scale += change;
			//}
			//if (Keyboard.GetState().IsKeyDown(Keys.F))
			//{
			//	scale -= change;
			//}

			_camera.LookAt(_player.Position);

			//_camera.Translate(dx, dy);
			////_camera.Rotate(rot);
			//_camera.Scale(scale);
		}
	}
}
