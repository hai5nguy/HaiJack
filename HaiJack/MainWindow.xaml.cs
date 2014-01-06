using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using HaiJack.Components;
using HaiJack.Domain;

namespace HaiJack
{
    public partial class MainWindow : Window
    {

        
        public MainWindow()
        {
            InitializeComponent();
            
            icPlayers.ItemsSource = Game.ThePlayers;

            lblNumberOfCardsLeft.DataContext = Game.TheShoe;
            btnAddPlayer.DataContext = Game.Status;
        }

        private void btnAddPlayer_Click(object sender, RoutedEventArgs e)
        {
            Game.AddPlayer();
        }

    }
}
