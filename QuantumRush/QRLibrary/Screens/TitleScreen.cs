using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using QRLibrary.Screens.GameEntities;

namespace QRLibrary.Screens
{
	internal class TitleScreen : IScreen
	{
		private SpriteFont _font;
		private SpriteBatch _batch;

		public TitleScreen()
		{
			QuantumRush game = QuantumRush.Instance();
			_font = game.Content.Load <SpriteFont> ("Font");
			_batch = new SpriteBatch(game.GraphicsDevice);
		}

		public void Draw(float pSeconds)
		{
			Game game = QuantumRush.Instance();
			game.GraphicsDevice.Clear(Color.MediumPurple);

			string[] text = { "Title Screen : Quantum Rush",
				"No Artists! No... Problem???",
				"Use the Mouse to Rotate the Ship",
				"Use W and S keys to Move Forward and Back",
				"Use A and D keys to Strafe Left and Right",
				"Spawn points count down for a PERIOD of time!",
				"Fly over the points to reset the count (and score points!)",
				"If the count down reaches 0 an evil octogon is PERIODICALLY spawned",
				"No problemo, use the left mouse button to send it to polygon hell! (and get more points!)",
				"Press the Period Button (.) to Start." };

			float totalHeight = 0;

			for (int i = 0; i < text.Length; i++)
			{
				totalHeight += _font.MeasureString(text[i]).Y;
			}

			float startingHeight = (QuantumRush.Instance().GraphicsDevice.Viewport.Height - totalHeight) / 2;

			_batch.Begin();
			Vector2 textPos = new Vector2(0, startingHeight);
			Vector2 fontSize;
			for (int i = 0; i < text.Length; i++)
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
			if (Keyboard.GetState().IsKeyDown(Keys.OemPeriod))
			{
				QuantumRush game = QuantumRush.Instance();
				game.PushScreen(new GameScreen());
			}
		}
	}
}
