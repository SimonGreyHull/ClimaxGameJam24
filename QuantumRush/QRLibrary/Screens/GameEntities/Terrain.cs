﻿using Microsoft.Xna.Framework;
using QRLibrary.Shapes;
using System;

namespace QRLibrary.Screens.GameEntities
{
	internal class TerrainChunk
	{
		public const int CHUNK_SIZE = 16;

	}

	internal class TerrainCellData
	{
		public Terrain.CELL_TYPE Type { get; private set; }
		public int stepsToPlayer { get; private set; }
		public int stepsToTarget { get; set; }

		public Color Colour { get; private set; }

		public void SetStepsToPlayer(int steps)
		{
			stepsToPlayer = steps;

			Color[] colors = new Color[10];

			int n = 0;
			for (int i = 0; i < colors.Length; i++)
			{
				n = 255 - i * 255 / colors.Length;
				colors[i] = new Color(n, n, n);
			}

			if (steps < colors.Length)
			{
				Colour = colors[steps];
			}
		}

		public void SetType(Terrain.CELL_TYPE type)
		{
			Type = type;
			switch (type)
			{
				case Terrain.CELL_TYPE.NONE:
					Colour = Color.MediumPurple;
					break;
				case Terrain.CELL_TYPE.WALL:
					Colour = Color.Black;
					break;
				case Terrain.CELL_TYPE.TARGET:
					Colour = Color.LimeGreen;
					break;
				case Terrain.CELL_TYPE.ENEMY_SPAWNER:
					Colour = Color.Red;
					break;
				case Terrain.CELL_TYPE.PLAYER:
					Colour = Color.HotPink;
					break;
			}
		}
	}

	internal class Terrain
	{
		public enum CELL_TYPE { WALL, TARGET, ENEMY_SPAWNER, PLAYER, NONE }

		public const int TERRAIN_COLS = 16;
		public const int TERRAIN_ROWS = 16;
		public const int CELL_WIDTH = 100;
		public const int CELL_HEIGHT = 100;

		public Bullet[] _bullets = new Bullet[100];
		public int _bulletCount = 0;

		public Enemy[] _enemies = new Enemy[100];
		public int _enemyCount = 0;

		private Vector2[,] _terrainVertices = new Vector2[TERRAIN_COLS + 1, TERRAIN_ROWS + 1];
		private TerrainCellData[,] _cellData = new TerrainCellData[TERRAIN_COLS, TERRAIN_ROWS];

		public Triangle[,] _triangles = new Triangle[TERRAIN_COLS * 2, TERRAIN_ROWS * 2];

		public Vector2[,] Vertices { get { return _terrainVertices; } }
		public TerrainCellData[,] CellData { get { return _cellData; } }

		public int _targetCol = 5, _targetRow = 5;
		public int _playerCol = 1, _playerRow = 1;

