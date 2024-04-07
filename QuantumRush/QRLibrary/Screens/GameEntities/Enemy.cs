using Microsoft.Xna.Framework;
using QRLibrary.Shapes;

namespace QRLibrary.Screens.GameEntities
{
	internal class Enemy
	{
		public Circle Circle { get; private set; }

		public Vector2 Position { get { return Circle.Position; } }

		public Enemy(Vector2 position)
		{
			Circle = new Circle(position, 12);
		}
	}
}
