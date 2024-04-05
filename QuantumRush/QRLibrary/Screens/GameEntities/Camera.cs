using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace QRLibrary.Screens.GameEntities
{
	internal class Camera
	{
		private Matrix _View;
		private Matrix _Projection;
		public Camera()
		{
			_View = Matrix.Identity;
			_Projection = Matrix.CreateOrthographicOffCenter(-500, 500, -500, 500, 0f, 1f);
		}

		public Matrix Projection { get { return _Projection; } }
		public Matrix View { get { return _View; } }

		public void Translate(float x, float y)
		{
			_View *= Matrix.CreateTranslation(x, y, 0);
		}

		public void Rotate(float theta)
		{
			_View *= Matrix.CreateRotationZ(theta);
		}

		public void Scale(float scale)
		{
			_View *= Matrix.CreateScale(scale);
		}

		public Vector2 ScreenSpaceToWorldSpace(Point p)
		{
			Vector2 v = new Vector2(0, 0);
			Matrix.Invert(ref _Projection, out Matrix inverted);
			v = Vector2.Transform(v, inverted);
			return v;
		}

		public Vector2 ScreenSpaceFromWorldSpace(Vector2 v)
		{
			return Vector2.Zero;
		}
	}
}
