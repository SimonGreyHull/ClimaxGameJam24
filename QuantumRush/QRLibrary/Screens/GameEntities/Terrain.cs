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
		public const int TERRAIN_COLS = 64;
		public const int TERRAIN_ROWS = 64;
		public const int CELL_WIDTH = 100;
		public const int CELL_HEIGHT = 100;

		private Vector2[,] _terrainVertices = new Vector2[TERRAIN_COLS + 1, TERRAIN_ROWS + 1];
		private Color[,] _cellColours = new Color[TERRAIN_COLS, TERRAIN_ROWS];

		public Vector2[,] Vertices { get { return _terrainVertices; } }
		public Color[,] Colours { get { return _cellColours; } }

		public Terrain()
		{
			Random rng = new Random(1);
			int dx = 0;// (int)CELL_WIDTH / 4;
			int dy = 0;// (int)CELL_HEIGHT / 4;
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
		}
	}
}
