using Microsoft.Xna.Framework;
using QRLibrary.Shapes;

namespace QRLibrary.Screens.GameEntities
{
	internal class Player
	{
		public enum Movement { FORWARD, NONE, BACKWARD };
		public enum Strafe { LEFT, NONE, RIGHT };

		public int Score { get; private set; }

		public bool IsAlive { get; set; }

		private float bulletCooldown = 1f;

		public bool CanFire { get { return bulletCooldown <= 0f; } }

		public Bullet GetBullet()
		{
			bulletCooldown = 0.25f;
			return new Bullet(new Circle(Position, 4), Heading * 300);
		}

		public Movement movement { get; set; }
		public Strafe strafe { get; set; }

		public Circle Circle { get; private set; }
		public Triangle Triangle { get; private set; }

		private Vector2 OldPosition { get; set; }

		public Vector2 Position { get { return Circle.Position; } }
		public Vector2 Heading { get; private set; }

		public float MAX_SPEED { get; private set; }

		public Player() {
			movement = Movement.NONE;
			strafe = Strafe.NONE;
			Circle = new Circle(new Vector2(150, 250), 12);
			BuildTriangle(Position, Heading, Circle.Radius);
			IsAlive = true;
		}

		public void AddScore(int score)
		{
			Score += score;
		}

		private void BuildTriangle(Vector2 position, Vector2 Heading, float radius)
		{
			Vector2 forward = Heading * radius;
			Vector2 left = new Vector2(-forward.Y, forward.X);
			Vector2 V1 = Position + forward;
			Vector2 V2 = Position - forward + left;
			Vector2 V3 = Position - forward - left;
			Triangle = new Triangle(V2, V1, V3);
		}

		public void Update(float seconds, Vector2 heading)
		{
			OldPosition = Position;
			Heading = heading;
			Vector2 v = heading * 200 * seconds;
			switch (movement)
			{
				case Movement.FORWARD:
					Circle.Position += v;
					break;
				case Movement.BACKWARD:
					Circle.Position -= v;
					break;
			}

			switch (strafe)
			{
				case Strafe.LEFT:
					Circle.Position += new Vector2(-v.Y, v.X);
					break;
				case Strafe.RIGHT:
					Circle.Position += new Vector2(v.Y, -v.X);
					break;
			}

			if (bulletCooldown > 0)
			{
				bulletCooldown -= seconds;
			}

			BuildTriangle(Position, Heading, Circle.Radius);
		}

		public void PreviousPosition()
		{
			Circle.Position = OldPosition;
		}
	}
}
