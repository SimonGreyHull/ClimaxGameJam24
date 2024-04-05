using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace QRLibrary
{
	public class QuantumRush : Game
	{
		private static QuantumRush _instance = null;

		public static QuantumRush Instance()
		{
			if(_instance == null)
			{
				_instance = new QuantumRush();
			}
			return _instance;
		}

		private GraphicsDeviceManager _graphics;
		private SpriteBatch _spriteBatch;

		private QuantumRush()
		{
			_graphics = new GraphicsDeviceManager(this);
			Content.RootDirectory = "Content";
			IsMouseVisible = true;
		}

		protected override void Initialize()
		{
			// TODO: Add your initialization logic here

			base.Initialize();
		}

		protected override void LoadContent()
		{
			_spriteBatch = new SpriteBatch(GraphicsDevice);

			// TODO: use this.Content to load your game content here
		}

		protected override void Update(GameTime gameTime)
		{
			if (Keyboard.GetState().IsKeyDown(Keys.Escape))
				Exit();

			// TODO: Add your update logic here

			base.Update(gameTime);
		}

		protected override void Draw(GameTime gameTime)
		{
			GraphicsDevice.Clear(Color.Red);

			// TODO: Add your drawing code here

			base.Draw(gameTime);
		}
	}
}
