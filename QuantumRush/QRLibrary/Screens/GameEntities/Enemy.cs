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

			Terrain terrain = Terrain.Instance();

			(TerrainCellData[], int) cells = terrain.GetSurroundingCells(Position);

			TerrainCellData targetCell = cells.Item1[0];

			for(int i = 1; i < cells.Item2; i++)
			{
				if (cells.Item1[i].stepsToPlayer < targetCell.stepsToPlayer)
				{
					targetCell = cells.Item1[i];
				}
			}

			Vector2 desiredVelocity = targetCell.Centre - Position;
			Vector2 steeringForce = desiredVelocity - Velocity;
			Velocity += steeringForce * seconds;

			Circle.Position += Velocity * seconds;
		}
	}
}
