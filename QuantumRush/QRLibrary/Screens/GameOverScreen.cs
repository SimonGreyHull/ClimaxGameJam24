using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace QRLibrary.Screens
{
	internal class GameOverScreen : IScreen
	{
		private SpriteFont _font;
		private SpriteBatch _batch;
		private int _Score;
		public GameOverScreen(int score)
		{
			QuantumRush game = QuantumRush.Instance();
			_font = game.Content.Load<SpriteFont>("Font");
			_batch = new SpriteBatch(game.GraphicsDevice);
			_Score = score;	
		}

		public void Draw(float pSeconds)
		{
			Game game = QuantumRush.Instance();
			game.GraphicsDevice.Clear(Color.MediumPurple);

			string[] text = { "Game Over Screen", $"You Scored {_Score} Fabulous Points!", "If games are art then a game with no art is still art? Right?", "Press Enter to Return to the Title Screen" };

			float totalHeight = 0;

			for(int i = 0; i < text.Length; i++)
			{
				totalHeight += _font.MeasureString(text[i]).Y;
			}

			float startingHeight = (QuantumRush.Instance().GraphicsDevice.Viewport.Height - totalHeight) / 2;

			_batch.Begin();
			Vector2 textPos = new Vector2(0, startingHeight);
			Vector2 fontSize;
			for(int i = 0; i < text.Length; i++)
			{
				fontSize = _font.MeasureString(text[i]);
				textPos.X = (QuantumRush.Instance().GraphicsDevice.Viewport.Width - fontSize.X) / 2;
				textPos.Y += fontSize.Y;
				_batch.DrawString(_font, text[i], textPos, Color.Black);
			}
			_batch.End();
		}

		public void Update(float pSeconds)
		{
			if (Keyboard.GetState().IsKeyDown(Keys.Enter))
			{
				QuantumRush game = QuantumRush.Instance();
				game.PopScreen();
			}
		}
	}
}
