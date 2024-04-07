using Microsoft.AspNetCore.Components.Web;
using Microsoft.Xna.Framework;
using QRLibrary.Shapes;

namespace QRLibrary.Screens.GameEntities
{
	internal class Enemy
	{


		public Circle Circle { get; private set; }

		public Vector2 Position { get { return Circle.Position; } }

		private Vector2 OldPosition { get; set; }

		public Vector2 Velocity { get; private set; }

		public Enemy(Vector2 position)
		{
			Circle = new Circle(position, 12);
			Velocity = new Vector2(10, 10);
		}
		
		public void Update(float seconds)
		{
			OldPosition = Position;

			Circle.Position += Velocity * seconds;
		}
	}
}
