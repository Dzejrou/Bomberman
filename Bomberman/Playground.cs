﻿using System;
using System.Collections.Generic;
using System.Drawing;

namespace Bomberman
{
	enum Square
	{
		Player_1,
		Player_2,
		Player_3,
		Player_4,
		Empty,
		Wall,
		Unbreakable_Wall,
		Bomb_1,
		Bomb_1_1,
		Bomb_1_2,
		Bomb_1_3,
		Bomb_1_4,
		Bomb_2,
		Bomb_2_1,
		Bomb_2_2,
		Bomb_2_3,
		Bomb_2_4,
		Bomb_3,
		Bomb_3_1,
		Bomb_3_2,
		Bomb_3_3,
		Bomb_3_4,
		Fire
	}

	class Playground
	{
		public static int playgroundSize = 14; // must be greater or eaqual to 7! 14
		public Square[][] board;
		private Queue<Point> bombs = new Queue<Point>();
		private Queue<Point> fire = new Queue<Point>();

		public Playground()
		{
			this.board = new Square[playgroundSize][];
			for (int i = 0; i < playgroundSize; i++)
			{
				this.board[i] = new Square[playgroundSize];
			}
			InitPlayground();
		}
		public Playground(int size)
		{
			playgroundSize = size;
			this.board = new Square[playgroundSize][];
			for (int i = 0; i < playgroundSize; i++)
			{
				this.board[i] = new Square[playgroundSize];
			}
		}
		internal void AddBomb(Point location)
		{
			if (!bombs.Contains(location)) bombs.Enqueue(location);
		}
		internal void UpdateBombsFire()
		{
			int fireCount = fire.Count;
			for (int i = 0; i < fireCount; i++)
			{
				Point location = fire.Dequeue();
				if (board[location.X][location.Y] == Square.Fire)
				{
					board[location.X][location.Y] = Square.Empty;
					GameLogic.changes.Add(new Change(new Point(location.X, location.Y), Square.Empty));
				}
			}
			int bombCount = bombs.Count;
			for (int i = 0; i < bombCount; i++)
			{
				Point location = bombs.Dequeue();
				switch (board[location.X][location.Y])
				{
					case Square.Bomb_1:
						board[location.X][location.Y] = Square.Bomb_2;
						GameLogic.changes.Add(new Change(new Point(location.X, location.Y), Square.Bomb_2));
						bombs.Enqueue(location);
						break;
					case Square.Bomb_2:
						//board[location.X][location.Y] = Square.Bomb_3;
						//bombs.Enqueue(location);
						Explode(location);
						break;
					//case Square.Bomb_3:
					//	Explode(location);
					//	break;
					case Square.Player_1:
						board[location.X][location.Y] = Square.Bomb_1_1;
						GameLogic.changes.Add(new Change(new Point(location.X, location.Y), Square.Bomb_1_1));
						bombs.Enqueue(location);
						break;
					case Square.Player_2:
						board[location.X][location.Y] = Square.Bomb_1_2;
						GameLogic.changes.Add(new Change(new Point(location.X, location.Y), Square.Bomb_1_2));
						bombs.Enqueue(location);
						break;
					case Square.Player_3:
						board[location.X][location.Y] = Square.Bomb_1_3;
						GameLogic.changes.Add(new Change(new Point(location.X, location.Y), Square.Bomb_1_3));
						bombs.Enqueue(location);
						break;
					case Square.Player_4:
						board[location.X][location.Y] = Square.Bomb_1_4;
						GameLogic.changes.Add(new Change(new Point(location.X, location.Y), Square.Bomb_1_4));
						bombs.Enqueue(location);
						break;
					case Square.Bomb_1_1:
						board[location.X][location.Y] = Square.Bomb_2_1;
						GameLogic.changes.Add(new Change(new Point(location.X, location.Y), Square.Bomb_2_1));
						bombs.Enqueue(location);
						break;
					case Square.Bomb_1_2:
						board[location.X][location.Y] = Square.Bomb_2_2;
						GameLogic.changes.Add(new Change(new Point(location.X, location.Y), Square.Bomb_2_2));
						bombs.Enqueue(location);
						break;
					case Square.Bomb_1_3:
						board[location.X][location.Y] = Square.Bomb_2_3;
						GameLogic.changes.Add(new Change(new Point(location.X, location.Y), Square.Bomb_2_3));
						bombs.Enqueue(location);
						break;
					case Square.Bomb_1_4:
						board[location.X][location.Y] = Square.Bomb_2_4;
						GameLogic.changes.Add(new Change(new Point(location.X, location.Y), Square.Bomb_2_4));
						bombs.Enqueue(location);
						break;
					case Square.Bomb_2_1:
					case Square.Bomb_2_2:
					case Square.Bomb_2_3:
					case Square.Bomb_2_4:
						Explode(location);
						break;
					//case Square.Bomb_3_1:
					//case Square.Bomb_3_2:
					//case Square.Bomb_3_3:
					//case Square.Bomb_3_4:
					//	Explode(location);
					//	break;
					default:
						bombs.Enqueue(location);
						break;
				}
			}
		}
		private void Explode(Point location)
		{
			SetFire(location, true);
			SetFire(new Point(location.X - 1, location.Y), false);
			SetFire(new Point(location.X, location.Y - 1), false);
			SetFire(new Point(location.X + 1, location.Y), false);
			SetFire(new Point(location.X, location.Y + 1), false);
		}
		/// <summary>
		/// Set fire to location if it is possible
		/// </summary>
		/// <param name="location"></param>
		/// <param name="center">if the location is where the bomb explode</param>
		private void SetFire(Point location, bool center)
		{
			Square squere = board[location.X][location.Y];
			if (squere == Square.Unbreakable_Wall) return;
			if ((!center)&&(squere >= Square.Bomb_1 && squere <= Square.Bomb_3_4)) Explode(location);
			if ((squere >= Square.Player_1 && squere <= Square.Player_4) ||
				(squere >= Square.Bomb_1_1 && squere <= Square.Bomb_1_4) ||
				(squere >= Square.Bomb_2_1 && squere <= Square.Bomb_2_4) ||
				(squere >= Square.Bomb_3_1 && squere <= Square.Bomb_3_4)) Server.Dead(location);
			board[location.X][location.Y] = Square.Fire;
			GameLogic.changes.Add(new Change(new Point(location.X, location.Y), Square.Fire));
			fire.Enqueue(location);
		}
		/// <summary>
		/// Set borders and corners with players
		/// </summary>
		private void InitPlayground()
		{
			#region Borders
			for (int i = 0; i < playgroundSize; i++)
			{
				board[0][i] = Square.Unbreakable_Wall;
				board[playgroundSize - 1][i] = Square.Unbreakable_Wall;
				board[i][0] = Square.Unbreakable_Wall;
				board[i][playgroundSize - 1] = Square.Unbreakable_Wall;
			}
			#endregion
			#region Top-left corner
			board[1][1] = Square.Player_1;
			board[2][1] = Square.Empty;
			board[2][2] = Square.Empty;
			board[1][2] = Square.Empty;
			board[3][1] = Square.Wall;
			board[3][2] = Square.Wall;
			board[2][3] = Square.Wall;
			board[1][3] = Square.Wall;
			board[3][3] = Square.Unbreakable_Wall;
			#endregion
			#region Top-right corner
			board[1][playgroundSize - 2] = Square.Player_2;
			board[1][playgroundSize - 3] = Square.Empty;
			board[2][playgroundSize - 3] = Square.Empty;
			board[2][playgroundSize - 2] = Square.Empty;
			board[1][playgroundSize - 4] = Square.Wall;
			board[2][playgroundSize - 4] = Square.Wall;
			board[3][playgroundSize - 3] = Square.Wall;
			board[3][playgroundSize - 2] = Square.Wall;
			board[3][playgroundSize - 4] = Square.Unbreakable_Wall;
			#endregion
			#region Bottom-left corner
			board[playgroundSize - 2][1] = Square.Player_3;
			board[playgroundSize - 3][1] = Square.Empty;
			board[playgroundSize - 3][2] = Square.Empty;
			board[playgroundSize - 2][2] = Square.Empty;
			board[playgroundSize - 4][1] = Square.Wall;
			board[playgroundSize - 4][2] = Square.Wall;
			board[playgroundSize - 3][3] = Square.Wall;
			board[playgroundSize - 2][3] = Square.Wall;
			board[playgroundSize - 4][3] = Square.Unbreakable_Wall;
			#endregion
			#region Bottom-right corner
			board[playgroundSize - 2][playgroundSize - 2] = Square.Player_4;
			board[playgroundSize - 3][playgroundSize - 2] = Square.Empty;
			board[playgroundSize - 3][playgroundSize - 3] = Square.Empty;
			board[playgroundSize - 2][playgroundSize - 3] = Square.Empty;
			board[playgroundSize - 4][playgroundSize - 2] = Square.Wall;
			board[playgroundSize - 4][playgroundSize - 3] = Square.Wall;
			board[playgroundSize - 3][playgroundSize - 4] = Square.Wall;
			board[playgroundSize - 2][playgroundSize - 4] = Square.Wall;
			board[playgroundSize - 4][playgroundSize - 4] = Square.Unbreakable_Wall;
			#endregion
			do
			{
				InitPlaygroundCenter();
			} while (!CheckFeasibility());
		}
		/// <summary>
		/// Set randomly rest of board
		/// </summary>
		private void InitPlaygroundCenter()
		{
			Random random = new Random();
			for (int i = 1; i < 4; i++)
			{
				for (int j = 4; j < playgroundSize - 4; j++)
				{
					board[i][j] = (Square)(random.Next(3) + 4);
					board[j][i] = (Square)(random.Next(3) + 4);
				}
			}
			for (int i = playgroundSize - 4; i < playgroundSize - 1; i++)
			{
				for (int j = 4; j < playgroundSize - 4; j++)
				{
					board[i][j] = (Square)(random.Next(3) + 4);
					board[j][i] = (Square)(random.Next(3) + 4);
				}
			}
			for (int i = 4; i < playgroundSize - 4; i++)
			{
				for (int j = 4; j < playgroundSize - 4; j++)
				{
					board[i][j] = (Square)(random.Next(3) + 4);
				}
			}
		}
		/// <summary>
		/// Findeout if every player can reach other players
		/// </summary>
		/// <returns></returns>
		private bool CheckFeasibility()
		{
			bool[][] field = new bool[playgroundSize][];
			Stack<Point> stack = new Stack<Point>();

			for (int i = 0; i < playgroundSize; i++)
			{
				field[i] = new bool[playgroundSize];
			}
			for (int i = 1; i < playgroundSize - 1; i++)
			{
				for (int j = 1; j < playgroundSize - 1; j++)
				{
					field[i][j] = false;
				}
			}
			for (int i = 0; i < playgroundSize; i++)
			{
				field[0][i] = true;
				field[playgroundSize - 1][i] = true;
				field[i][0] = true;
				field[i][playgroundSize - 1] = true;
			}
			field[1][1] = true;
			stack.Push(new Point(1, 1));
			do
			{
				Point p = stack.Pop();
				if ((field[p.X][p.Y + 1] == false) && (board[p.X][p.Y + 1] != Square.Unbreakable_Wall))
					stack.Push(new Point(p.X, p.Y + 1)); field[p.X][p.Y + 1] = true;
				if ((field[p.X + 1][p.Y] == false) && (board[p.X + 1][p.Y] != Square.Unbreakable_Wall))
					stack.Push(new Point(p.X + 1, p.Y)); field[p.X + 1][p.Y] = true;
				if ((field[p.X][p.Y - 1] == false) && (board[p.X][p.Y - 1] != Square.Unbreakable_Wall))
					stack.Push(new Point(p.X, p.Y - 1)); field[p.X][p.Y - 1] = true;
				if ((field[p.X - 1][p.Y] == false) && (board[p.X - 1][p.Y] != Square.Unbreakable_Wall))
					stack.Push(new Point(p.X - 1, p.Y)); field[p.X - 1][p.Y] = true;
			} while (stack.Count != 0);
			return (field[1][playgroundSize - 2] & field[playgroundSize - 2][1] & field[playgroundSize - 2][playgroundSize - 2]);
		}
	}
}
