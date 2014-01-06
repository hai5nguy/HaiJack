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
using HaiJack.Domain;

namespace HaiJack.Components
{
    public partial class PlayerView : UserControl
    {
        public Player Player
        {
            get { return (Player) this.DataContext; }
        }

        public PlayerView()
        {
            InitializeComponent();
        }

        private void btnBet_Click(object sender, RoutedEventArgs e)
        {
            if (txtAmount.Text != "")
            {
                Game.PlayerBets(Player.Id, Convert.ToDouble(txtAmount.Text));
            }
        }

        private void btnHit_Click(object sender, RoutedEventArgs e)
        {
            Game.PlayerHits(Player.Id);
        }

        private void btnStand_Click(object sender, RoutedEventArgs e)
        {
            Game.PlayerStands(Player.Id);
        }

        private void btnDoubleDown_Click(object sender, RoutedEventArgs e)
        {
            Game.PlayerDoublingDown(Player.Id);
        }

        private void btnSplit_Click(object sender, RoutedEventArgs e)
        {
            Game.PlayerSplitting(Player.Id);
        }
    }
}
