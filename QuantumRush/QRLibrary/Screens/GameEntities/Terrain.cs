using Microsoft.Xna.Framework;
using QRLibrary.Shapes;
using System;
using System.Reflection.Metadata.Ecma335;

namespace QRLibrary.Screens.GameEntities
{
	internal class TerrainChunk
	{
		public const int CHUNK_SIZE = 16;

	}

	internal class Terrain
	{
		public const int TERRAIN_COLS = 16;
		public const int TERRAIN_ROWS = 16;
		public const int CELL_WIDTH = 100;
		public const int CELL_HEIGHT = 100;

		private Vector2[,] _terrainVertices = new Vector2[TERRAIN_COLS + 1, TERRAIN_ROWS + 1];
		private Color[,] _cellColours = new Color[TERRAIN_COLS, TERRAIN_ROWS];

		public Triangle[,] _triangles = new Triangle[TERRAIN_COLS * 2, TERRAIN_ROWS * 2];

		public Vector2[,] Vertices { get { return _terrainVertices; } }
		public Color[,] Colours { get { return _cellColours; } }

		public int _targetCol = 5, _targetRow = 5;

		public Terrain()
		{
			Random rng = new Random(1);
			int dx = (int)CELL_WIDTH / 4;
			int dy = (int)CELL_HEIGHT / 4;
			for (int i = 0; i <= TERRAIN_COLS; i++)
			{
				for (int j = 0; j <= TERRAIN_ROWS; j++)
				{
					_terrainVertices[i, j] = new Vector2(i * CELL_WIDTH + rng.Next(-dx, dx), j * CELL_HEIGHT + rng.Next(-dy,dy));
				}
			}

			for (int i = 0; i < TERRAIN_COLS; i++)
			{
				for (int j = 0; j < TERRAIN_ROWS; j++)
				{
					if (i == 0 || j == 0 || i == TERRAIN_COLS - 1 || j == TERRAIN_ROWS - 1)
					{
						_cellColours[i, j] = Color.Black;
					}
					else
					{
						switch (rng.Next(1, 4))
						{
							case 1:
								_cellColours[i, j] = Color.Black;
								break;
							case 2:
								_cellColours[i, j] = Color.MediumPurple;
								break;
							case 3:
								_cellColours[i, j] = Color.MediumVioletRed;
								break;
						}
					}
				}
			}

			for (int i = 0; i < TERRAIN_COLS; i++)
			{
				for (int j = 0; j < TERRAIN_ROWS; j++)
				{
					_triangles[2 * i, 2 * j] = new Triangle(Vertices[i, j], Vertices[i + 1, j + 1], Vertices[i + 1, j]);
					_triangles[2 * i + 1, 2 * j + 1] = new Triangle(Vertices[i, j], Vertices[i, j + 1], Vertices[i + 1, j + 1]);
				}
			}

			_cellColours[_targetCol, _targetRow] = Color.LimeGreen;
		}

		public void UpdateMouse(Vector2 v)
		{
			for (int i = 0; i < TERRAIN_COLS; i++)
			{
				for (int j = 0; j < TERRAIN_ROWS; j++)
				{
					if (_triangles[2 * i,2 * j].IsInside(v) || _triangles[2 * i + 1, 2 * j + 1].IsInside(v))
					{
						_cellColours[i, j] = Color.Blue;
					}
				}
			}
		}

		public bool CheckPlayerCollision(Player player)
		{
			for (int i = 0; i < TERRAIN_COLS; i++)
			{
				for (int j = 0; j < TERRAIN_ROWS; j++)
				{
					if (_cellColours[i, j] == Color.Black)
					{
						if (_triangles[2 * i, 2 * j].IntersectsCircle(player.Circle) || _triangles[2 * i + 1, 2 * j + 1].IntersectsCircle(player.Circle))
						{
							return true;
						}
					}
				}
			}
			return false;
		}

		public void ChangeTarget()
		{
			_cellColours[_targetCol, _targetRow] = Color.MediumVioletRed;
			int newCol, newRow;
			Random rng = new Random();
			do
			{
				newCol = rng.Next(_targetCol - 5, _targetCol + 5);
				newRow = rng.Next(_targetRow - 5, _targetRow + 5);

			} while (newCol < 0 || newRow < 0 || newCol >= TERRAIN_COLS || newRow >= TERRAIN_ROWS || _cellColours[newCol, newRow] == Color.Black);

			_targetCol = newCol;
			_targetRow = newRow;
			_cellColours[_targetCol, _targetRow] = Color.LimeGreen;
		}

		public bool ReachedTarget(Vector2 playerPosition)
		{
			Vector2 centreOfCell = (_terrainVertices[_targetCol, _targetRow] +
				_terrainVertices[_targetCol + 1, _targetRow] +
				_terrainVertices[_targetCol + 1, _targetRow + 1] +
				_terrainVertices[_targetCol, _targetRow + 1]) * 0.25f;

			return Vector2.DistanceSquared(centreOfCell, playerPosition) < CELL_WIDTH * CELL_WIDTH;
		}
	}
}
