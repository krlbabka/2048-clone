using NUnit.Framework;
using _2048.Models;

namespace _2048.Tests
{
    [TestFixture]
    public class BoardTests
    {
        private Board board;

        [SetUp]
        public void Setup()
        {
            board = new Board();
            
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    board.GetTileAt(i, j).Value = 0;
                }
            }
        }

        [Test]
        public void ResetBoard_InitializesBoardWithTwoTiles()
        {
            // Setup
            int nonZeroTiles = 0;

            board.ResetBoard();

            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (board.GetTileAt(i, j).Value > 0)
                    {
                        nonZeroTiles++;
                    }
                }
            }

            // Assert
            Assert.AreEqual(2, nonZeroTiles);
        }

        [Test]
        public void ResetBoard_SetsScoreToZero()
        {
            // Act
            board.ResetBoard();

            // Assert
            Assert.AreEqual(0, board.Score);
        }

        [Test]
        public void Move_TilesMoveCorrectlyVertically()
        {
            // Setup
            // 2 0 2 8
            // 0 2 0 0
            // 0 0 0 0
            // 0 0 0 2
            board.GetTileAt(0, 0).Value = 2;
            board.GetTileAt(1, 1).Value = 2;
            board.GetTileAt(0, 2).Value = 2;
            board.GetTileAt(0, 3).Value = 8;
            board.GetTileAt(3, 3).Value = 2;

            board.Move(Board.Direction.Down);
            
            // Assert
            // 0 0 0 0
            // 0 0 0 0
            // 0 0 0 8
            // 2 2 2 2
            Assert.AreEqual(2, board.GetTileAt(3, 0).Value);
            Assert.AreEqual(2, board.GetTileAt(3, 1).Value);
            Assert.AreEqual(2, board.GetTileAt(3, 2).Value);
            Assert.AreEqual(2, board.GetTileAt(3, 3).Value);
            Assert.AreEqual(8, board.GetTileAt(2, 3).Value);
        }
        
        [Test]
        public void Move_TilesMoveCorrectlyHorizontally()
        {
            // Setup
            // 0 0 2 0
            // 0 2 0 0
            // 2 0 4 0
            // 0 0 0 2
            board.GetTileAt(0, 2).Value = 2;
            board.GetTileAt(1, 1).Value = 2;
            board.GetTileAt(2, 0).Value = 2;
            board.GetTileAt(2, 2).Value = 4;
            board.GetTileAt(3, 3).Value = 2;

            board.Move(Board.Direction.Right);
            
            // Assert
            // 0 0 0 2
            // 0 0 0 2
            // 0 0 2 4
            // 0 0 0 2
            Assert.AreEqual(2, board.GetTileAt(0, 3).Value);
            Assert.AreEqual(2, board.GetTileAt(1, 3).Value);
            Assert.AreEqual(2, board.GetTileAt(2, 2).Value);
            Assert.AreEqual(4, board.GetTileAt(2, 3).Value);
            Assert.AreEqual(2, board.GetTileAt(3, 3).Value);
        }

        [Test]
        public void Move_CombinesTilesCorrectlyHorizontally()
        {
            // Setup
            // 2 2 2 2
            // 0 0 0 0
            // 0 0 0 0
            // 0 0 0 0
            board.GetTileAt(0, 0).Value = 2;
            board.GetTileAt(0, 1).Value = 2;
            board.GetTileAt(0, 2).Value = 2;
            board.GetTileAt(0, 3).Value = 2;

            board.Move(Board.Direction.Left);

            // Assert
            // 4 4 0 0
            // 0 0 0 0
            // 0 0 0 0
            // 0 0 0 0
            Assert.AreEqual(4, board.GetTileAt(0, 0).Value);
            Assert.AreEqual(4, board.GetTileAt(0, 1).Value);
            Assert.AreEqual(0, board.GetTileAt(0, 2).Value);
            Assert.AreEqual(0, board.GetTileAt(0, 3).Value);
            Assert.AreEqual(true, board.CanMove(Board.Direction.Left, board.tiles));
        }
        
        [Test]
        public void Move_CombinesTilesCorrectlyHorizontallyNonAdjacent()
        {
            // Setup
            // 2 0 0 2
            // 0 0 0 0
            // 0 0 0 0
            // 0 0 0 0
            board.GetTileAt(0, 0).Value = 2;
            board.GetTileAt(0, 3).Value = 2;

            board.Move(Board.Direction.Right);

            // Assert
            // 0 0 0 4
            // 0 0 0 0
            // 0 0 0 0
            // 0 0 0 0
            Assert.AreEqual(0, board.GetTileAt(0, 0).Value);
            Assert.AreEqual(4, board.GetTileAt(0, 3).Value);
        }
        
        [Test]
        public void Move_CombinesTilesCorrectlyVertically()
        {
            // Setup
            // 2 0 0 0
            // 2 0 0 0
            // 2 0 0 0
            // 2 0 0 0
            board.GetTileAt(0, 0).Value = 2;
            board.GetTileAt(1, 0).Value = 2;
            board.GetTileAt(2, 0).Value = 2;
            board.GetTileAt(3, 0).Value = 2;

            board.Move(Board.Direction.Up);

            // Assert
            // 4 0 0 0
            // 4 0 0 0
            // 0 0 0 0
            // 0 0 0 0
            Assert.AreEqual(4, board.GetTileAt(0, 0).Value);
            Assert.AreEqual(4, board.GetTileAt(1, 0).Value);
            Assert.AreEqual(0, board.GetTileAt(2, 0).Value);
            Assert.AreEqual(0, board.GetTileAt(3, 0).Value);
        }
        
        [Test]
        public void Move_CombinesTilesCorrectlyVerticallyNonAdjacent()
        {
            // Setup
            // 2 0 0 0
            // 0 0 0 0
            // 0 0 0 0
            // 2 0 0 0
            board.GetTileAt(0, 0).Value = 2;
            board.GetTileAt(3, 0).Value = 2;

            board.Move(Board.Direction.Down);

            // Assert
            // 0 0 0 0
            // 0 0 0 0
            // 0 0 0 0
            // 4 0 0 0
            Assert.AreEqual(0, board.GetTileAt(0, 0).Value);
            Assert.AreEqual(4, board.GetTileAt(3, 0).Value);
        }

        [Test]
        public void HasMovesLeft_ReturnsFalseWhenNoMovesExist()
        {
            // Setup: A board where no moves are possible.
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    board.GetTileAt(i, j).Value = (i + j) % 2 == 0 ? 4 : 2;
                }
            }

            // Assert
            Assert.IsFalse(board.HasMovesLeft());
        }
        
        [Test]
        public void Move_IncreasesScoreWhenTilesCombine()
        {
            // Setup
            // 2 2 0 0
            // 0 0 0 0
            // 0 0 0 0
            // 0 0 0 0
            board.GetTileAt(0, 0).Value = 4;
            board.GetTileAt(0, 1).Value = 4;
            board.Score = 0;

            board.Move(Board.Direction.Left);

            // Assert
            // 0 0 0 4
            // 0 0 0 0
            // 0 0 0 0
            // 0 0 0 0
            Assert.AreEqual(8, board.Score);
        }
    }
}