		public Terrain()
		{
			Random rng = new Random(1);
			int dx = (int)CELL_WIDTH / 4;
			int dy = (int)CELL_HEIGHT / 4;
			for (int i = 0; i <= TERRAIN_COLS; i++)
			{
				for (int j = 0; j <= TERRAIN_ROWS; j++)
				{
					_terrainVertices[i, j] = new Vector2(i * CELL_WIDTH + rng.Next(-dx, dx), j * CELL_HEIGHT + rng.Next(-dy, dy));
				}
			}

			for (int i = 0; i < TERRAIN_COLS; i++)
			{
				for (int j = 0; j < TERRAIN_ROWS; j++)
				{
					_cellData[i, j] = new TerrainCellData();
					if (i == 0 || j == 0 || i == TERRAIN_COLS - 1 || j == TERRAIN_ROWS - 1)
					{
						_cellData[i, j].SetType(CELL_TYPE.WALL);
					}
					else
					{
						switch (rng.Next(1, 4))
						{
							case 1:
								_cellData[i, j].SetType(CELL_TYPE.WALL);
								break;
							case 2:
								_cellData[i, j].SetType(CELL_TYPE.NONE);
								break;
							case 3:
								_cellData[i, j].SetType(CELL_TYPE.NONE);
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

			_cellData[_targetCol, _targetRow].SetType(CELL_TYPE.TARGET);
			_cellData[_playerCol, _playerRow].SetType(CELL_TYPE.PLAYER);
		}

		public void UpdateMouse(Vector2 v)
		{
			for (int i = 0; i < TERRAIN_COLS; i++)
			{
				for (int j = 0; j < TERRAIN_ROWS; j++)
				{
					if (_triangles[2 * i, 2 * j].IsInside(v) || _triangles[2 * i + 1, 2 * j + 1].IsInside(v))
					{
						//_cellColours[i, j] = Color.Blue;
					}
				}
			}
		}

		public void UpdateBullets(float seconds)
		{
			for (int i = 0; i < _bulletCount; i++)
			{
				_bullets[i].Update(seconds);
			}

			for (int i = _bulletCount - 1; i >= 0; i--)
			{
				if (CheckWallsCollision(_bullets[i].Circle))
				{
					_bullets[i] = _bullets[_bulletCount - 1];
					_bulletCount--;
				}
			}
		}

		public void AddBullet(Bullet bullet)
		{
			if (_bulletCount == _bullets.Length)
			{
				return;
			}

			_bullets[_bulletCount] = bullet;
			_bulletCount++;
		}

		public bool CheckWallsCollision(Circle circle)
		{
			for (int i = 0; i < TERRAIN_COLS; i++)
			{
				for (int j = 0; j < TERRAIN_ROWS; j++)
				{
					if (_cellData[i, j].Type == CELL_TYPE.WALL)
					{
						if (_triangles[2 * i, 2 * j].IntersectsCircle(circle) || _triangles[2 * i + 1, 2 * j + 1].IntersectsCircle(circle))
						{
							return true;
						}
					}
				}
			}
			return false;
		}

		public bool PointInCell(int col, int row, Vector2 point)
		{
			return (_triangles[2 * col, 2 * row].IsInside(point) ||
				_triangles[2 * col + 1, 2 * row + 1].IsInside(point));
		}

		private bool IsValidGridCell(int col, int row)
		{
			return col >= 0 && col < TERRAIN_COLS && row >= 0 && row < TERRAIN_ROWS;
		}


		public void SetStepsToPlayer(int col, int row, int steps)
		{
			if(!IsValidGridCell(col, row))
			{
				return;
			}

			if (_cellData[col, row].Type == CELL_TYPE.WALL)
			{
				return;
			}

			if (_cellData[col, row].stepsToPlayer > steps)
			{
				_cellData[col, row].SetStepsToPlayer(steps);

				SetStepsToPlayer(col + 1, row, steps + 1);
				SetStepsToPlayer(col - 1, row, steps + 1);
				SetStepsToPlayer(col, row + 1, steps + 1);
				SetStepsToPlayer(col, row - 1, steps + 1);
			}
		}

		public void UpdatePlayerDistances(int col, int row)
		{
			for(int i = 0; i < TERRAIN_COLS; i++)
			{
				for(int j = 0; j < TERRAIN_ROWS; j++)
				{
					_cellData[i, j].SetStepsToPlayer(int.MaxValue);
				}
			}

			SetStepsToPlayer(col, row, 0);
		}

		public void UpdatePlayerCell(Player player)
		{
			if (PointInCell(_playerCol, _playerRow, player.Position))
			{
				return;
			}

			int lowCol = Math.Max(_playerCol - 1, 0);
			int highCol = Math.Min(_playerCol + 1, TERRAIN_COLS - 1);
			int lowRow = Math.Max(_playerRow - 1, 0);
			int highRow = Math.Min(_playerRow + 1, TERRAIN_ROWS - 1);

			for(int i = lowCol; i <= highCol; i++)
			{
				for(int j = lowRow; j <= highRow; j++)
				{
					if(PointInCell(i, j, player.Position))
					{
						_playerCol = i;
						_playerRow = j;
						UpdatePlayerDistances(i, j);
						return;
					}
				}
			}

		}

		public bool CheckPlayerCollision(Player player)
		{
			UpdatePlayerCell(player);
			return CheckWallsCollision(player.Circle);
		}

		public void ChangeTarget()
		{
			_cellData[_targetCol, _targetRow].SetType(CELL_TYPE.NONE);
			int newCol, newRow;
			Random rng = new Random();
			do
			{
				newCol = rng.Next(_targetCol - 5, _targetCol + 5);
				newRow = rng.Next(_targetRow - 5, _targetRow + 5);

			} while (newCol < 0 || newRow < 0 || newCol >= TERRAIN_COLS || newRow >= TERRAIN_ROWS || _cellData[newCol, newRow].Type == CELL_TYPE.WALL);

			_targetCol = newCol;
			_targetRow = newRow;
			_cellData[_targetCol, _targetRow].SetType(CELL_TYPE.TARGET);
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
