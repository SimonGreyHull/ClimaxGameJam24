using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace QRLibrary.Screens
{
	internal class FlashScreen : IScreen
	{
		Texture2D _LogoTexture = null;
		SpriteBatch _SpriteBatch = null;
		Rectangle _Rectangle;
		float _SecondsLeft;

		public FlashScreen()
		{
			QuantumRush game = QuantumRush.Instance();
			_LogoTexture = game.Content.Load<Texture2D>("selogo");
			game.SoundManager.Add("music");
			game.SoundManager.Add("shoot");
			game.SoundManager.Add("explosion");
			game.SoundManager.Add("points");
			_SpriteBatch = new SpriteBatch(game.GraphicsDevice);
			int screenWidth = game.GraphicsDevice.Viewport.Width;
			int screenHeight = game.GraphicsDevice.Viewport.Height;
			int height = screenHeight / 2;
			int width = (int)(_LogoTexture.Width * (float)height / _LogoTexture.Height);
			int x = (screenWidth - width) / 2;
			int y = (screenHeight - height) / 2;
			_Rectangle = new Rectangle(x, y, width, height);
			_SecondsLeft = DGS.SECONDS_TO_DISPLAY_FLASH_SCREEN;
			//LoadContent();
		}

		public void Draw(float pSeconds)
		{
			Game game = QuantumRush.Instance();
			game.GraphicsDevice.Clear(Color.Black);
			_SpriteBatch.Begin();
			_SpriteBatch.Draw(_LogoTexture, _Rectangle, Color.White);
			_SpriteBatch.End();
		}

		public void Update(float pSeconds)
		{
			_SecondsLeft -= pSeconds;

			if (_SecondsLeft <= 0.0f)
			{
				QuantumRush game = QuantumRush.Instance();
				game.ReplaceScreen(new TitleScreen());
			}
		}
	}
}
