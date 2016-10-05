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

namespace ASME_C_WPF
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Login : Window
    {
        public Login()
        {
            InitializeComponent();
            this.Hide();
            splash sps = new splash();
            sps.nteu();
            this.Show();
        }

        private void clicked(object sender, RoutedEventArgs e)
        {
            var username = this.FindName("username");
            
            MainWindow main = new MainWindow();
            main.Show();
            this.Close();
        }
    }
}
