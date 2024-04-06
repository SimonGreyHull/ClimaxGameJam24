using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace QRLibrary.Shapes
{
	internal class Circle : Shape
	{
		public float Radius { get; private set; }

		public Circle(Vector2 position, float radius) : base(position)
		{
			Radius = radius;
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

		public override bool IsInside(in Point point)
		{
			throw new NotImplementedException();
		}

		public override bool IsInside(in Vector2 point)
		{
			throw new NotImplementedException();
		}

		public override bool IsOutside(in Point point)
		{
			throw new NotImplementedException();
		}

		public override bool IsOutside(in Vector2 point)
		{
			throw new NotImplementedException();
		}

		public override bool IntersectsCircle(in Circle circle)
		{
			throw new NotImplementedException();
		}

		public override bool IntersectsTriangle(in Triangle triangle)
		{
			throw new NotImplementedException();
		}
	}
}
