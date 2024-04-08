using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using QRLibrary.Screens.GameEntities;
using System;

namespace QRLibrary.Shapes
{
	/*
	  * Shape batcher is based on code development by Two-Bit Coding
	  * For an in depth explanation see
	  * https://www.youtube.com/watch?v=ZqwfoMjJAO4
	  * https://www.youtube.com/watch?v=nG9mTQcGnG0
	  * https://www.youtube.com/watch?v=rrDDryCRl94
	  * */

	internal class ShapeBatcher
	{
		private Game _game;

		private bool _disposed;
		private BasicEffect _effect;
		private VertexPositionColor[] _vertices;
		private short[] _indices;

		private int _shapeCount = 0;
		private int _vertexCount = 0;
		private int _indexCount = 0;

		private bool _isStarted = false;

		public static readonly float MIN_LINE_THICKNESS = 1f;
		public static readonly float MAX_LINE_THICKNESS = 10f;

		public ShapeBatcher()
		{
			_game = QuantumRush.Instance() ?? throw new ArgumentNullException(nameof(QuantumRush));
			_disposed = false;
			_effect = new BasicEffect(_game.GraphicsDevice);
			_effect.TextureEnabled = false;
			_effect.FogEnabled = false;
			_effect.LightingEnabled = false;
			_effect.VertexColorEnabled = true;
			_effect.World = Matrix.Identity;
			_effect.View = Matrix.Identity;
			_effect.Projection = Matrix.Identity;

			const int MAX_VERTEX_COUNT = 1024;
			const int MAX_INDEX_COUNT = MAX_VERTEX_COUNT * 3;
			_vertices = new VertexPositionColor[MAX_VERTEX_COUNT];
			_indices = new short[MAX_INDEX_COUNT];
		}

		public void Dispose()
		{
			if (_disposed)
			{
				return;
			}

			_effect?.Dispose();
			_disposed = true;
		}

		public void Begin(Camera camera)
		{
			if (_isStarted)
			{
				throw new System.Exception("Batch already started.");
			}

			Viewport viewport = _game.GraphicsDevice.Viewport;
			_effect.Projection = camera.Projection;
			_effect.View = camera.View;
			_isStarted = true;
		}

		public void End()
		{
			Flush();
			_isStarted = false;
		}

		private void Flush()
		{
			if (_shapeCount == 0)
			{
				return;
			}

			EnsureStarted();

			foreach (EffectPass pass in _effect.CurrentTechnique.Passes)
			{
				pass.Apply();
				_game.GraphicsDevice.DrawUserIndexedPrimitives<VertexPositionColor>(
					PrimitiveType.TriangleList,
					_vertices,
					0,
					_vertexCount,
					_indices,
					0,
					_indexCount / 3
				);
			}

			_shapeCount = 0;
			_indexCount = 0;
			_vertexCount = 0;
		}

		private void EnsureStarted()
		{
			if (!_isStarted)
			{
				throw new System.Exception("Batch not started.");
			}
		}

		private void EnsureSpace(int pShapeVertexCount, int pShapeIndexCount)
		{
			if (pShapeVertexCount > _vertices.Length)
			{
				throw new System.Exception("Maximum shape vertex count is " + _vertices.Length);
			}

			if (pShapeIndexCount > _indices.Length)
			{
				throw new System.Exception("Maximum shape index count is " + _indices.Length);
			}

			if (_vertexCount + pShapeVertexCount > _vertices.Length ||
				_indexCount + pShapeIndexCount > _indices.Length)
			{
				Flush();
			}
		}

