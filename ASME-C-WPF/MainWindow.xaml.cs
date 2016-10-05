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


namespace ASME_C_WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            
            InitializeComponent();
        }

        private void dash_control(object sender, RoutedEventArgs e)
        {
            
            frame1.Source = new System.Uri("/ui/ui_dash.xaml", UriKind.RelativeOrAbsolute);

        }

        private void ui_kasir(object sender, RoutedEventArgs e)
        {
            frame1.Source = new System.Uri("/ui/ui_cashier.xaml", UriKind.RelativeOrAbsolute);
        }

        private void _ui_bb(object sender, RoutedEventArgs e)
        {
            frame1.Source = new System.Uri("/ui/ui_bahan_baku.xaml", UriKind.RelativeOrAbsolute);
        }
    }
}
