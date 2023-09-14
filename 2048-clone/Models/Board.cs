﻿using System;
using System.Collections.Generic;

namespace _2048.Models
{
    public class Board
    {
        private const int Size = 4;
        private readonly Tile[,] _tiles = new Tile[Size,Size];
        private readonly Random _random = new Random();
        public int Score { get; private set; }

        public Board()
        {
            ResetBoard();
        }
        
        public enum Direction
        {
            Up,
            Down,
            Left,
            Right
        }
        
        public void ResetBoard()
        {
            for (var i = 0; i < Size; i++)
            {
                for (var j = 0; j < Size; j++)
                {
                    _tiles[i, j] = new Tile(0, i, j);
                }
            }

            AddRandomTile();
            AddRandomTile();
            Score = 0;
        }

        public Tile GetTileAt(int row, int col)
        {
            return _tiles[row, col];
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

        public void Move(Direction direction)
        {
            if (!CanMove(direction, _tiles)) return;
            
            switch (direction)
            {
                case Direction.Left:
                    MoveLeft();
                    break;
                case Direction.Up:
                    MoveUp();
                    break;
                case Direction.Right:
                    MoveRight();
                    break;
                case Direction.Down:
                    MoveDown();
                    break;
            }
            
            // After each move, add a random tile.
            AddRandomTile();
        }

        public void MoveLeft()
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

        public void MoveUp()
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

        public void MoveRight()
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
                    Score += tiles[i].Value;
                }
            }
        }
        
        public bool HasMovesLeft()
        {
            // Check for empty tiles
            for (var i = 0; i < Size; i++)
            {
                for (var j = 0; j < Size; j++)
                {
                    if (_tiles[i, j].Value == 0) 
                        return true;
                }
            }
            
            // Check for possible merge
            for (var i = 0; i < Size; i++)
            {
                for (var j = 0; j < Size-1; j++)
                {
                    if (_tiles[i, j].Value == _tiles[i, j+1].Value) 
                        return true;
                }
            }
            
            // Check for possible merge
            for (var j = 0; j < Size; j++)
            {
                for (var i = 0; i < Size-1; i++)
                {
                    if (_tiles[i, j].Value == _tiles[i+1, j].Value) 
                        return true;
                }
            }
            
            return false;
        }
        
        bool CanMove(Direction direction, Tile[,] tiles)
        {
            switch (direction)
            {
                case Direction.Up:
                    for (int j = 0; j < Size; j++)
                    {
                        for (int i = 1; i < Size; i++)
                        {
                            if (tiles[i, j].Value != 0) 
                            {
                                if (tiles[i-1, j].Value == 0 || tiles[i-1, j].Value == tiles[i, j].Value)
                                    return true;
                            }
                        }
                    }
                    break;

                case Direction.Down:
                    for (int j = 0; j < Size; j++)
                    {
                        for (int i = Size - 2; i >= 0; i--)
                        {
                            if (tiles[i, j].Value != 0) 
                            {
                                if (tiles[i+1, j].Value == 0 || tiles[i+1, j].Value == tiles[i, j].Value)
                                    return true;
                            }
                        }
                    }
                    break;

                case Direction.Left:
                    for (int i = 0; i < Size; i++)
                    {
                        for (int j = 1; j < Size; j++)
                        {
                            if (tiles[i, j].Value != 0) 
                            {
                                if (tiles[i, j-1].Value == 0 || tiles[i, j-1].Value == tiles[i, j].Value)
                                    return true;
                            }
                        }
                    }
                    break;

                case Direction.Right:
                    for (int i = 0; i < Size; i++)
                    {
                        for (int j = Size - 2; j >= 0; j--)
                        {
                            if (tiles[i, j].Value != 0) 
                            {
                                if (tiles[i, j+1].Value == 0 || tiles[i, j+1].Value == tiles[i, j].Value)
                                    return true;
                            }
                        }
                    }
                    break;
            }

            return false;
        }
    }
}