﻿using System;
using System.Collections.Generic;
using System.Text;
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
		Bomb,
		Fire
	}

	class Playground
	{
		private const int playgroundSize = 12; // must be greater then 6!

		Square[][] board;

		public Playground()
		{
			this.board = new Square[playgroundSize][];
			for (int i = 0; i < playgroundSize; i++)
			{
				this.board[i] = new Square[playgroundSize];
			}
			InitPlayground();
		}
		private void InitPlayground()
		{
			//#region Borders

			//#endregion
			#region Top-left corner
			board[0][0] = Square.Player_1;
			board[1][0] = Square.Empty;
			board[1][1] = Square.Empty;
			board[0][1] = Square.Empty;
			board[2][0] = Square.Wall;
			board[2][1] = Square.Wall;
			board[1][2] = Square.Wall;
			board[0][2] = Square.Wall;
			board[2][2] = Square.Unbreakable_Wall;
			#endregion
			#region Top-right corner
			board[0][playgroundSize - 1] = Square.Player_2;
			board[0][playgroundSize - 2] = Square.Empty;
			board[1][playgroundSize - 2] = Square.Empty;
			board[1][playgroundSize - 1] = Square.Empty;
			board[0][playgroundSize - 3] = Square.Wall;
			board[1][playgroundSize - 3] = Square.Wall;
			board[2][playgroundSize - 2] = Square.Wall;
			board[2][playgroundSize - 1] = Square.Wall;
			board[2][playgroundSize - 3] = Square.Unbreakable_Wall;
			#endregion
			#region Bottom-left corner
			board[playgroundSize - 1][0] = Square.Player_3;
			board[playgroundSize - 2][0] = Square.Empty;
			board[playgroundSize - 2][1] = Square.Empty;
			board[playgroundSize - 1][1] = Square.Empty;
			board[playgroundSize - 3][0] = Square.Wall;
			board[playgroundSize - 3][1] = Square.Wall;
			board[playgroundSize - 2][2] = Square.Wall;
			board[playgroundSize - 1][2] = Square.Wall;
			board[playgroundSize - 3][2] = Square.Unbreakable_Wall;
			#endregion
			#region Bottom-right corner
			board[playgroundSize - 1][playgroundSize - 1] = Square.Player_4;
			board[playgroundSize - 2][playgroundSize - 1] = Square.Empty;
			board[playgroundSize - 2][playgroundSize - 2] = Square.Empty;
			board[playgroundSize - 1][playgroundSize - 2] = Square.Empty;
			board[playgroundSize - 3][playgroundSize - 1] = Square.Wall;
			board[playgroundSize - 3][playgroundSize - 2] = Square.Wall;
			board[playgroundSize - 2][playgroundSize - 3] = Square.Wall;
			board[playgroundSize - 1][playgroundSize - 3] = Square.Wall;
			board[playgroundSize - 3][playgroundSize - 3] = Square.Unbreakable_Wall;
			#endregion
			do
			{
				InitPlaygroundCenter();
			} while (!CheckFeasibility());
		}
		private void InitPlaygroundCenter()
		{
			Random random = new Random();
			for (int i = 0; i < 3; i++)
			{
				for (int j = 3; j < (playgroundSize - 3); j++)
				{
					board[i][j] = (Square)(random.Next(3) + 4);
					board[j][i] = (Square)(random.Next(3) + 4);
				}
			}
			for (int i = playgroundSize - 3; i < playgroundSize; i++)
			{
				for (int j = 3; j < playgroundSize - 3; j++)
				{
					board[i][j] = (Square)(random.Next(3) + 4);
					board[j][i] = (Square)(random.Next(3) + 4);
				}
			}
			for (int i = 3; i < playgroundSize - 3; i++)
			{
				for (int j = 3; j < playgroundSize - 3; j++)
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
			bool[][] field = new bool[playgroundSize + 2][];
			Stack<Point> stack = new Stack<Point>();

			for (int i = 0; i < playgroundSize + 2; i++)
			{
				field[i] = new bool[playgroundSize + 2];
			}
			for (int i = 1; i < playgroundSize + 1; i++)
			{
				for (int j = 1; j < playgroundSize + 1; j++)
				{
					field[i][j] = false;
				}
			}
			for (int i = 0; i < playgroundSize + 2; i++)
			{
				field[0][i] = true;
				field[playgroundSize + 1][i] = true;
				field[i][0] = true;
				field[i][playgroundSize + 1] = true;
			}
			field[1][1] = true;
			stack.Push(new Point(1, 1));
			do
			{
				Point p = stack.Pop();
				if ((field[p.X][p.Y + 1] == false) && (board[p.X - 1][p.Y] != Square.Unbreakable_Wall))
					stack.Push(new Point(p.X, p.Y + 1)); field[p.X][p.Y + 1] = true;
				if ((field[p.X + 1][p.Y] == false) && (board[p.X][p.Y - 1] != Square.Unbreakable_Wall))
					stack.Push(new Point(p.X + 1, p.Y)); field[p.X + 1][p.Y] = true;
				if ((field[p.X][p.Y - 1] == false) && (board[p.X - 1][p.Y - 2] != Square.Unbreakable_Wall))
					stack.Push(new Point(p.X, p.Y - 1)); field[p.X][p.Y - 1] = true;
				if ((field[p.X - 1][p.Y] == false) && (board[p.X - 2][p.Y - 1] != Square.Unbreakable_Wall))
					stack.Push(new Point(p.X - 1, p.Y)); field[p.X - 1][p.Y] = true;
			} while (stack.Count != 0);
			return (field[1][playgroundSize] & field[playgroundSize][1] & field[playgroundSize][playgroundSize]);
		}
	}
}
