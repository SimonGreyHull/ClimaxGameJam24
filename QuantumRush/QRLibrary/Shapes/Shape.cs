using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace QRLibrary.Shapes
{
	internal abstract class Shape
	{
		public Vector2 Position { get; set; }

		protected Shape(Vector2 position) { Position = position; }

		public abstract bool IsInside(in Point point);
		public abstract bool IsOutside(in Point point);
		public abstract bool IsInside(in Vector2 point);
		public abstract bool IsOutside(in Vector2 point);
		public abstract bool Intersects(in Line line);
		public abstract bool Intersects(in LineSegment lineSegment);
		public abstract bool Intersects(in Shape shape);
		public abstract bool IntersectsCircle(in Circle circle);
		public abstract bool IntersectsTriangle(in Triangle triangle);
	}
}
