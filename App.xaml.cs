using System.Windows;
using _2048.Models;

namespace _2048
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
    
            Board gameBoard = new Board();
            gameBoard.DebugPlay();

            Shutdown();
        }
    }
}