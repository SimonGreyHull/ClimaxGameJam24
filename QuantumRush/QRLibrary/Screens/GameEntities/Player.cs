using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace QRLibrary.Screens.GameEntities
{
	internal class Player
	{
		public Vector2 Position { get; private set; }

		public float MAX_SPEED { get; private set; }

		public Player() {
			Position = new Vector2(100, 100);
		}
	}
}
