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
        
        private void UpdateUI()
        {
            BoardGrid.Children.Clear();

            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                {
                    var tile = _gameBoard.GetTileAt(i, j); // You'll need to add this function to your Board class.
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
        
        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            base.OnPreviewKeyDown(e);

            switch (e.Key)
            {
                case Key.Up:
                    _gameBoard.Move(Board.Direction.Up);
                    break;
                case Key.Down:
                    _gameBoard.Move(Board.Direction.Down);
                    break;
                case Key.Left:
                    _gameBoard.Move(Board.Direction.Left);
                    break;
                case Key.Right:
                    _gameBoard.Move(Board.Direction.Right);
                    break;
            }

            UpdateUI();

            if (!_gameBoard.HasMovesLeft())
            {
                GameOverOverlay.Visibility = Visibility.Visible;
            }
        }
        
        private void OnRestartButtonClick(object sender, RoutedEventArgs e)
        {
            StartGame();
        }
        
        private Brush GetBackgroundForValue(int value)
        {
            switch (value)
            {
                case 0: return (SolidColorBrush)new BrushConverter().ConvertFrom("#4D5160");
                case 2: return (SolidColorBrush)new BrushConverter().ConvertFrom("#627A70");
                case 4: return (SolidColorBrush)new BrushConverter().ConvertFrom("#62747A");
                case 8: return (SolidColorBrush)new BrushConverter().ConvertFrom("#626C7A");
                case 16: return (SolidColorBrush)new BrushConverter().ConvertFrom("#62637A");
                case 32: return (SolidColorBrush)new BrushConverter().ConvertFrom("#69627A");
                case 64: return (SolidColorBrush)new BrushConverter().ConvertFrom("#71627A");
                case 128: return (SolidColorBrush)new BrushConverter().ConvertFrom("#7A627A");
                case 256: return (SolidColorBrush)new BrushConverter().ConvertFrom("#7A627A");
                case 512: return (SolidColorBrush)new BrushConverter().ConvertFrom("#7A627A");
                case 1024: return (SolidColorBrush)new BrushConverter().ConvertFrom("#7A627A");
                case 2048: return (SolidColorBrush)new BrushConverter().ConvertFrom("#7A627A");
                default: return (SolidColorBrush)new BrushConverter().ConvertFrom("#7A627A");
            }
        }

        private Brush GetForegroundForValue(int value)
        {
            switch (value)
            {
                default: return Brushes.White;
            }
        }
    }
}