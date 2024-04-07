using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using QRLibrary.Screens;

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
		private ScreenManager _screenManager = new ScreenManager();

		internal SoundManager SoundManager { get; private set; }

		internal void ReplaceScreen(IScreen screen)
		{
			_screenManager.Pop();
			_screenManager.Push(screen);
		}

		internal void PushScreen(IScreen screen)
		{
			_screenManager.Push(screen);
		}

		internal void PopScreen()
		{
			_screenManager.Pop();
		}

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

			SoundManager = new SoundManager();

			_screenManager.Push(new FlashScreen());
		}

		protected override void Update(GameTime gameTime)
		{
			if (Keyboard.GetState().IsKeyDown(Keys.Escape))
				Exit();

			float seconds = 0.001f * gameTime.ElapsedGameTime.Milliseconds;
			_screenManager.Update(seconds);

			// TODO: Add your update logic here

			base.Update(gameTime);
		}

		protected override void Draw(GameTime gameTime)
		{
			// TODO: Add your drawing code here
			float seconds = 0.001f * gameTime.ElapsedGameTime.Milliseconds;
			_screenManager.Draw(seconds);

			base.Draw(gameTime);
		}
	}
}
