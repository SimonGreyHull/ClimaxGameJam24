using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace QRLibrary.Screens
{
	internal class TitleScreen : IScreen
	{
		public void Draw(float pSeconds)
		{
			Game game = QuantumRush.Instance();
			game.GraphicsDevice.Clear(Color.HotPink);
		}

		public void Update(float pSeconds)
		{
		}
	}
}
