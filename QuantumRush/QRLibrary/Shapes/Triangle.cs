using Microsoft.Xna.Framework;
using System;

namespace QRLibrary.Shapes
{
	internal class Triangle : Shape
	{
		private Vector2[] _vertices = new Vector2[3];

		public Vector2 V1 { get { return _vertices[0] + Position; } }
		public Vector2 V2 { get { return _vertices[1] + Position; } }
		public Vector2 V3 { get { return _vertices[2] + Position; } }

		public Triangle(Vector2 v1, Vector2 v2, Vector2 v3) : base((v1 + v2 + v3) / 3)
		{
			_vertices[0] = v1 - Position;
			_vertices[1] = v2 - Position;
			_vertices[2] = v3 - Position;
		}

		public override bool Intersects(in Line line)
		{
			throw new NotImplementedException();
		}

		public override bool Intersects(in LineSegment lineSegment)
		{
			throw new NotImplementedException();
		}

		public override bool Intersects(in Shape shape)
		{
			throw new NotImplementedException();
		}

		public override bool IntersectsCircle(in Circle circle)
		{
			// Check if any vertex of the triangle is inside the circle
			if ((V1 - circle.Position).LengthSquared() <= circle.Radius * circle.Radius
				|| (V2 - circle.Position).LengthSquared() <= circle.Radius * circle.Radius
					|| (V3 - circle.Position).LengthSquared() <= circle.Radius * circle.Radius)
			{
				return true;
			}				

			// Check if any edge of the triangle intersects the circle
			for (int i = 0; i < 3; i++)
			{
				float edgeStartX = i == 0 ? V1.X : i == 1 ? V2.X : V3.X;
				float edgeStartY = i == 0 ? V1.Y : i == 1 ? V2.Y : V3.Y;
				float edgeEndX = (i + 1) % 3 == 0 ? V1.X : (i + 1) % 3 == 1 ? V2.X : V3.X;
				float edgeEndY = (i + 1) % 3 == 0 ? V1.Y : (i + 1) % 3 == 1 ? V2.Y : V3.Y;

				float closestX = edgeStartX + (edgeEndX - edgeStartX) * ((circle.Position.X - edgeStartX) * (edgeEndX - edgeStartX) + (circle.Position.Y - edgeStartY) * (edgeEndY - edgeStartY)) / ((edgeEndX - edgeStartX) * (edgeEndX - edgeStartX) + (edgeEndY - edgeStartY) * (edgeEndY - edgeStartY));
				float closestY = edgeStartY + (edgeEndY - edgeStartY) * ((circle.Position.X - edgeStartX) * (edgeEndX - edgeStartX) + (circle.Position.Y - edgeStartY) * (edgeEndY - edgeStartY)) / ((edgeEndX - edgeStartX) * (edgeEndX - edgeStartX) + (edgeEndY - edgeStartY) * (edgeEndY - edgeStartY));

				if (closestX >= Math.Min(edgeStartX, edgeEndX) && closestX <= Math.Max(edgeStartX, edgeEndX) &&
					closestY >= Math.Min(edgeStartY, edgeEndY) && closestY <= Math.Max(edgeStartY, edgeEndY))
				{
					float distanceSquared = (closestX - circle.Position.X) * (closestX - circle.Position.X) + (closestY - circle.Position.Y) * (closestY - circle.Position.Y);
					if (distanceSquared <= circle.Radius * circle.Radius)
					{
						return true;
					}
				}
			}

			return false;
		}

		public override bool IsInside(in Point point)
		{
			throw new NotImplementedException();
		}

		// TODO Optimise this rubbish
		public override bool IsInside(in Vector2 point)
		{
			float[] triangleVerticesX = { Position.X + _vertices[0].X, Position.X + _vertices[1].X, Position.X + _vertices[2].X };
			float[] triangleVerticesY = { Position.Y + _vertices[0].Y, Position.Y + _vertices[1].Y, Position.Y + _vertices[2].Y };

			// Calculate barycentric coordinates
			float alpha = ((triangleVerticesY[1] - triangleVerticesY[2]) * (point.X - triangleVerticesX[2]) +
							(triangleVerticesX[2] - triangleVerticesX[1]) * (point.Y - triangleVerticesY[2])) /
						   ((triangleVerticesY[1] - triangleVerticesY[2]) * (triangleVerticesX[0] - triangleVerticesX[2]) +
							(triangleVerticesX[2] - triangleVerticesX[1]) * (triangleVerticesY[0] - triangleVerticesY[2]));

			float beta = ((triangleVerticesY[2] - triangleVerticesY[0]) * (point.X - triangleVerticesX[2]) +
						   (triangleVerticesX[0] - triangleVerticesX[2]) * (point.Y - triangleVerticesY[2])) /
						  ((triangleVerticesY[1] - triangleVerticesY[2]) * (triangleVerticesX[0] - triangleVerticesX[2]) +
						   (triangleVerticesX[2] - triangleVerticesX[1]) * (triangleVerticesY[0] - triangleVerticesY[2]));

			float gamma = 1.0f - alpha - beta;

			// Check if point is inside the triangle (barycentric coordinates are all positive)
			return alpha >= 0 && beta >= 0 && gamma >= 0;
		}

		public override bool IsOutside(in Point point)
		{
			throw new NotImplementedException();
		}

		public override bool IsOutside(in Vector2 point)
		{
			throw new NotImplementedException();
		}

		public override bool IntersectsTriangle(in Triangle triangle)
		{
			throw new NotImplementedException();
		}
	}
}
