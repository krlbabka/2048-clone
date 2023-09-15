using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using _2048.Models;

namespace _2048.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private const int Size = 4;
        private Board _gameBoard;
        
        public MainWindow()
        {
            InitializeComponent();
            StartGame();
        }
        
        private void StartGame()
        {
            _gameBoard = new Board();
            GameOverOverlay.Visibility = Visibility.Collapsed;
            UpdateUI();
        }
        
        // Refresh UI based on board's state
        private void UpdateUI()
        {
            BoardGrid.Children.Clear();

            // Populate UI grid with tiles from game board
            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                {
                    var tile = _gameBoard.GetTileAt(i, j);
                    var label = new Label
                    {
                        Content = tile.Value == 0 ? "" : tile.Value.ToString(),
                        Background = GetBackgroundForValue(tile.Value),
                        Foreground = (SolidColorBrush)new BrushConverter().ConvertFrom("#F4EEE0"),
                        VerticalContentAlignment = VerticalAlignment.Center,
                        HorizontalContentAlignment = HorizontalAlignment.Center,
                        Margin = new Thickness(4),
                        FontSize = 24
                    };
                    Grid.SetRow(label, i);
                    Grid.SetColumn(label, j);
                    BoardGrid.Children.Add(label);
                    Score.Text = _gameBoard.Score.ToString();
                }
            }
        }
        
        // Keyboard inputs for tile movements
        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            base.OnPreviewKeyDown(e);

            Board.Direction? moveDirection = null;
            switch (e.Key)
            {
                case Key.Up:
                case Key.W:
                    moveDirection = Board.Direction.Up;
                    break;
                case Key.Down:
                case Key.S:
                    moveDirection = Board.Direction.Down;
                    break;
                case Key.Left:
                case Key.A:
                    moveDirection = Board.Direction.Left;
                    break;
                case Key.Right:
                case Key.D:
                    moveDirection = Board.Direction.Right;
                    break;
            }

            if (moveDirection.HasValue && _gameBoard.CanMove(moveDirection.Value, _gameBoard._tiles))
            {
                _gameBoard.Move(moveDirection.Value);
                _gameBoard.AddRandomTile();
            }

            // Update UI & check end-game condition
            UpdateUI();

            if (!_gameBoard.HasMovesLeft())
            {
                GameOverOverlay.Visibility = Visibility.Visible;
            }
        }
        
        // Game restart button
        private void OnRestartButtonClick(object sender, RoutedEventArgs e)
        {
            StartGame();
        }
        
        // Tile color scheme
        private static readonly Dictionary<int, string> TileColors = new Dictionary<int, string>
        {
            { 0, "#4D5160" },
            { 2, "#627A70" },
            { 4, "#62747A" },
            { 8, "#626C7A" },
            { 16, "#62637A" },
            { 32, "#69627A" },
            { 64, "#71627A" },
            { 128, "#7A627A" },
        };
        private Brush GetBackgroundForValue(int value)
        {
            if (TileColors.TryGetValue(value, out string color)) return (SolidColorBrush)new BrushConverter().ConvertFrom(color);
            
            return (SolidColorBrush)new BrushConverter().ConvertFrom("#7A627A");
        }
    }
}