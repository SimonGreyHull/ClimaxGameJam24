using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace QRLibrary.Screens
{
	internal class GameScreen : IScreen
	{
		private float _SecondsLeft;

		private SpriteFont _font;
		private SpriteBatch _batch;

		internal GameScreen()
		{
			_SecondsLeft = 15;
			QuantumRush game = QuantumRush.Instance();
			_font = game.Content.Load<SpriteFont>("Font");
			_batch = new SpriteBatch(game.GraphicsDevice);
		}

		public void Draw(float pSeconds)
		{
			Game game = QuantumRush.Instance();
			game.GraphicsDevice.Clear(Color.Turquoise);

			_batch.Begin();
			_batch.DrawString(_font, "GamePlay Screen", new Vector2(100, 50), Color.Black);
			_batch.DrawString(_font, $"{_SecondsLeft.ToString("0.0")} Seconds Remaining", new Vector2(100, 100), Color.Black);
			_batch.End();
		}

		public void Update(float pSeconds)
		{
			_SecondsLeft -= pSeconds;

			if (_SecondsLeft <= 0.0f)
			{
				QuantumRush game = QuantumRush.Instance();
				game.ReplaceScreen(new GameOverScreen());
			}
		}
	}
}
