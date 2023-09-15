using System;
using System.Collections.Generic;

namespace _2048.Models
{
    public class Board
    {
        private const int Size = 4;
        public readonly Tile[,] tiles = new Tile[Size,Size];
        private readonly Random random = new Random();
        public int Score { get; set; }
        
        
        // Directions for possible tile moves.
        public enum Direction
        {
            Up,
            Down,
            Left,
            Right
        }
        
        // Initializes the board with all zeros and two random tiles.
        public Board()
        {
            ResetBoard();
        }
        
        // Resets the board to its initial state.
        public void ResetBoard()
        {
            // Initialize tiles in board with zeros.
            for (var i = 0; i < Size; i++)
            {
                for (var j = 0; j < Size; j++)
                {
                    tiles[i, j] = new Tile(0, i, j);
                }
            }

            // Add two random tiles and reset score for game start
            AddRandomTile();
            AddRandomTile();
            Score = 0;
        }

        public Tile GetTileAt(int row, int col)
        {
            return tiles[row, col];
        }
        
        // Add a random 2(90%) or 4(10%) tile to an available spot on the board.
        public void AddRandomTile()
        {
            List<(int, int)> availableTiles = new List<(int, int)>();

            // Empty tiles
            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                {
                    if (tiles[i,j].Value == 0)
                    {
                        availableTiles.Add((i,j));
                    }
                }
            }
            
            // Add a random tile into an empty spot
            if (availableTiles.Count > 0)
            {
                var position = availableTiles[random.Next(availableTiles.Count)];
                tiles[position.Item1, position.Item2] = new Tile(random.Next(0, 10) == 0 ? 4 : 2, position.Item1, position.Item2);
            }
        }

        // Moves tiles in the specified direction
        public void Move(Direction direction)
        {
            if (!CanMove(direction, tiles)) return;
            
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
        }

        // Methods for moving tiles in each direction.
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
                        this.tiles[row, i] = tiles[i];
                        this.tiles[row, i].Column = i;
                        this.tiles[row, i].SetMergeStatus(false);
                    }
                    else
                    {
                        this.tiles[row, i] = new Tile(0, row, i);
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
                        this.tiles[i, col] = tiles[i];
                        this.tiles[i, col].Row = i;
                        this.tiles[i, col].SetMergeStatus(false);
                    }
                    else
                    {
                        this.tiles[i, col] = new Tile(0, i, col);
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
                
                // Place the tiles back and reset merge status
                for (var i = Size - 1; i >= 0; i--)
                {
                    if (i >= Size - tiles.Count)
                    {
                        this.tiles[row, i] = tiles[Size - 1 - i];
                        this.tiles[row, i].Column = i;
                        this.tiles[row, i].SetMergeStatus(false);
                    }
                    else
                    {
                        this.tiles[row, i] = new Tile(0, row, i);
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
                        this.tiles[i, col] = tiles[Size - 1 - i];
                        this.tiles[i, col].Row = i;
                        this.tiles[i, col].SetMergeStatus(false);
                    }
                    else
                    {
                        this.tiles[i, col] = new Tile(0, i, col);
                    }
                }
            }
        }

        // Fetches all non-empty tiles in a row or column
        private List<Tile> FetchTiles(bool isRow, int index, bool isReverse)
        {
            var tiles = new List<Tile>();
            for (var i = 0; i < Size; i++)
            {
                var tile = isRow ? this.tiles[index, isReverse ? Size - 1 - i : i] : this.tiles[isReverse ? Size - 1 - i : i, index];
                if (tile.Value != 0)
                {
                    tiles.Add(tile);
                }
            }
            return tiles;
        }
        
        // Combines tiles if they have the same value and have not been merged this iteration
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
        
        // Checks if there are any possible moves left
        public bool HasMovesLeft()
        {
            // Check for empty tiles
            for (var i = 0; i < Size; i++)
            {
                for (var j = 0; j < Size; j++)
                {
                    if (tiles[i, j].Value == 0) 
                        return true;
                }
            }
            
            // Check for possible merge
            for (var i = 0; i < Size; i++)
            {
                for (var j = 0; j < Size-1; j++)
                {
                    if (tiles[i, j].Value == tiles[i, j+1].Value) 
                        return true;
                }
            }
            
            // Check for possible merge
            for (var j = 0; j < Size; j++)
            {
                for (var i = 0; i < Size-1; i++)
                {
                    if (tiles[i, j].Value == tiles[i+1, j].Value) 
                        return true;
                }
            }
            
            return false;
        }
        
        // Determines if a direction is a valid move
        public bool CanMove(Direction direction, Tile[,] tiles)
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