		/// <summary>
		/// Draws a line from pA to PB as a rectangle with thickness pThickness and colour pColour.
		/// </summary>
		/// <param name="pA">Start of the line</param>
		/// <param name="pB">End of the line</param>
		/// <param name="pThickness">Thickness of the line (clamped between 2 and 10)</param>
		/// <param name="pColour">Colour of the line</param>
		public void DrawLine(Vector2 pA, Vector2 pB, float pThickness, Color pColour)
		{
			EnsureStarted();

			const int shapeVertexCount = 4;
			const int shapeIndexCount = 6;

			EnsureSpace(shapeVertexCount, shapeIndexCount);

			pThickness = Math.Clamp(pThickness, MIN_LINE_THICKNESS, MAX_LINE_THICKNESS);

			float halfThickness = pThickness * 0.5f;

			float e1x = pB.X - pA.X;
			float e1y = pB.Y - pA.Y;

			float invLength = halfThickness / MathF.Sqrt(e1x * e1x + e1y * e1y);

			e1x *= invLength;
			e1y *= invLength;

			float e2x = -e1x;
			float e2y = -e1y;

			float n1x = -e1y;
			float n1y = e1x;

			float n2x = -n1x;
			float n2y = -n1y;

			_indices[_indexCount++] = (short)(0 + _vertexCount);
			_indices[_indexCount++] = (short)(1 + _vertexCount);
			_indices[_indexCount++] = (short)(2 + _vertexCount);
			_indices[_indexCount++] = (short)(0 + _vertexCount);
			_indices[_indexCount++] = (short)(2 + _vertexCount);
			_indices[_indexCount++] = (short)(3 + _vertexCount);

			_vertices[_vertexCount++] = new VertexPositionColor(new Vector3(pA.X + n1x + e2x, pA.Y + n1y + e2y, 0f), pColour);
			_vertices[_vertexCount++] = new VertexPositionColor(new Vector3(pB.X + n1x + e1x, pB.Y + n1y + e1y, 0f), pColour);
			_vertices[_vertexCount++] = new VertexPositionColor(new Vector3(pB.X + n2x + e1x, pB.Y + n2y + e1y, 0f), pColour);
			_vertices[_vertexCount++] = new VertexPositionColor(new Vector3(pA.X + n2x + e2x, pA.Y + n2y + e2y, 0f), pColour);

			_shapeCount++;
		}

		public void DrawCircle(Vector2 pCentre, float pRadius, int pNumVertices, float pThickness, Color pColour)
		{
			const int MIN_POINTS = 3;
			const int MAX_POINTS = 256;

			pNumVertices = Math.Clamp(pNumVertices, MIN_POINTS, MAX_POINTS);

			float deltaAngle = MathHelper.TwoPi / pNumVertices;
			float angle = 0f;

			for (int i = 0; i < pNumVertices; i++)
			{
				float ax = pCentre.X + pRadius * MathF.Sin(angle);
				float ay = pCentre.Y + pRadius * MathF.Cos(angle);

				angle += deltaAngle;

				float bx = pCentre.X + pRadius * MathF.Sin(angle);
				float by = pCentre.Y + pRadius * MathF.Cos(angle);
				DrawLine(new Vector2(ax, ay), new Vector2(bx, by), pThickness, pColour);
			}
		}

		public void DrawFilledCircle(Vector2 pCentre, float pRadius, int pNumVertices, Color pColour)
		{

			const int MIN_POINTS = 3;
			const int MAX_POINTS = 256;

			pNumVertices = Math.Clamp(pNumVertices, MIN_POINTS, MAX_POINTS);

			float deltaAngle = MathHelper.TwoPi / pNumVertices;
			float angle = 0f;

			for (int i = 0; i < pNumVertices; i++)
			{
				float ax = pCentre.X + pRadius * MathF.Sin(angle);
				float ay = pCentre.Y + pRadius * MathF.Cos(angle);

				angle += deltaAngle;

				float bx = pCentre.X + pRadius * MathF.Sin(angle);
				float by = pCentre.Y + pRadius * MathF.Cos(angle);
				DrawTriangle(new Vector2(ax, ay), new Vector2(bx, by), pCentre, pColour);
			}

			//EnsureStarted();

			//const int MIN_POINTS = 3;
			//const int MAX_POINTS = 256;

			//pNumVertices = Math.Clamp(pNumVertices, MIN_POINTS, MAX_POINTS);
			//int shapeTriangleCount = pNumVertices - 2;
			//int shapeIndexCount = shapeTriangleCount * 3;

			//EnsureSpace(pNumVertices, shapeIndexCount);

			//for (int i = 0, index = 1; i < shapeTriangleCount; i++, index++)
			//{
			//	_indices[_indexCount++] = (short)(0 + _vertexCount);
			//	_indices[_indexCount++] = (short)(index + _vertexCount);
			//	_indices[_indexCount++] = (short)(index + 1 + _vertexCount);
			//}

			//float ax = pRadius;
			//float ay = 0f;

			//float rotation = MathHelper.TwoPi / pNumVertices;
			//float sin = MathF.Sin(rotation);
			//float cos = MathF.Cos(rotation);

			//for (int i = 0; i < pNumVertices; i++)
			//{
			//	float x1 = ax;
			//	float y1 = ay;

			//	_vertices[_vertexCount++] = new VertexPositionColor(new Vector3(x1 + pCentre.X, y1 + pCentre.Y, 0f), pColour);

			//	ax = cos * x1 - sin * y1;
			//	ay = sin * x1 + cos * y1;
			//}

			//_shapeCount++;
		}

