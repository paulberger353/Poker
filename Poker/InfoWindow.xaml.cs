using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Poker
{
    /// <summary>
    /// Logik für das InfoWindow
    /// </summary>
    public partial class InfoWindow : Window
    {
        /// <summary>
        /// Konstruktor des InfoWindows, initialisiert Komponenten
        /// </summary>
        public InfoWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Methode wird aufgerufen um das Fenster zu minimieren
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Minimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        /// <summary>
        /// Methode wird aufgerufen um das Fenster zu schließen
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Methode wiurd aufgerufen, damit der Spieler das Fenster überall halten und verschieben kann
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }
    }
}
