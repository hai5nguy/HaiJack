using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// <summary>
    /// Interaction logic for HandView.xaml
    /// </summary>
    public partial class HandView : UserControl
    {
        public HandView() : this(true) { }

        public HandView(bool showBet)
        {
            InitializeComponent();

            stpBet.Visibility = showBet ? Visibility.Visible : Visibility.Hidden;

        }

    }
}
