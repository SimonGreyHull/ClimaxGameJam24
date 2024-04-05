using Microsoft.Xna.Framework;
using QRLibrary.Shapes;
using System.Reflection.Metadata.Ecma335;

namespace QRLibrary.Screens.GameEntities
{
	internal class Terrain
	{
		public const int TERRAIN_COLS = 8;
		public const int TERRAIN_ROWS = 8;
		public const float CELL_WIDTH = 100;
		public const float CELL_HEIGHT = 100;

		private Vector2[,] _terrainVertices = new Vector2[TERRAIN_COLS + 1, TERRAIN_ROWS + 1];

		public Vector2[,] Vertices { get { return _terrainVertices; } }

		public Terrain()
		{
			for (int i = 0; i <= TERRAIN_COLS; i++)
			{
				for (int j = 0; j <= TERRAIN_ROWS; j++)
				{
					_terrainVertices[i, j] = new Vector2(i * CELL_WIDTH, j * CELL_HEIGHT);
				}
			}
		}
	}
}
