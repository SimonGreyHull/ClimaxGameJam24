using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace QRLibrary.Screens.GameEntities
{
	internal class Player
	{
		public enum Movement { FORWARD, NONE, BACKWARD };
		public enum Strafe { LEFT, NONE, RIGHT };

		public Movement movement { get; set; }
		public Strafe strafe { get; set; }

		public Vector2 Position { get; private set; }
		public Vector2 Heading { get; private set; }

		public float MAX_SPEED { get; private set; }

		public Player() {
			movement = Movement.NONE;
			strafe = Strafe.NONE;
			Position = new Vector2(100, 100);
		}

		public void Update(float seconds, Vector2 heading)
		{
			Heading = heading;
			Vector2 v = heading * 100 * seconds;
			switch (movement)
			{
				case Movement.FORWARD:
					Position += v;
					break;
				case Movement.BACKWARD:
					Position -= v;
					break;
			}

			switch(strafe)
			{
				case Strafe.LEFT:
					Position += new Vector2(-v.Y, v.X);
					break;
				case Strafe.RIGHT:
					Position += new Vector2(v.Y, -v.X);
					break;
			}
		}
	}
}
