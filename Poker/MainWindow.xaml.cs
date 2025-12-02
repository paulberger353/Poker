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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Poker
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Konstruktor des MainWindows
        /// Initialisiert Komponten und die Instanzen der Logikklassen.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
        }


        #region Header Settings, Info, AddBalance, Minimize, Close

        /// <summary>
        /// Methode wird aufgerufen um wieder Balance hinzuzufügen
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddBalance_Click(object sender, RoutedEventArgs e) { }

        /// <summary>
        /// MEthode wird aufgerufen um das Fenster für die Einstellungen zu öffnen
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Settings_Click(object sender, RoutedEventArgs e) { }

        /// <summary>
        /// Methode wird aufgerufen um das Fenster für die Infos zu öffnen
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Infos_Click(object sender, RoutedEventArgs e) 
        {
            //Öffne Info Window
            new InfoWindow().Show();
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
            //Frage ob er wirklich schließen will
            MessageBoxResult yn = MessageBox.Show("Möchtest du das Programm wirklich schließen?", "Anwendung wird beendet...", MessageBoxButton.YesNo);
            if (yn == MessageBoxResult.Yes)
            {
                this.Close();
            }           
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        #endregion
    }
}
