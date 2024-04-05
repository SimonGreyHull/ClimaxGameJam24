using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace QRLibrary.Screens
{
	internal class GameOverScreen : IScreen
	{
		private SpriteFont _font;
		private SpriteBatch _batch;

		public GameOverScreen()
		{
			QuantumRush game = QuantumRush.Instance();
			_font = game.Content.Load<SpriteFont>("Font");
			_batch = new SpriteBatch(game.GraphicsDevice);
		}

		public void Draw(float pSeconds)
		{
			Game game = QuantumRush.Instance();
			game.GraphicsDevice.Clear(Color.Red);

			_batch.Begin();
			_batch.DrawString(_font, "Game Over Screen", new Vector2(100, 50), Color.Black);
			_batch.DrawString(_font, "Press Space", new Vector2(100, 100), Color.Black);
			_batch.End();
		}

		public void Update(float pSeconds)
		{
			if (Keyboard.GetState().IsKeyDown(Keys.Space))
			{
				QuantumRush game = QuantumRush.Instance();
				game.PopScreen();
			}
		}
	}
}
