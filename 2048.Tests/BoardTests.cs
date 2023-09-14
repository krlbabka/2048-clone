using NUnit.Framework;
using _2048.Models;

namespace _2048.Tests
{
    [TestFixture]
    public class BoardTests
    {
        private Board _board;

        [SetUp]
        public void Setup()
        {
            _board = new Board();
        }

        [Test]
        public void ResetBoard_InitializesBoardWithTwoTiles()
        {
            // Setup
            int nonZeroTiles = 0;

            _board.ResetBoard();

            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (_board.GetTileAt(i, j).Value > 0)
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
            _board.ResetBoard();

            // Assert
            Assert.AreEqual(0, _board.Score);
        }

        [Test]
        public void MoveLeft_CombinesTilesCorrectly()
        {
            // Setup
            // 2 2 0 0
            // 0 0 0 0
            // 0 0 0 0
            // 0 0 0 0
            _board.GetTileAt(0, 0).Value = 2;
            _board.GetTileAt(0, 1).Value = 2;

            _board.Move(Board.Direction.Left);

            // Assert
            // 4 0 0 0
            // 0 0 0 0
            // 0 0 0 0
            // 0 0 0 0
            Assert.AreEqual(4, _board.GetTileAt(0, 0).Value);
            Assert.AreEqual(0, _board.GetTileAt(0, 1).Value);
        }

        [Test]
        public void HasMovesLeft_ReturnsFalseWhenNoMovesExist()
        {
            // Setup: A board where no moves are possible.
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    _board.GetTileAt(i, j).Value = (i + j) % 2 == 0 ? 4 : 2;
                }
            }

            // Assert
            Assert.IsFalse(_board.HasMovesLeft());
        }
        
        [Test]
        public void Move_IncreasesScoreWhenTilesCombine()
        {
            // Setup
            // 2 2 0 0
            // 0 0 0 0
            // 0 0 0 0
            // 0 0 0 0
            _board.GetTileAt(0, 0).Value = 2;
            _board.GetTileAt(0, 1).Value = 2;

            _board.Move(Board.Direction.Right);

            // Assert
            // 0 0 0 4
            // 0 0 0 0
            // 0 0 0 0
            // 0 0 0 0
            Assert.AreEqual(4, _board.Score);
            Assert.AreEqual(4, _board.GetTileAt(0, 3).Value);
        }
        
        [Test]
        public void Move_TilesHuggingSideNoScoreIncrease()
        {
            // Setup
            // 4 2 0 0
            // 2 0 0 0
            // 0 0 0 0
            // 0 0 0 0
            _board.GetTileAt(0, 0).Value = 4;
            _board.GetTileAt(1, 0).Value = 2;
            _board.GetTileAt(1, 0).Value = 2;

            _board.Move(Board.Direction.Up);

            // Assert
            // 4 2 0 0
            // 2 0 0 0
            // 0 0 0 0
            // 0 0 0 0
            Assert.AreEqual(0, _board.Score);
        }
    }
}