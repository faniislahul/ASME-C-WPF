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
using System.Threading;
using System.Globalization;
using ASME_C_WPF.core;

namespace ASME_C_WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        CoreDataContext db = new CoreDataContext();
        public MainWindow()
        {
            
            InitializeComponent();
            listBox.SelectedItem = listBox.Items.GetItemAt(1);
            user.Content = db.master_users.FirstOrDefault(c => c.Id == Properties.Settings.Default.Active_user).name;
            Thread.CurrentThread.CurrentCulture = new CultureInfo("id-ID");
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

        private void ui_product(object sender, RoutedEventArgs e)
        {
            frame1.Source = new System.Uri("/ui/ui_produk.xaml", UriKind.RelativeOrAbsolute);
        }

        private void ui_control_pengeluaran_Selected(object sender, RoutedEventArgs e)
        {
            frame1.Source = new System.Uri("/ui/ui_pengeluaran.xaml", UriKind.RelativeOrAbsolute);
        }

        private void ui_control_pemasukan_Selected(object sender, RoutedEventArgs e)
        {
            frame1.Source = new System.Uri("/ui/ui_pemasukan.xaml", UriKind.RelativeOrAbsolute);
        }

        private void ui_control_neraca_saldo_Selected(object sender, RoutedEventArgs e)
        {
            ASME_C_WPF.ui.report.buku_besar nc = new ui.report.buku_besar();
            nc.ShowDialog();
        }
    }
}
