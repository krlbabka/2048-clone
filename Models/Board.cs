using System;
using System.Collections.Generic;

namespace _2048.Models
{
    public class Board
    {
        private const int Size = 4;
        private readonly Tile[,] _tiles = new Tile[Size,Size];
        private readonly Random _random = new Random();

        public Board()
        {
            ResetBoard();
        }
        
        private void ResetBoard()
        {
            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                {
                    _tiles[i, j] = new Tile(0, i, j);
                }
            }

            AddRandomTile();
            AddRandomTile();
            Display();
        }

        private void AddRandomTile()
        {
            List<(int, int)> availableTiles = new List<(int, int)>();

            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                {
                    if (_tiles[i,j].Value == 0)
                    {
                        availableTiles.Add((i,j));
                    }
                }
            }

            if (availableTiles.Count > 0)
            {
                var position = availableTiles[_random.Next(availableTiles.Count)];
                _tiles[position.Item1, position.Item2] = new Tile(_random.Next(0, 10) == 0 ? 4 : 2, position.Item1, position.Item2);
            }
        }

        public void Move(string direction)
        {
            switch (direction)
            {
                case "L":
                    MoveLeft();
                    break;
                case "U":
                    MoveUp();
                    break;
                case "R":
                    MoveRight();
                    break;
                case "D":
                    MoveDown();
                    break;
            }
        }

        private void MoveLeft()
        {
            for (var row = 0; row < Size; row++)
            {
                var tiles = FetchTiles(true, row, false);
                CombineTiles(tiles);
                
                // Place the tiles back and reset merge status
                for (var i = 0; i < Size; i++)
                {
                    if (i < tiles.Count)
                    {
                        _tiles[row, i] = tiles[i];
                        _tiles[row, i].Column = i;
                        _tiles[row, i].SetMergeStatus(false);
                    }
                    else
                    {
                        _tiles[row, i] = new Tile(0, row, i);
                    }
                }
            }
        }

        private void MoveUp()
        {
            for (var col = 0; col < Size; col++)
            {
                var tiles = FetchTiles(false, col, false);
                CombineTiles(tiles);
                
                // Place the tiles back and reset merge status
                for (var i = 0; i < Size; i++)
                {
                    if (i < tiles.Count)
                    {
                        _tiles[i, col] = tiles[i];
                        _tiles[i, col].Row = i;
                        _tiles[i, col].SetMergeStatus(false);
                    }
                    else
                    {
                        _tiles[i, col] = new Tile(0, i, col);
                    }
                }
            }
        }

        private void MoveRight()
        {
            for (var row = 0; row < Size; row++)
            {
                var tiles = FetchTiles(true, row, true);
                CombineTiles(tiles);
                
                // Place the tiles back starting from the rightmost column and reset merge status
                for (var i = Size - 1; i >= 0; i--)
                {
                    if (i >= Size - tiles.Count)
                    {
                        _tiles[row, i] = tiles[Size - 1 - i];
                        _tiles[row, i].Column = i;
                        _tiles[row, i].SetMergeStatus(false);
                    }
                    else
                    {
                        _tiles[row, i] = new Tile(0, row, i);
                    }
                }
            }
        }

        public void MoveDown()
        {
            for (var col = 0; col < Size; col++)
            {
                var tiles = FetchTiles(false, col, true);
                CombineTiles(tiles);
                
                // Place the tiles back and reset merge status
                for (var i = Size - 1; i >= 0; i--)
                {
                    if (i >= Size - tiles.Count)
                    {
                        _tiles[i, col] = tiles[Size - 1 - i];
                        _tiles[i, col].Row = i;
                        _tiles[i, col].SetMergeStatus(false);
                    }
                    else
                    {
                        _tiles[i, col] = new Tile(0, i, col);
                    }
                }
            }
        }

        private List<Tile> FetchTiles(bool isRow, int index, bool isReverse)
        {
            var tiles = new List<Tile>();
            for (var i = 0; i < Size; i++)
            {
                var tile = isRow ? _tiles[index, isReverse ? Size - 1 - i : i] : _tiles[isReverse ? Size - 1 - i : i, index];
                if (tile.Value != 0)
                {
                    tiles.Add(tile);
                }
            }
            return tiles;
        }
        
        private void CombineTiles(List<Tile> tiles)
        {
            for (var i = 0; i < tiles.Count - 1; i++)
            {
                if (tiles[i].Value == tiles[i + 1].Value && !tiles[i].HasMerged && !tiles[i + 1].HasMerged)
                {
                    tiles[i].Value *= 2;
                    tiles[i].SetMergeStatus(true);
                    tiles.RemoveAt(i + 1);
                }
            }
        }
        
        public bool HasMovesLeft()
        {
            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                {
                    if (_tiles[i, j].Value == 0) return true;
                }
            }
            
            // TODO: cases whether moves are possible
            return false;
        }
        
        private void Display()
        {
            for (var i = 0; i < Size; i++)
            {
                for (var j = 0; j < Size; j++)
                {
                    Console.Write($"{_tiles[i,j].Value}\t");
                }
                Console.WriteLine();
            }
        }
        
        public void DebugPlay()
        {
            while (true)
            {
                Console.WriteLine(" ----- ");
                var input = Console.ReadLine();

                if (input == "R" || input == "L" || input == "U" || input == "D")
                {
                    Move(input);
            
                    // After each move, add a random tile.
                    AddRandomTile();
                    Display();
                }
                else
                {
                    Console.WriteLine("Invalid input. Please enter R, L, U, or D.");
                }
        
                // Check if the game is over
                if (!HasMovesLeft())
                {
                    Console.WriteLine("Game Over!");
                    break;
                }
            }
        }
    }
}