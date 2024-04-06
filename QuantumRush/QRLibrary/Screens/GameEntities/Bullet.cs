using Microsoft.Xna.Framework;
using QRLibrary.Shapes;

namespace QRLibrary.Screens.GameEntities
{
	internal class Bullet
	{
		public Circle Circle { get; set; }

		public Vector2 Velocity { get; }

		public Bullet(Circle circle, Vector2 velocity) { Circle = circle; Velocity = velocity; }

		public void Update(float seconds)
		{
			Circle.Position += Velocity * seconds;
		}
	}
}