		public void DrawAxisAlignedRectangle(Vector2 pTopLeft, float pWidth, float pHeight, float pThickness, Color pColour)
		{
			Vector2 topRight = new Vector2(pTopLeft.X + pWidth, pTopLeft.Y);
			Vector2 bottomRight = new Vector2(topRight.X, topRight.Y - pHeight);
			Vector2 bottomLeft = new Vector2(pTopLeft.X, bottomRight.Y);
			DrawLine(pTopLeft, topRight, pThickness, pColour);
			DrawLine(topRight, bottomRight, pThickness, pColour);
			DrawLine(bottomRight, bottomLeft, pThickness, pColour);
			DrawLine(bottomLeft, pTopLeft, pThickness, pColour);
		}

		public void DrawFilledAxisAlignedRectangle(Vector2 pTopLeft, float pWidth, float pHeight, Color pColour)
		{
			EnsureStarted();

			const int shapeVertexCount = 4;
			const int shapeIndexCount = 6;

			EnsureSpace(shapeVertexCount, shapeIndexCount);

			_indices[_indexCount++] = (short)(0 + _vertexCount);
			_indices[_indexCount++] = (short)(1 + _vertexCount);
			_indices[_indexCount++] = (short)(2 + _vertexCount);
			_indices[_indexCount++] = (short)(0 + _vertexCount);
			_indices[_indexCount++] = (short)(2 + _vertexCount);
			_indices[_indexCount++] = (short)(3 + _vertexCount);

			_vertices[_vertexCount++] = new VertexPositionColor(new Vector3(pTopLeft.X, pTopLeft.Y, 0f), pColour);
			_vertices[_vertexCount++] = new VertexPositionColor(new Vector3(pTopLeft.X + pWidth, pTopLeft.Y, 0f), pColour);
			_vertices[_vertexCount++] = new VertexPositionColor(new Vector3(pTopLeft.X + pWidth, pTopLeft.Y - pHeight, 0f), pColour);
			_vertices[_vertexCount++] = new VertexPositionColor(new Vector3(pTopLeft.X, pTopLeft.Y - pHeight, 0f), pColour);

			_shapeCount++;
		}

		public void DrawArrow(Vector2 pStart, Vector2 pVector, float pThickness, float pArrowSize, Color pColour)
		{
			Vector2 lineEnd = pStart + pVector;

			Vector2 u = pVector * (1f / pVector.Length());
			Vector2 v = new Vector2(-u.Y, u.X);

			Vector2 arrowHead1 = lineEnd - pArrowSize * u + pArrowSize * v;
			Vector2 arrowHead2 = lineEnd - pArrowSize * u - pArrowSize * v;

			DrawLine(pStart, lineEnd, pThickness, pColour);
			DrawLine(lineEnd, arrowHead1, pThickness, pColour);
			DrawLine(lineEnd, arrowHead2, pThickness, pColour);
		}

		public void DrawTriangle(Triangle pTriangle, Color pColour)
		{
			DrawTriangle(pTriangle.V1, pTriangle.V2, pTriangle.V3, pColour);
		}

		public void DrawTriangle(Vector2 pA, Vector2 pB, Vector2 pC, Color pColour)
		{
			EnsureStarted();

			const int shapeVertexCount = 3;
			const int shapeIndexCount = 3;

			EnsureSpace(shapeVertexCount, shapeIndexCount);

			_indices[_indexCount++] = (short)(0 + _vertexCount);
			_indices[_indexCount++] = (short)(1 + _vertexCount);
			_indices[_indexCount++] = (short)(2 + _vertexCount);

			_vertices[_vertexCount++] = new VertexPositionColor(new Vector3(pA.X, pA.Y, 0f), pColour);
			_vertices[_vertexCount++] = new VertexPositionColor(new Vector3(pB.X, pB.Y, 0f), pColour);
			_vertices[_vertexCount++] = new VertexPositionColor(new Vector3(pC.X, pC.Y, 0f), pColour);

			_shapeCount++;
		}
	}
}
