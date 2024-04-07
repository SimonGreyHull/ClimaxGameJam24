using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;
using static System.Formats.Asn1.AsnWriter;

namespace QRLibrary.Screens.GameEntities
{
	internal class Camera
	{
		private Vector2 _currentPosition;
		private Vector2 _currentHeading;
		private Vector2 _targetPosition;
		private Vector2 _targetHeading;

		private Matrix _View;
		private Matrix _Projection;
		public Camera()
		{
			_View = Matrix.Identity;
			_Projection = Matrix.CreateOrthographicOffCenter(-500, 500, -500, 500, 0f, 1f);
			_Projection = Matrix.CreateOrthographic(QuantumRush.Instance().GraphicsDevice.Viewport.Width,
				QuantumRush.Instance().GraphicsDevice.Viewport.Height,
				0f, 1f);

			_currentPosition = Vector2.Zero;
			_currentHeading = Vector2.UnitY;
		}

		public Matrix Projection { get { return _Projection; } }
		public Matrix View { get { return _View; } }

		public void LookAt(Vector2 position, Vector2 heading)
		{
			_currentPosition += (position - _currentPosition) * 0.05f;

			_currentHeading += (heading - _currentHeading) * 0.005f;
			_currentHeading.Normalize();

			_View = Matrix.CreateLookAt(new Vector3(_currentPosition.X, _currentPosition.Y, 0.5f), new Vector3(_currentPosition.X, _currentPosition.Y, 0), new Vector3(_currentHeading.X, _currentHeading.Y, 0));
		}

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
			Vector3 v = QuantumRush.Instance().GraphicsDevice.Viewport.Unproject(new Vector3(p.X, p.Y, 0), _Projection, _View, Matrix.Identity);
			return new Vector2(v.X, v.Y);
		}

		public Vector2 ScreenSpaceFromWorldSpace(Vector2 v)
		{
			Vector3 result = QuantumRush.Instance().GraphicsDevice.Viewport.Project(new Vector3(v.X, v.Y, 0), _Projection, _View, Matrix.Identity);
			return new Vector2(result.X, result.Y);
		}
	}
}
