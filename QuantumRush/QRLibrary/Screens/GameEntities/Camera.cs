using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;
using static System.Formats.Asn1.AsnWriter;

namespace QRLibrary.Screens.GameEntities
{
	public class Camera2
	{
		public float Zoom { get; set; }
		public Vector2 Position { get; set; }
		public Rectangle Bounds { get; protected set; }
		public Rectangle VisibleArea { get; protected set; }
		public Matrix Transform { get; protected set; }

		private float currentMouseWheelValue, previousMouseWheelValue, zoom, previousZoom;

		public Camera2()
		{
			Bounds = QuantumRush.Instance().GraphicsDevice.Viewport.Bounds ;
			Zoom = 1f;
			Position = Vector2.Zero;
		}


		private void UpdateVisibleArea()
		{
			var inverseViewMatrix = Matrix.Invert(Transform);

			var tl = Vector2.Transform(Vector2.Zero, inverseViewMatrix);
			var tr = Vector2.Transform(new Vector2(Bounds.X, 0), inverseViewMatrix);
			var bl = Vector2.Transform(new Vector2(0, Bounds.Y), inverseViewMatrix);
			var br = Vector2.Transform(new Vector2(Bounds.Width, Bounds.Height), inverseViewMatrix);

			var min = new Vector2(
				MathHelper.Min(tl.X, MathHelper.Min(tr.X, MathHelper.Min(bl.X, br.X))),
				MathHelper.Min(tl.Y, MathHelper.Min(tr.Y, MathHelper.Min(bl.Y, br.Y))));
			var max = new Vector2(
				MathHelper.Max(tl.X, MathHelper.Max(tr.X, MathHelper.Max(bl.X, br.X))),
				MathHelper.Max(tl.Y, MathHelper.Max(tr.Y, MathHelper.Max(bl.Y, br.Y))));
			VisibleArea = new Rectangle((int)min.X, (int)min.Y, (int)(max.X - min.X), (int)(max.Y - min.Y));
		}

		private void UpdateMatrix()
		{
			Transform = Matrix.CreateTranslation(new Vector3(-Position.X, -Position.Y, 0)) *
					Matrix.CreateScale(Zoom) *
					Matrix.CreateTranslation(new Vector3(Bounds.Width * 0.5f, Bounds.Height * 0.5f, 0));
			UpdateVisibleArea();
		}

		public void MoveCamera(Vector2 movePosition)
		{
			Vector2 newPosition = Position + movePosition;
			Position = newPosition;
		}

		public void AdjustZoom(float zoomAmount)
		{
			Zoom += zoomAmount;
			if (Zoom < .35f)
			{
				Zoom = .35f;
			}
			if (Zoom > 2f)
			{
				Zoom = 2f;
			}
		}

		public void UpdateCamera()
		{
			Viewport bounds = QuantumRush.Instance().GraphicsDevice.Viewport;
			Bounds = bounds.Bounds;
			UpdateMatrix();

			Vector2 cameraMovement = Vector2.Zero;
			int moveSpeed;

			if (Zoom > .8f)
			{
				moveSpeed = 15;
			}
			else if (Zoom < .8f && Zoom >= .6f)
			{
				moveSpeed = 20;
			}
			else if (Zoom < .6f && Zoom > .35f)
			{
				moveSpeed = 25;
			}
			else if (Zoom <= .35f)
			{
				moveSpeed = 30;
			}
			else
			{
				moveSpeed = 10;
			}


			if (Keyboard.GetState().IsKeyDown(Keys.W))
			{
				cameraMovement.Y = -moveSpeed;
			}

			if (Keyboard.GetState().IsKeyDown(Keys.S))
			{
				cameraMovement.Y = moveSpeed;
			}

			if (Keyboard.GetState().IsKeyDown(Keys.A))
			{
				cameraMovement.X = -moveSpeed;
			}

			if (Keyboard.GetState().IsKeyDown(Keys.D))
			{
				cameraMovement.X = moveSpeed;
			}

			previousMouseWheelValue = currentMouseWheelValue;
			currentMouseWheelValue = Mouse.GetState().ScrollWheelValue;

			if (currentMouseWheelValue > previousMouseWheelValue)
			{
				AdjustZoom(.05f);
				Console.WriteLine(moveSpeed);
			}

			if (currentMouseWheelValue < previousMouseWheelValue)
			{
				AdjustZoom(-.05f);
				Console.WriteLine(moveSpeed);
			}

			previousZoom = zoom;
			zoom = Zoom;
			if (previousZoom != zoom)
			{
				Console.WriteLine(zoom);

			}

			MoveCamera(cameraMovement);
		}
	}

	internal class Camera
	{
		private Matrix _View;
		private Matrix _Projection;
		public Camera()
		{
			_View = Matrix.Identity;
			_Projection = Matrix.CreateOrthographicOffCenter(-500, 500, -500, 500, 0f, 1f);
			_Projection = Matrix.CreateOrthographic(QuantumRush.Instance().GraphicsDevice.Viewport.Width,
				QuantumRush.Instance().GraphicsDevice.Viewport.Height,
				0f, 1f);
		}

		public Matrix Projection { get { return _Projection; } }
		public Matrix View { get { return _View; } }

		public void LookAt(Vector2 position)
		{
			_View = Matrix.CreateLookAt(new Vector3(position.X, position.Y, 0.5f), new Vector3(position.X, position.Y, 0), Vector3.UnitY);
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
			return Vector2.Zero;
		}
	}
}
