using Microsoft.Xna.Framework;
using QRLibrary.Shapes;
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

		public Circle Circle { get; private set; }

		private Vector2 OldPosition { get; set; }

		public Vector2 Position { get { return Circle.Position; } }
		public Vector2 Heading { get; private set; }

		public float MAX_SPEED { get; private set; }

		public Player() {
			movement = Movement.NONE;
			strafe = Strafe.NONE;
			Circle = new Circle(new Vector2(150, 250), 8);
		}

		public void Update(float seconds, Vector2 heading)
		{
			OldPosition = Position;
			Heading = heading;
			Vector2 v = heading * 100 * seconds;
			switch (movement)
			{
				case Movement.FORWARD:
					Circle.Position += v;
					break;
				case Movement.BACKWARD:
					Circle.Position -= v;
					break;
			}

			switch(strafe)
			{
				case Strafe.LEFT:
					Circle.Position += new Vector2(-v.Y, v.X);
					break;
				case Strafe.RIGHT:
					Circle.Position += new Vector2(v.Y, -v.X);
					break;
			}
		}

		public void PreviousPosition()
		{
			Circle.Position = OldPosition;
		}
	}
}
