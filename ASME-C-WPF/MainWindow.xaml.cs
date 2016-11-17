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
        private int active_user = Properties.Settings.Default.Active_user;
        public MainWindow()
        {
            
            InitializeComponent();
            listBox.SelectedItem = listBox.Items.GetItemAt(1);
            user.Content = db.master_users.FirstOrDefault(c => c.Id == Properties.Settings.Default.Active_user).name;
            Thread.CurrentThread.CurrentCulture = new CultureInfo("id-ID");
            client_name.Content = ASME_C_WPF.Properties.Settings.Default.Client_Name;
            security_barrier();
        }

        private void security_barrier()
        {
            int role = db.master_users.FirstOrDefault(c => c.Id == active_user).role;
            if (role == 2 || role == 1)
            {
                stock.Visibility = Visibility.Visible;
                ui_control_bahan_baku.Visibility = Visibility.Visible;
                ui_control_product.Visibility = Visibility.Visible;
                finance.Visibility = Visibility.Visible;
                ui_control_pemasukan.Visibility = Visibility.Visible;
                ui_control_pengeluaran.Visibility = Visibility.Visible;
                report.Visibility = Visibility.Visible;
                ui_control_finance_dash.Visibility = Visibility.Visible;
            }
            else
            {
                if(role == 3)
                {
                    stock.Visibility = Visibility.Visible;
                    ui_control_bahan_baku.Visibility = Visibility.Visible;
                    ui_control_product.Visibility = Visibility.Visible;
                    finance.Visibility = Visibility.Visible;
                    ui_control_pemasukan.Visibility = Visibility.Visible;
                    ui_control_pengeluaran.Visibility = Visibility.Visible;
                }else
                {
                    if (role == 4)
                    {
                        stock.Visibility = Visibility.Visible;
                        ui_control_bahan_baku.Visibility = Visibility.Visible;
                        ui_control_product.Visibility = Visibility.Visible;
                    }else
                    {
                        
                    }
                }
            }
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

        private void ui_setting_Selected(object sender, RoutedEventArgs e)
        {
            frame1.Source = new System.Uri("/ui/ui_settings.xaml", UriKind.RelativeOrAbsolute);
        }

        private void ui_control_finance_dash_Selected(object sender, RoutedEventArgs e)
        {

        }

        private void MaterialIcon_MouseEnter(object sender, MouseEventArgs e)
        {
            logout_buton.Foreground = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FF2196F3"));
        }
        private void logout_buton_MouseLeave(object sender, MouseEventArgs e)
        {
            logout_buton.Foreground = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFFFFFFF"));
        }

        private void MaterialIcon_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            
            Login login = new Login();
            ASME_C_WPF.Properties.Settings.Default.Active_user = 1;
            ASME_C_WPF.Properties.Settings.Default.Save();
            login.Show();
            this.Close();
        }

       
    }
}
